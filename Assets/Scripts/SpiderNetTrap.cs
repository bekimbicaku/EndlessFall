using UnityEngine;

public class SpiderNetTrap : MonoBehaviour
{
    public Transform trapCenter; // Reference to the trap center
    public float pullForce = 5f; // Force to pull the player to the center
    public float damageInterval = 0.1f; // Time interval between damage ticks
    public int damageAmount = 5; // Damage amount per tick

    private bool playerInTrap = false; // Is the player in the trap collider
    private bool playerInCenter = false; // Is the player in the trap center collider
    private GameObject player; // Reference to the player object
    private float damageTimer = 0f; // Timer for damage intervals

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            playerInTrap = true;
            Debug.Log("Player entered the trap area");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrap = false;
            playerInCenter = false;
            player = null;
            Debug.Log("Player exited the trap area");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerInTrap)
            {
                // Pull player towards the center
                Vector2 direction = (trapCenter.position - player.transform.position).normalized;
                player.GetComponent<Rigidbody2D>().AddForce(direction * pullForce);
                Debug.Log("Pulling player towards the trap center");

                // Check if player is within center area
                if (Vector2.Distance(player.transform.position, trapCenter.position) < 0.5f) // Increased threshold
                {
                    playerInCenter = true;
                    Debug.Log("Player is within the trap center");
                }
                else
                {
                    playerInCenter = false;
                }
            }
        }
    }

    private void Update()
    {
        if (playerInCenter)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                // Apply damage to player
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount);
                    Debug.Log("Player takes damage: " + damageAmount);
                }
                else
                {
                    Debug.LogError("PlayerHealth component not found on the player!");
                }
                damageTimer = 0f;
            }
        }
    }
}
