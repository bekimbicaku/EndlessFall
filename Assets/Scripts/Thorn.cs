using System.Collections;
using UnityEngine;

public class Thorn : MonoBehaviour
{
    public float jumpForce = 2f; // Force applied to the player
    public float gravityScaleAfterJump = -5f; // Gravity scale after jumping
    public float gravityResetDelay = 3f; // Time to reset gravity scale

    private CameraShake cameraShake;

    private void Start()
    {
        // Get the CameraShake component from the main camera
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(10);
            SoundManager.instance.PlaySFX("Hitted");
            cameraShake.ShakeCamera();
            Handheld.Vibrate();

            // Get the Rigidbody2D component of the player to apply force
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Debug.Log("Applying jump force.");
                // Apply vertical force to lift the player
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                // Set gravity scale for a short period
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
        }
    }

    private IEnumerator ResetGravityScale(Rigidbody2D rb)
    {
        // Set gravity scale to a new value temporarily
        rb.gravityScale = gravityScaleAfterJump;
        yield return new WaitForSeconds(gravityResetDelay);
        // Reset gravity scale to original
        rb.gravityScale = 0f;
    }
}
