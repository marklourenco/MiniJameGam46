using Unity.VisualScripting;
using UnityEngine;

public class PlayerInfect : MonoBehaviour
{
    public Sprite enemyInfected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // animator boolean to infected
            other.GetComponent<Animator>().SetBool("Infected", true);
            other.tag = "Infected";
            Debug.Log("Balloon has been infected!");
            // score
            GameInstance.Instance.EnemyInfected(other.gameObject);
        }
    }
}
