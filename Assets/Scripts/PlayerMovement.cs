using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rb = null;
    public float speed = 5.0f;

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
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isRunning)
        {
            isRunning = true;
            speed *= 1.5f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && isRunning)
        {
            isRunning = false;
            speed /= 1.5f;
        }
    }

    private void FixedUpdate()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 moveVelocity = moveInput.normalized * speed;
        rb.linearVelocity = new Vector2(moveVelocity.x, moveVelocity.y);
    }
}
