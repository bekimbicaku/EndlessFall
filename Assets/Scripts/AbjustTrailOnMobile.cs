using UnityEngine;

public class AdjustTrailOnMobile : MonoBehaviour
{
    private TrailRenderer trailRenderer;

    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();

        // Adjust TrailRenderer Time based on the platform
#if UNITY_ANDROID || UNITY_IOS
        // If on mobile, adjust the time dynamically
        trailRenderer.time = 0.7f; // Adjust this to get the perfect value on mobile
#else
        // In the editor, keep the time value small as you preferred
        trailRenderer.time = 1f;
#endif
    }
}
