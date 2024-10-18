using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public int damage = 50;
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
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            health.TakeDamage(damage);
            SoundManager.instance.PlaySFX("Hitted");
            cameraShake.ShakeCamera();
            Handheld.Vibrate();

        }
    }
}
