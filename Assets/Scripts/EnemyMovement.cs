using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyMovement : MonoBehaviour
{
    public Rigidbody2D rb = null;

    public float wanderTimer = 5f;
    private float timeSinceLastWander = 0f;

    public float speed = 1.0f;

    public Vector2 newDestination = Vector2.zero;

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        SetRandomDestination();
    }

    void Update()
    {
        timeSinceLastWander += Time.deltaTime;

        if (timeSinceLastWander >= wanderTimer)
        {
            timeSinceLastWander = 0f;
            SetRandomDestination();
        }
    }

    void SetRandomDestination()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);
        newDestination = new Vector2(randomX, randomY);
        newDestination.Normalize();
    }

    private void FixedUpdate()
    {
        if (!GameInstance.Instance.gameEnd)
        {
            newDestination = (newDestination).normalized;
            newDestination = newDestination * speed;
            rb.linearVelocity = new Vector2(newDestination.x, newDestination.y);
        }
        else
        {
            // stop movement when the game ends
            rb.linearVelocity = Vector2.zero;
        }
    }
}