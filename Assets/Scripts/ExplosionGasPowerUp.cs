using UnityEngine;

public class ExplosionGasPowerUp : MonoBehaviour
{
    public GameObject gasBubble = null;

    public float duration = 0.5f;
    private float timer = 0f;

    Color gasColor = Color.black;

    private void Start()
    {
        if (gasBubble == null)
        {
            gasBubble = GetComponent<GameObject>();
        }
        else { }
        gasColor = gasBubble.GetComponent<SpriteRenderer>().color;
    }

    void Update()
    {
        // lower alpha color of gas until it reaches 0
        if (timer < duration)
        {
            timer += Time.deltaTime;
            gasColor.a = 1 - (timer / duration);
            gasBubble.GetComponent<SpriteRenderer>().color = gasColor;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
