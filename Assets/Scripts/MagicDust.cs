using UnityEngine;
using System.Collections;

public class MagicDust : MonoBehaviour
{
    public Transform[] teleportLocations; // Array of possible teleport locations
    private GameObject player;
    private Animation anim;
    private Rigidbody2D rb;
    private PlayerController playerController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Assign the player object
            player = other.gameObject;

            // Get components from the player object
            anim = player.GetComponent<Animation>();
            rb = player.GetComponent<Rigidbody2D>();
            playerController = player.GetComponent<PlayerController>();

            // Ensure none of these components are null before proceeding
            if (anim != null && rb != null && playerController != null)
            {
                StartCoroutine(PortalIn());
            }
            else
            {
                Debug.LogError("Player components missing: Animation, Rigidbody2D, or PlayerController.");
            }
        }
    }

    IEnumerator PortalIn()
    {
        rb.simulated = false;
        playerController.scoreCalculationEnabled = false; // Disable score calculation
        playerController.SaveScoreBeforeTeleport(); // Save score before teleportation

        // Play portal in animation
        anim.Play("Portal In");
        StartCoroutine(MoveInPortal());

        yield return new WaitForSeconds(0.5f);

        // Check if there are teleport locations available
        if (teleportLocations.Length > 0)
        {
            // Choose a random teleport location
            Transform chosenLocation = teleportLocations[Random.Range(0, teleportLocations.Length)];
            player.transform.position = chosenLocation.position; // Teleport the player
        }
        else
        {
            Debug.LogError("No teleport locations assigned to MagicDust.");
        }

        playerController.ScoreAfterTeleoprt(); // Update score after teleport
        anim.Play("Portal Out");

        yield return new WaitForSeconds(0.5f);
        playerController.scoreCalculationEnabled = true; // Re-enable score calculation
        rb.simulated = true;
    }

    IEnumerator MoveInPortal()
    {
        float timer = 0;
        while (timer < 0.5f)
        {
            player.transform.position = Vector2.MoveTowards(player.transform.position, transform.position, 3 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }
    }
}
