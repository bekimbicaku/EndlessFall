using UnityEngine;

public class MagneticField : MonoBehaviour
{
    // Fuqia e forcës magnetike
    public float magneticForce = 10f;
    // Për të caktuar nëse është tërheqëse (pozitive) ose shtytëse (negative)
    public bool isAttractive = true;
    public GameObject explosionEffect;
    public GameObject magneticEffect;
    void OnTriggerStay2D(Collider2D other)
    {
        // Kontrollo nëse objekti që ka hyrë në trigger është lojtari
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Drejtimi i forcës magnetike (tërheqje ose shtytje)
                Vector2 direction = (transform.position - other.transform.position).normalized;
                if (!isAttractive)
                {
                    direction = -direction;
                }

                // Apliko forcën magnetike
                rb.AddForce(direction * magneticForce);
            }
        }
        if (other.CompareTag("Shield"))
        {
            Explode();
        }
        if (other.CompareTag("PlayerTrigger"))
        {
            SpawnMagneticParticles();
        }
    }
    public void Explode()
    {
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            ParticleSystem ps = explosion.GetComponent<ParticleSystem>();

            if (ps != null)
            {
                var main = ps.main;
                //main.duration = 3f;  // Vendos kohën e shpërthimit
                main.loop = false;   // Sigurohuni që të mos përsëritet

                Destroy(explosion, main.duration + main.startLifetime.constantMax);
            }

        }

        Destroy(gameObject);
    }
    void SpawnMagneticParticles()
    {
        if (magneticEffect != null)
        {
            magneticEffect.SetActive(true);
        }
 
    }
}
