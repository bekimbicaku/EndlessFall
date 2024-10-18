using UnityEngine;

public class TurretController : MonoBehaviour
{
    public float rotationSpeed = 45.0f;
    private float currentAngle = 0.0f;
    private bool rotatingUp = true;
    public float sideAngle;

    void Update()
    {
        if (rotatingUp)
        {
            currentAngle += rotationSpeed * Time.deltaTime;
            if (currentAngle >= 45.0f)
            {
                rotatingUp = false;
            }
        }
        else
        {
            currentAngle -= rotationSpeed * Time.deltaTime;
            if (currentAngle <= -45.0f)
            {
                rotatingUp = true;
            }
        }
        transform.localRotation = Quaternion.Euler(0, sideAngle, currentAngle);
    }
}
