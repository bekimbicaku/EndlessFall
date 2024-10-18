using UnityEngine;
using System.Collections;

public class JumpPlatform : MonoBehaviour
{
    public float jumpForce = 10f; // Forca e kërcimit që do të aplikohet
    public float gravityScaleAfterJump = -10f; // Gravity scale pas kërcimit
    public float gravityResetDelay = 3f; // Koha për të rivendosur gravity scale
    public GameObject explosionEffect;
    public Animator animator;

    // Name of the animation state to play
    public string animationStateName = "TrampolineAnim";
    void OnTriggerEnter2D(Collider2D other)
    {
        // Kontrollo nëse objekti që ka prekur platformën është lojtari
        if (other.CompareTag("Player"))
        {
            animator.Play(animationStateName);
            // Merr komponentin Rigidbody2D të lojtarit për të aplikuar forcën
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Debug.Log("Applying jump force.");
                // Apliko një forcë vertikale për të ngritur lojtarin lart
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                // Përcakto gravity scale të lojtarit për të kontrolluar rënien
                StartCoroutine(ResetGravityScale(rb));

                // Trigger the jump state in the player controller
                PlayerController playerController = other.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.TriggerJump(3f); // 3 seconds jump duration
                }
            }
            else
            {
                Debug.Log("Rigidbody2D not found on player.");
            }
        }else if(other.CompareTag("Shield") || other.CompareTag("SpeedUp"))
        {
            Explode();
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
    private IEnumerator ResetGravityScale(Rigidbody2D rb)
    {
        // Cakto gravity scale për një periudhë të shkurtër kohe
        rb.gravityScale = gravityScaleAfterJump;
        yield return new WaitForSeconds(gravityResetDelay);
        // Rivendos gravity scale në vlerën origjinale
        rb.gravityScale = 0f;
    }
}
