using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform player;
    public float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 targetPosition = new Vector3(player.position.x, player.position.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }

    public void Restart()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
