using UnityEngine;

public class DynamicCameraAdjuster : MonoBehaviour
{
    private Camera cam;

    // Reference resolution (aspect ratio) for which you know the ideal camera size
    private float referenceWidth = 1080f;
    private float referenceHeight = 1920f;
    private float referenceCameraSize = 14f; // Ideal size for 1920x1080 resolution

    void Start()
    {
        cam = Camera.main;
        AdjustCameraSize();
    }

    void AdjustCameraSize()
    {
        // Get the current screen's aspect ratio
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float referenceAspect = referenceWidth / referenceHeight;

        // Adjust the camera size based on the aspect ratio difference
        if (screenAspect >= referenceAspect)
        {
            // Wider screens (e.g., landscape)
            cam.orthographicSize = referenceCameraSize;
        }
        else
        {
            // Taller screens (e.g., phones with taller aspect ratios like 1080x2400)
            // Maintain the same width but adjust height accordingly
            float differenceInSize = referenceAspect / screenAspect;
            cam.orthographicSize = referenceCameraSize * differenceInSize;
        }

        Debug.Log("Adjusted Camera Size: " + cam.orthographicSize);
    }
}
