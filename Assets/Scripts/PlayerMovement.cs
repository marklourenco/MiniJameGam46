using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5.0f;

    private Vector2 moveInput = Vector2.zero;

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    void Update()
    { 
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    void FixedUpdate()
    {
        Vector2 targetVelocity = moveInput * speed;

        if (!GameInstance.Instance.gameEnd)
        {
            rb.linearVelocity = targetVelocity;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}