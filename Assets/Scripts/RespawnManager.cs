using UnityEngine;
using System.Collections;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance { get; private set; }

    private void Awake()
    {
        // Ensure that there's only one instance of RespawnManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartRespawnCoroutine(CollectibleRespawn collectible)
    {
        StartCoroutine(RespawnCoroutine(collectible));
    }

    private IEnumerator RespawnCoroutine(CollectibleRespawn collectible)
    {
        // Wait for the respawn time
        yield return new WaitForSeconds(collectible.respawnTime);

        // Call the Respawn method on the collectible
        collectible.Respawn();
    }
}
