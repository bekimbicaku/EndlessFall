using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallTrigger : MonoBehaviour
{
    public enum ObstacleType { Bullet, FireParticle, Water, Smoke, Lighten }
    public ObstacleType obstacleType;
    public GameObject bulletPrefab;
    public GameObject fireParticlePrefab;
    public GameObject WaterPrefab;
    public GameObject SmokePrefab;
    public GameObject lightenPrefab;
    public Quaternion desiredRotation;
    public Transform firePoint;
    public int numberOfObstacles = 3;
    public float timeBetweenObstacles = 0.5f;
    public float respawnDelay = 2.0f; // Koha e vonesës para se efektet të shfaqen përsëri

    private bool hasSpawned = false;
    private bool isCollidingWithPlayer = false;
    private int obstaclesSpawned = 0;
    private float nextSpawnTime;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerTrigger"))
        {
            isCollidingWithPlayer = true;
            if (!hasSpawned)
            {
                nextSpawnTime = Time.time;
                obstaclesSpawned = 0;
                hasSpawned = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerTrigger"))
        {
            isCollidingWithPlayer = false;
            hasSpawned = false; // Reset hasSpawned kur player largohet
        }
    }

    private void Update()
    {
        if (hasSpawned && obstaclesSpawned < numberOfObstacles)
        {
            if (Time.time >= nextSpawnTime)
            {
                SpawnObstacle();
                obstaclesSpawned++;
                nextSpawnTime = Time.time + timeBetweenObstacles;

                if (obstaclesSpawned >= numberOfObstacles && isCollidingWithPlayer)
                {
                    StartCoroutine(RespawnObstacles());
                }
            }
        }
    }

    private IEnumerator RespawnObstacles()
    {
        yield return new WaitForSeconds(respawnDelay);
        obstaclesSpawned = 0;
        nextSpawnTime = Time.time;
    }

    void SpawnObstacle()
    {
        switch (obstacleType)
        {
            case ObstacleType.Bullet:
                SpawnBullet();
                break;
            case ObstacleType.FireParticle:
                SpawnFireParticles();
                break;
            case ObstacleType.Water:
                SpawnWaterParticles();
                break;
            case ObstacleType.Smoke:
                SpawnSmokeParticles();
                break;
            case ObstacleType.Lighten:
                SpawnLightenParticles();
                break;
        }
    }

    void SpawnBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Vector2 bulletDirection = (firePoint.position - transform.position).normalized;
        HorizontalMover mover = bullet.GetComponent<HorizontalMover>();
        if (mover != null)
        {
            mover.SetSpeedAndDirection(mover.bulletSpeed, bulletDirection);
        }
    }

    void SpawnWaterParticles()
    {
        if (WaterPrefab != null && firePoint != null)
        {
            GameObject explosion = Instantiate(WaterPrefab, firePoint.position, desiredRotation);
            ParticleSystem ps = explosion.GetComponent<ParticleSystem>();

            if (ps != null)
            {
                var main = ps.main;
                main.loop = false;
                Destroy(explosion, ps.main.duration);
            }
        }
    }
    void SpawnSmokeParticles()
    {
        if (WaterPrefab != null && firePoint != null)
        {
            GameObject explosion = Instantiate(SmokePrefab, firePoint.position, desiredRotation);
            ParticleSystem ps = explosion.GetComponent<ParticleSystem>();

            if (ps != null)
            {
                var main = ps.main;
                main.loop = false;
                Destroy(explosion, ps.main.duration);
            }
        }
    }
    void SpawnLightenParticles()
    {
        if (WaterPrefab != null && firePoint != null)
        {
            GameObject explosion = Instantiate(lightenPrefab, firePoint.position, desiredRotation);
            ParticleSystem ps = explosion.GetComponent<ParticleSystem>();

            if (ps != null)
            {
                var main = ps.main;
                main.loop = false;
                Destroy(explosion, ps.main.duration);
            }
        }
    }

    void SpawnFireParticles()
    {
        if (fireParticlePrefab != null && firePoint != null)
        {
            GameObject explosion = Instantiate(fireParticlePrefab, firePoint.position, desiredRotation);
            ParticleSystem ps = explosion.GetComponent<ParticleSystem>();

            if (ps != null)
            {
                var main = ps.main;
                main.loop = false;
                Destroy(explosion, ps.main.duration);
            }
        }
    }
}
