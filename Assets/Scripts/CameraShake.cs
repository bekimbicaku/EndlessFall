using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.3f;
    private Vector3 originalPosition;
    private float currentShakeDuration = 0f;

    private void Awake()
    {
        originalPosition = transform.localPosition;
    }

    public void ShakeCamera()
    {
        currentShakeDuration = shakeDuration;
    }

    private void Update()
    {
        if (currentShakeDuration > 0)
        {
            transform.localPosition = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            currentShakeDuration -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = originalPosition;
        }
    }
}
