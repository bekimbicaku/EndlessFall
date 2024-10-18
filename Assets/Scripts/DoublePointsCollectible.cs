using UnityEngine;

public class DoublePointsCollectible : MonoBehaviour
{
    public float duration = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().ActivateDoublePoints(duration);
            CollectibleRespawn collectible = gameObject.GetComponent<CollectibleRespawn>();
            if (collectible != null)
            {
                collectible.Collect();
            }
        }
    }
}