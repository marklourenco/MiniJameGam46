using UnityEngine;

public class ShootPowerBalloonMovement : MonoBehaviour
{
    public Rigidbody2D rb = null;

    public float duration = 5f;
    private float timer = 0.0f;

    public float speed = 1.0f;

    public Vector2 newDestination = Vector2.zero;

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
        if (rb != null)
        {
            timer += Time.deltaTime;
            if (timer >= duration)
            {
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!GameInstance.Instance.gameEnd)
        {
            rb.linearVelocity = newDestination.normalized * speed;
        }
        else
        {
            // Stop movement when the game ends
            rb.linearVelocity = Vector2.zero;
        }
    }
}
