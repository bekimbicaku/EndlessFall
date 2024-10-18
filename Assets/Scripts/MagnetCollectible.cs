using UnityEngine;

public class MagnetCollectible : MonoBehaviour
{
    public float duration = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().ActivateMagnet(duration);
            Destroy(gameObject);
        }
    }
}
