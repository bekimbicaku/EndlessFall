using UnityEngine;

public class MagneticField : MonoBehaviour
{
    // Fuqia e forc�s magnetike
    public float magneticForce = 10f;
    // P�r t� caktuar n�se �sht� t�rheq�se (pozitive) ose shtyt�se (negative)
    public bool isAttractive = true;
    public GameObject explosionEffect;
    public GameObject magneticEffect;
    void OnTriggerStay2D(Collider2D other)
    {
        // Kontrollo n�se objekti q� ka hyr� n� trigger �sht� lojtari
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Drejtimi i forc�s magnetike (t�rheqje ose shtytje)
                Vector2 direction = (transform.position - other.transform.position).normalized;
                if (!isAttractive)
                {
                    direction = -direction;
                }

                // Apliko forc�n magnetike
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
                //main.duration = 3f;  // Vendos koh�n e shp�rthimit
                main.loop = false;   // Sigurohuni q� t� mos p�rs�ritet

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
