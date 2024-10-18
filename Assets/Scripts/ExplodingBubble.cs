using UnityEngine;
using System.Collections;

public class ExplodingBubble : MonoBehaviour
{
    public float growthRate = 1f; // Shpejtësia e rritjes
    public float maxSize = 5f; // Madhësia maksimale e flluskës
    public float explosionDelay = 1f; // Koha për shpërthim
    public GameObject explosionEffect; // Efekti i shpërthimit

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerTrigger"))
        {
            StartCoroutine(GrowAndExplode());
        }
       
    }
    private IEnumerator GrowAndExplode()
    {
        // Rrit flluskën deri në madhësinë maksimale
        while (transform.localScale.x < maxSize)
        {
            transform.localScale += Vector3.one * growthRate * Time.deltaTime;
            yield return null;
        }

        // Prit për shpërthim
        yield return new WaitForSeconds(explosionDelay);

        // Shfaq efektin e shpërthimit dhe shkatërro flluskën
        if (explosionEffect != null)
        {
            Debug.Log("Instantiating explosion effect");
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            ParticleSystem ps = explosion.GetComponent<ParticleSystem>();

            if (ps != null)
            {
                var main = ps.main;
                main.loop = false;   // Sigurohuni që të mos përsëritet

                Destroy(explosion, main.duration + main.startLifetime.constantMax);
                SoundManager.instance.PlaySFX("Explosion");

            }
            else
            {
                Debug.LogError("ParticleSystem component not found on explosionEffect");
            }
        }
        else
        {
            Debug.LogError("ExplosionEffect is not assigned in the inspector");
        }

        Destroy(gameObject);
    }
}
