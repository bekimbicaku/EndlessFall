using UnityEngine;
public class ShrinkCollectible : MonoBehaviour
{
    public float duration = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().ActivateShrink(duration);
            Destroy(gameObject);
        }
    }
}
