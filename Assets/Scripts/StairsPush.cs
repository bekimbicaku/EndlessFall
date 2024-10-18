using UnityEngine;

public class StairsPush : MonoBehaviour
{
    public Transform bottomOfStairs; // Reference to the bottom of the stairs
    public float pushSpeed = 5f; // Speed at which the player is pushed down

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Start coroutine to move player to the bottom of the stairs
            StartCoroutine(PushPlayerToBottom(other.transform));
        }
    }

    private System.Collections.IEnumerator PushPlayerToBottom(Transform player)
    {
        while (Vector2.Distance(player.position, bottomOfStairs.position) > 0.1f)
        {
            // Move the player towards the bottom of the stairs
            player.position = Vector2.MoveTowards(player.position, bottomOfStairs.position, pushSpeed * Time.deltaTime);
            yield return null;
        }

        // Ensure the player reaches exactly the bottom position
        player.position = bottomOfStairs.position;
    }
}
