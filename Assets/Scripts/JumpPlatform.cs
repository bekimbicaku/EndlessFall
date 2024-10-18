using UnityEngine;
using System.Collections;

public class JumpPlatform : MonoBehaviour
{
    public float jumpForce = 10f; // Forca e k�rcimit q� do t� aplikohet
    public float gravityScaleAfterJump = -10f; // Gravity scale pas k�rcimit
    public float gravityResetDelay = 3f; // Koha p�r t� rivendosur gravity scale
    public GameObject explosionEffect;
    public Animator animator;

    // Name of the animation state to play
    public string animationStateName = "TrampolineAnim";
    void OnTriggerEnter2D(Collider2D other)
    {
        // Kontrollo n�se objekti q� ka prekur platform�n �sht� lojtari
        if (other.CompareTag("Player"))
        {
            animator.Play(animationStateName);
            // Merr komponentin Rigidbody2D t� lojtarit p�r t� aplikuar forc�n
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Debug.Log("Applying jump force.");
                // Apliko nj� forc� vertikale p�r t� ngritur lojtarin lart
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                // P�rcakto gravity scale t� lojtarit p�r t� kontrolluar r�nien
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
                //main.duration = 3f;  // Vendos koh�n e shp�rthimit
                main.loop = false;   // Sigurohuni q� t� mos p�rs�ritet

                Destroy(explosion, main.duration + main.startLifetime.constantMax);
            }

        }

        Destroy(gameObject);
    }
    private IEnumerator ResetGravityScale(Rigidbody2D rb)
    {
        // Cakto gravity scale p�r nj� periudh� t� shkurt�r kohe
        rb.gravityScale = gravityScaleAfterJump;
        yield return new WaitForSeconds(gravityResetDelay);
        // Rivendos gravity scale n� vler�n origjinale
        rb.gravityScale = 0f;
    }
}
