using UnityEngine;

public class HorizontalMover : MonoBehaviour
{
    private Rigidbody2D rb;
    public float bulletSpeed;
    private Vector2 bulletDirection;

    public GameObject explosionEffect;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = bulletDirection.normalized * bulletSpeed;
    }

    public void SetSpeedAndDirection(float speed, Vector2 direction)
    {
        bulletSpeed = speed;
        bulletDirection = direction;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(50) ;

            Explode();
            SoundManager.instance.PlaySFX("Explosion");
        }
        if (other.CompareTag("SideWall"))
        {
            Explode();
            SoundManager.instance.PlaySFX("Explosion");
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


}
