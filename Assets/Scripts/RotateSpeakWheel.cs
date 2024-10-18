using UnityEngine;

public class RotateSpikedWheel : MonoBehaviour
{
    // Rotation speed
    public float rotationSpeed = 100f;
    public GameObject explosionEffect;
    // Reference to CameraShake component
    private CameraShake cameraShake;

    private void Start()
    {
        // Get the CameraShake component from the main camera
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    void Update()
    {
        // Rotate the object around the Z-axis
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Deal damage to the player
            other.GetComponent<PlayerHealth>().TakeDamage(10);

            // Play the hit sound effect
            SoundManager.instance.PlaySFX("Hitted");

            // Trigger camera shake
            cameraShake.ShakeCamera();

            // Vibrate the device
            Handheld.Vibrate();
        }
    }
}
