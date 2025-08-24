using Unity.VisualScripting;
using UnityEngine;

public class EnemyInfect : MonoBehaviour
{
    public Sprite enemyInfected;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && this.gameObject.CompareTag("Infected"))
        {
            // change Tag
            other.GetComponent<Animator>().SetBool("Infected", true);
            other.tag = "Infected";
            Debug.Log("Balloon has been infected!");
            // score
            GameInstance.Instance.EnemyInfected(other.gameObject);
        }
    }
}
