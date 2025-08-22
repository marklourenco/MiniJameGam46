using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rb = null;
    public float speed = 5.0f;
    public float runMultiplier = 1.5f;

    private Vector2 moveInput = Vector2.zero;
    private bool isRunning = false;

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
        // Read input in Update
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // Running toggle
        isRunning = Input.GetKey(KeyCode.LeftShift);
    }

    private void FixedUpdate()
    {
        speed = isRunning ? speed * runMultiplier : speed;
        Vector2 targetVelocity = moveInput * speed;

        if (!GameInstance.Instance.gameEnd)
        {
        // Apply directly (snappy, but smooth enough if in Update)
        rb.linearVelocity = targetVelocity;
        }
        else
        {
            // Stop movement when the game ends
            rb.linearVelocity = Vector2.zero;
        }
    }
}
