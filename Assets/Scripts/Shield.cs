using UnityEngine;
using System.Collections;
public class Shield : MonoBehaviour
{
    public GameObject explosionEffect;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle") || other.CompareTag("Bullet"))
        {
            Explode(other.transform.position);
            SoundManager.instance.PlaySFX("Explosion");

            CollectibleRespawn collectible = other.gameObject.GetComponent<CollectibleRespawn>();
            if (collectible != null)
            {
                collectible.Collect();
            }
        }
    }

    public void Explode(Vector2 position)
    {
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, position, Quaternion.identity);
            ParticleSystem ps = explosion.GetComponent<ParticleSystem>();

            if (ps != null)
            {

                Destroy(explosion, ps.main.duration);
            }
            else
            {
                Debug.LogWarning("No ParticleSystem component found on explosionEffect prefab.");
            }
        }
        else
        {
            Debug.LogWarning("Explosion effect is not assigned.");
        }
    }

}
