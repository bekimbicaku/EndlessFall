using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThronHummer : MonoBehaviour
{
    public GameObject thornPrefab;
    public Transform firePoint; // Changed from GameObject to Transform for simplicity
    private Animator animator;

    private void Start()
    {
        animator = thornPrefab.GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerTrigger"))
        {
            SpawnThorn();
        }
    }

    void SpawnThorn()
    {
        if (thornPrefab != null && firePoint != null)
        {
            // Instantiate the thorn prefab at the firePoint position
            GameObject thorn = Instantiate(thornPrefab, firePoint.position, Quaternion.identity);

            // Play the animation at the firePoint position
            PlayAnimationAt(firePoint.position, "HammerRot");
        }
    }

    public void PlayAnimationAt(Vector3 position, string animStateName)
    {
        // Move the GameObject to the specified position
        transform.position = position;

        // Play the specified animation state
        animator.Play(animStateName);
    }
}
