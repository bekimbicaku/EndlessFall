using UnityEngine;
using System.Collections;

public class CollectibleRespawn : MonoBehaviour
{
    // Time after which the collectible will respawn
    public float respawnTime = 5f;

    // Original spawn position of the collectible
    private Vector3 originalPosition;
    private bool isCollected = false;

    private void Start()
    {
        // Store the original position of the collectible
        originalPosition = transform.position;
    }

    // This function is called when the player collects the collectible
    public void Collect()
    {
        if (isCollected)
            return;

        isCollected = true;
        // Hide or destroy the collectible (can use SetActive(false) instead of Destroy)
        gameObject.SetActive(false);

        // Start the coroutine to respawn the collectible after a delay
        RespawnManager.Instance.StartRespawnCoroutine(this);
    }

    public void Respawn()
    {
        // Respawn the collectible at its original position
        transform.position = originalPosition;

        // Reactivate the collectible
        gameObject.SetActive(true);
        isCollected = false;
    }
}
