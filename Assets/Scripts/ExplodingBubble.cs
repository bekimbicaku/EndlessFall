using UnityEngine;
using System.Collections;

public class ExplodingBubble : MonoBehaviour
{
    public float growthRate = 1f; // Shpejt�sia e rritjes
    public float maxSize = 5f; // Madh�sia maksimale e fllusk�s
    public float explosionDelay = 1f; // Koha p�r shp�rthim
    public GameObject explosionEffect; // Efekti i shp�rthimit

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
        // Rrit fllusk�n deri n� madh�sin� maksimale
        while (transform.localScale.x < maxSize)
        {
            transform.localScale += Vector3.one * growthRate * Time.deltaTime;
            yield return null;
        }

        // Prit p�r shp�rthim
        yield return new WaitForSeconds(explosionDelay);

        // Shfaq efektin e shp�rthimit dhe shkat�rro fllusk�n
        if (explosionEffect != null)
        {
            Debug.Log("Instantiating explosion effect");
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            ParticleSystem ps = explosion.GetComponent<ParticleSystem>();

            if (ps != null)
            {
                var main = ps.main;
                main.loop = false;   // Sigurohuni q� t� mos p�rs�ritet

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
