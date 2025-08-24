using UnityEngine;

public class PowerUps : MonoBehaviour
{
    public GameObject player = null;

    [Header("PowerUp Type")]
    public bool speedPowerUp = false;
    public bool timeStopPowerUp = false;
    public bool infectionShootPowerUp = false;
    public bool explosionPowerUp = false;

    [Header("Speed")]
    public float speedIncrease = 0.5f;
    public Sprite speedPowerUpSprite = null;

    [Header("Time Stop")]
    public float duration = 5.0f;
    private float timer = 0f;
    private bool timerStopActive = false;
    public Sprite timeStopPowerUpSprite = null;

    [Header("Infection Shoot")]
    public GameObject shootPrefab;
    public Sprite infectionShootPowerUpSprite = null;

    [Header("Explosion")]
    public GameObject explosionPrefab = null;
    public Sprite explosionPowerUpSprite = null;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        int randomPowerUp = Random.Range(0, 4);
        if (randomPowerUp == 0)
        {
            speedPowerUp = true;
            // Set the sprite for the speed power-up
            if (speedPowerUpSprite != null)
            gameObject.GetComponent<SpriteRenderer>().sprite = speedPowerUpSprite;
        }
        else if (randomPowerUp == 1)
        {
            timeStopPowerUp = true;
        }
        else if (randomPowerUp == 2)
        {
            infectionShootPowerUp = true;
            // Set the sprite for the infection shoot power-up
            if (infectionShootPowerUpSprite != null)
            gameObject.GetComponent<SpriteRenderer>().sprite = infectionShootPowerUpSprite;
        }
        else if (randomPowerUp == 3)
        {
            explosionPowerUp = true;
            // Set the sprite for the explosion power-up
            if (explosionPowerUpSprite != null)
            gameObject.GetComponent<SpriteRenderer>().sprite = explosionPowerUpSprite;
        }
        else
        {
            Debug.LogError("No power-up type assigned!");
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != player) return;
        else if (speedPowerUp)
        {
            AudioManager.Instance.PlaySFX("speed");
            other.GetComponent<Movement>().speed += speedIncrease;
            Destroy(gameObject);
        }
        else if (timeStopPowerUp)
        {
            AudioManager.Instance.PlaySFX("clock");
            Timer.Instance.StopTimer();
            timer = 0.0f;
            timerStopActive = true;
            // hide the power sprite and delete collider
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
        else if (infectionShootPowerUp)
        {
            for (int i = 0; i < 4; i++)
            {
                AudioManager.Instance.PlaySFX("shoot");
                // instantiate a prefab
                GameObject enemyToSpawn = Instantiate(shootPrefab, new Vector3(player.transform.position.x, player.transform.position.y, 0), Quaternion.identity);
                // give it a new speed direction
                if (i == 0)
                {
                    enemyToSpawn.GetComponent<ShootPowerBalloonMovement>().newDestination = new Vector2(1, 1).normalized;
                }
                else if (i == 1)
                {
                    enemyToSpawn.GetComponent<ShootPowerBalloonMovement>().newDestination = new Vector2(1, -1).normalized;
                }
                else if (i == 2)
                {
                    enemyToSpawn.GetComponent<ShootPowerBalloonMovement>().newDestination = new Vector2(-1, -1).normalized;
                }
                else if (i == 3)
                {
                    enemyToSpawn.GetComponent<ShootPowerBalloonMovement>().newDestination = new Vector2(-1, 1).normalized;
                }
                else { }
            }
            Destroy(gameObject);
        }
        else if (explosionPowerUp)
        {
            AudioManager.Instance.PlaySFX("explosion");
            GameObject explosion = Instantiate(explosionPrefab, player.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else { }
    }

    private void Update()
    {
        if (timerStopActive)
        {
            timer += Time.deltaTime;
            if (timer >= duration)
            {
                Timer.Instance.ResumeTimer();
                Destroy(gameObject);
            }
        }
    }
}

