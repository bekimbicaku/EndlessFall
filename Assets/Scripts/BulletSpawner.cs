using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;  // Prefab p�r plumbin
    public Transform firePoint;      // Pika nga ku gjuan gryka
    public float spawnRate = 1.0f;   // Shkalla e krijimit t� plumbave
    private float nextSpawnTime;

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnBullet();
            nextSpawnTime = Time.time + 1f / spawnRate;
        }
    }

    void SpawnBullet()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
