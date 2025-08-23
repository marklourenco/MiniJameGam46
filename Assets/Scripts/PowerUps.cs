using UnityEngine;

public class PowerUps : MonoBehaviour
{
    public float speedIncrease = 5.0f;
    public float duration = 5.0f;
    private bool isActive = false;
    private Movement playerRef = null;
    private float timer = 0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive)
        {
            Movement player = other.GetComponent<Movement>();
            if (player != null)
            {
                isActive = true;
                playerRef = player;
                timer = duration;
                playerRef.speed += speedIncrease;
                Timer.Instance.StopTimer();
                // Hide the sprite renderer
                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                if (sr != null) sr.enabled = false;
            }
        }
    }

    private void Update()
    {
        if (isActive)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f && playerRef != null)
            {
                playerRef.speed -= speedIncrease;
                Timer.Instance.ResumeTimer();
                Destroy(gameObject);
            }
        }
    }
}

