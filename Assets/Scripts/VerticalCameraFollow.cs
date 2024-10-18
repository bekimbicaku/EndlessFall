using UnityEngine;

public class VerticalCameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float offsetX = 0f; // The fixed x-position for the camera
    public float smoothSpeed = 0.125f; // Smoothing speed for the camera movement

    private void LateUpdate()
    {
        // Check if the player reference is set
        if (player != null)
        {
            // Calculate the desired position
            Vector3 desiredPosition = new Vector3(offsetX, player.position.y - 8f, transform.position.z);

            // Smoothly move the camera to the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
