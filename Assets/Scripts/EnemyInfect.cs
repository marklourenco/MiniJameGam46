using Unity.VisualScripting;
using UnityEngine;

public class EnemyInfect : MonoBehaviour
{
    public Sprite enemyInfected;

    // When the object collides with an object tagged as "Enemy", they become infected.
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && this.gameObject.CompareTag("Infected"))
        {
            // Change Tag
            other.GetComponent<SpriteRenderer>().sprite = enemyInfected;
            other.tag = "Infected";
            Debug.Log("Balloon has been infected!");
            // Up score
            GameInstance.Instance.EnemyInfected(other.gameObject);
        }
    }
}
