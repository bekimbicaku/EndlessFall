using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public float spawnRate = 1f;
    public float obstacleSpeed = 2f;

    private float nextSpawnTime;

    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            SpawnObstacle();
            nextSpawnTime = Time.time + 1f / spawnRate;
        }
    }

    void SpawnObstacle()
    {
        GameObject obstacle = Instantiate(obstaclePrefab, new Vector3(Random.Range(-2.5f, 2.5f), transform.position.y, 0), Quaternion.identity);
        Rigidbody2D rb = obstacle.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(0, -obstacleSpeed);
    }
}
