using UnityEngine;

public class RotateZAxis : MonoBehaviour
{
    public float rotationSpeed = 100f; // Rotation speed in degrees per second

    void Update()
    {
        // Rotate the object around its Z axis at the given speed
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
