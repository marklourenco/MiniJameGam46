using Unity.VisualScripting;
using UnityEngine;

public class PlayerInfect : MonoBehaviour
{
    public Sprite enemyInfected;

    // When the object collides with an object tagged as "Enemy", they become infected.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Change Animator boolean to infected
            other.GetComponent<Animator>().SetBool("Infected", true);
            other.tag = "Infected";
            Debug.Log("Balloon has been infected!");
            // Up score
            GameInstance.Instance.EnemyInfected(other.gameObject);
        }
    }
}
