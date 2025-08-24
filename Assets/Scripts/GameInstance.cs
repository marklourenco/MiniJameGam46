using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInstance : MonoBehaviour
{
    private static GameInstance instance = null;
    public static GameInstance Instance { get { return instance; } }

    // top left score stuff
    public TMP_Text scoreText = null;
    public TMP_Text finalScoreText = null;
    private int score = 0;

    // list of infected balloons
    public List<GameObject> infectedBalloons = new List<GameObject>();

    // camera and camera movement refs/values
    public Camera mainCamera;
    private float switchCameraTimer = 2.0f;
    private float switchCameraTimerCurrent = 0.0f;

    // game end flag
    public bool gameEnd = false;

    // UI references
    public GameObject gameOverScreen = null;
    public GameObject duringGameUI = null;

    // timer ref
    public GameObject timer = null;

    // list of enemies at spawn
    private List<GameObject> enemyPrefabs = new List<GameObject>();
    public GameObject enemyPrefab = null;
    public int enemyAmount = 0;

    // explosion values
    private float explosionRadius = 40.0f;
    private List<GameObject> pendingExplosions = new List<GameObject>();
    private float explosionDelayTimer = 0f;
    private bool waitingForExplosion = false;
    public float explosionDelay = 2f;

    // power up refs
    private List<GameObject> powerUpPrefabs = new List<GameObject>();
    public GameObject powerUpPrefab = null;
    public int powerUpAmount = 0;

    public GameObject player = null;

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
    public void Initialize()
    {
        // start of game, sets everything up
        Restart();
        AudioManager.Instance.SetMusicVolume(1f);
        AudioManager.Instance.SetSFXVolume(0.5f);
    }

    private void Update()
    {
        if (gameEnd && infectedBalloons.Count > 0)
        {
            AudioManager.Instance.StopMusic();
            // delay for camera movement
            if (waitingForExplosion)
            {
                // delay count-up
                explosionDelayTimer += Time.deltaTime;

                if (explosionDelayTimer >= explosionDelay)
                {
                    // delete balloons after delay
                    foreach (GameObject balloon in pendingExplosions)
                    {
                        if (balloon != null)
                        {
                            Debug.Log("Explosion triggered at: " + balloon.name);
                            Destroy(balloon);
                            infectedBalloons.Remove(balloon);
                        }
                    }
                    AudioManager.Instance.PlaySFX("balloon pop");

                    pendingExplosions.Clear();
                    waitingForExplosion = false;
                    explosionDelayTimer = 0f;
                }
            }
            else
            {
                // normal cluster explosion pacing
                switchCameraTimerCurrent += Time.deltaTime;

                if (switchCameraTimerCurrent >= switchCameraTimer)
                {
                    Explode(0); // explode first available cluster
                    switchCameraTimerCurrent = 0f;
                }
            }
        }
        else if (gameEnd && infectedBalloons.Count == 0)
        {
            switchCameraTimerCurrent += Time.deltaTime;

            if (switchCameraTimerCurrent >= switchCameraTimer)
            {
                // switch UI
                duringGameUI.SetActive(false);
                gameOverScreen.SetActive(true);
                finalScoreText.text = score.ToString();

                HighscoreManager.SaveHighscore(score);
            }
        }
    }

    // keep score of infected balloons and update UI and adds to list
    public void EnemyInfected(GameObject other)
    {
        score++;
        scoreText.text = "-    " + score.ToString();

        infectedBalloons.Add(other);
    }

    // explosion logic
    public void Explode(int i)
    {
        if (i < 0 || i >= infectedBalloons.Count) return;

        GameObject centerBalloon = infectedBalloons[i];
        Vector3 explosionCenter = centerBalloon.transform.position;

        // move camera right away
        mainCamera.GetComponent<PlayerCamera>().player = null;
        mainCamera.transform.position = new Vector3(explosionCenter.x, explosionCenter.y, mainCamera.transform.position.z);

        // start delay
        waitingForExplosion = true;
        explosionDelayTimer = 0f;

        // collect balloons within radius
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

    // new game, resets values
    public void Restart()
    {
        foreach (GameObject prefab in enemyPrefabs)
        {
            Destroy(prefab);
        }
        enemyPrefabs.Clear();
        foreach (GameObject prefab in powerUpPrefabs)
        {
            Destroy(prefab);
        }
        powerUpPrefabs.Clear();

        SpawnEnemies();
        SpawnPowerUps();
        GetComponent<Timer>().SetCurrentTimer(30);
        GetComponent<Timer>().ResumeTimer();
        score = 0;
        scoreText.text = "-    " + score.ToString();
        infectedBalloons.Clear();
        gameEnd = false;
        duringGameUI.SetActive(true);
        gameOverScreen.SetActive(false);
        switchCameraTimerCurrent = 0.0f;
        player.transform.position = new Vector3(0, 0, 0);
        mainCamera.GetComponent<PlayerCamera>().Restart();
        player.GetComponent<Movement>().speed = 5.0f;
    }

    // start of game creates enemies
    public void SpawnEnemies()
    {
        for (int i = 0; i < enemyAmount; i++)
        {
            GameObject enemyToSpawn = Instantiate(enemyPrefab, new Vector3(Random.Range(-70f, 70f), Random.Range(-70f, 45f), 0), Quaternion.identity);
            enemyPrefabs.Add(enemyToSpawn);
        }
    }

    public void SpawnPowerUps()
    {
        for (int i = 0; i < powerUpAmount; i++)
        {
            GameObject powerUpToSpawn = Instantiate(powerUpPrefab, new Vector3(Random.Range(-70f, 70f), Random.Range(-70f, 45f), 0), Quaternion.identity);
            powerUpPrefabs.Add(powerUpToSpawn);
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayLoopMusic()
    {
        AudioManager.Instance.PlayMusic("loop music", true);
    }
}
