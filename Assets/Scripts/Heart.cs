using UnityEngine;

public class Heart : MonoBehaviour
{
    public int healthRestoreAmount = 10; // The amount of health the heart restores

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.instance.PlaySFX("ShieldPickup");

            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.RestoreHealth(healthRestoreAmount);
            }
            CollectibleRespawn collectible = gameObject.GetComponent<CollectibleRespawn>();
            if (collectible != null)
            {
                collectible.Collect();
            }
        }
    }
}
