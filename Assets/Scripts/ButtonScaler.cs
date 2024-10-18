using UnityEngine;
using UnityEngine.UI;

public class ButtonScaler : MonoBehaviour
{
    public RectTransform speedButton;
    public RectTransform shieldButton;// Assign your button's RectTransform in the Inspector

    // Reference resolution size (base resolution)
    private float referenceWidth = 1080f;
    private float referenceHeight = 1920f;

    // Base width and height for the button at reference resolution
    private float referenceButtonWidth = 400f;
    private float referenceButtonHeight = 400f;

    void Start()
    {
        ScaleButton();
    }

    void ScaleButton()
    {
        // Get current screen resolution
        float currentWidth = Screen.width;
        float currentHeight = Screen.height;

        // Calculate the scale ratio based on width and height difference
        float widthRatio = currentWidth / referenceWidth;
        float heightRatio = currentHeight / referenceHeight;

        // Choose the smaller ratio to maintain consistent scaling
        float scaleFactor = Mathf.Min(widthRatio, heightRatio);

        // Set the new width and height of the button based on the scale factor
        speedButton.sizeDelta = new Vector2(
            referenceButtonWidth * scaleFactor,
            referenceButtonHeight * scaleFactor
        );

        shieldButton.sizeDelta = new Vector2(
            referenceButtonWidth * scaleFactor,
            referenceButtonHeight * scaleFactor
        );

        Debug.Log($"Button scaled to: {speedButton.sizeDelta.x}x{speedButton.sizeDelta.y}");
    }
}
