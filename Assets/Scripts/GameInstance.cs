using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
    private static GameInstance instance = null;
    public static GameInstance Instance { get { return instance; } }

    public TMP_Text scoreText = null;
    public TMP_Text finalScoreText = null;
    private int score = 0;

    // For enemy balloons that get infected
    public List<GameObject> infectedBalloons = new List<GameObject>();

    public Camera mainCamera;
    private float switchCameraTimer = 2.0f;
    private float switchCameraTimerCurrent = 0.0f;

    public bool gameEnd = false;

    public GameObject gameOverScreen = null;
    public GameObject duringGameUI = null;

    public GameObject timer = null;

    // List of enemy prefabs to spawn at the start
    private List<GameObject> enemyPrefabs = new List<GameObject>();
    public GameObject enemyPrefab = null;
    public int enemyAmount = 40;

    private float explosionRadius = 15.0f;
    private List<GameObject> pendingExplosions = new List<GameObject>();
    private float explosionDelayTimer = 0f;
    private bool waitingForExplosion = false;

    public float explosionDelay = 2f; // tweak in inspector


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            instance.Initialize();
        }
    }

    private int currentBalloonIndex = 0;

    private void Update()
    {
        if (gameEnd && infectedBalloons.Count > 0)
        {
            // If we're waiting to blow up a cluster
            if (waitingForExplosion)
            {
                explosionDelayTimer += Time.deltaTime;

                if (explosionDelayTimer >= explosionDelay)
                {
                    // Delete balloons after delay
                    foreach (GameObject balloon in pendingExplosions)
                    {
                        if (balloon != null)
                        {
                            Debug.Log("Explosion triggered at: " + balloon.name);
                            Destroy(balloon);
                            infectedBalloons.Remove(balloon);
                        }
                    }

                    pendingExplosions.Clear();
                    waitingForExplosion = false;
                    explosionDelayTimer = 0f;
                }
            }
            else
            {
                // Normal cluster explosion pacing
                switchCameraTimerCurrent += Time.deltaTime;

                if (switchCameraTimerCurrent >= switchCameraTimer)
                {
                    Explode(0); // Explode first available cluster
                    switchCameraTimerCurrent = 0f;
                }
            }
        }
        else if (gameEnd && infectedBalloons.Count == 0)
        {
            switchCameraTimerCurrent += Time.deltaTime;

            if (switchCameraTimerCurrent >= switchCameraTimer)
            {
                duringGameUI.SetActive(false);
                gameOverScreen.SetActive(true);
                finalScoreText.text = "Final Score: " + score.ToString();
            }
        }
    }



    private void OnDestroy()
    {
        
    }

    public void Initialize()
    {
        Restart();
    }

    public void EnemyInfected(GameObject other)
    {
        score++;
        scoreText.text = "-    " + score.ToString();

        infectedBalloons.Add(other);
    }

    public void Explode(int i)
    {
        if (i < 0 || i >= infectedBalloons.Count) return;

        GameObject centerBalloon = infectedBalloons[i];
        Vector3 explosionCenter = centerBalloon.transform.position;

        // Move camera right away
        mainCamera.GetComponent<PlayerCamera>().player = null;
        mainCamera.transform.position = new Vector3(explosionCenter.x, explosionCenter.y, mainCamera.transform.position.z);

        // Start delay
        waitingForExplosion = true;
        explosionDelayTimer = 0f;

        // Collect balloons within radius
        pendingExplosions.Clear();
        foreach (GameObject balloon in infectedBalloons)
        {
            if (balloon == null) continue;
            float distance = Vector3.Distance(balloon.transform.position, explosionCenter);
            if (distance <= explosionRadius)
            {
                pendingExplosions.Add(balloon);
            }
        }
    }


    public void Restart()
    {
        foreach (GameObject prefab in enemyPrefabs)
        {
            Destroy(prefab);
        }
        enemyPrefabs.Clear();

        SpawnEnemies();
        GetComponent<Timer>().SetCurrentTimer(15);
        GetComponent<Timer>().ResumeTimer();
        score = 0;
        scoreText.text = "-    " + score.ToString();
        infectedBalloons.Clear();
        gameEnd = false;
        duringGameUI.SetActive(true);
        gameOverScreen.SetActive(false);
        currentBalloonIndex = 0;
        switchCameraTimerCurrent = 0.0f;
        mainCamera.GetComponent<PlayerCamera>().Restart();
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < enemyAmount; i++)
        {
            GameObject enemyToSpawn = Instantiate(enemyPrefab, new Vector3(Random.Range(-40f, 40f), Random.Range(-40f, 40f), 0), Quaternion.identity);
            enemyPrefabs.Add(enemyToSpawn);
        }
    }
}
