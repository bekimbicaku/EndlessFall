using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class SpeedButtonControlle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button speedButton;
    public Slider speedIndicatorSlider;  // Change Image to Slider
    public float depletionRate = 1f;
    public float refillRate = 0.5f;

    private PlayerController playerController;
    public bool isButtonHeld = false;
    public float indicatorValue = 100f;

    private Coroutine fadeOutCoroutine;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

        if (playerController != null)
        {
            Debug.Log("PlayerController found.");
        }
        else
        {
            Debug.LogError("PlayerController not found.");
        }

        UpdateSpeedIndicator();
        speedIndicatorSlider.value = indicatorValue;
    }

    void Update()
    {
        if (isButtonHeld)
        {
            indicatorValue -= depletionRate * Time.deltaTime;

            if (indicatorValue <= 0.5)
            {
                indicatorValue = 0;
                speedButton.interactable = false;
                playerController.ResetFallSpeed();
            }
            else
            {
                speedButton.interactable = true;
                playerController.IncreaseFallSpeed();
            }
        }
        else
        {
            indicatorValue += refillRate * Time.deltaTime;
            indicatorValue = Mathf.Clamp(indicatorValue, 0, 10);
        }

        UpdateSpeedIndicator();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (indicatorValue >= 0.5)
        {
            isButtonHeld = true;
            playerController.isHelding = isButtonHeld;
            Debug.Log("Speed button held. Indicator Value: " + indicatorValue);
            SoundManager.instance.PlaySFX("Speed");
        }
        else
        {
            isButtonHeld = false;
            Debug.Log("Speed button pressed but not enough indicator value.");
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isButtonHeld = false;
        playerController.isHelding = isButtonHeld;
        playerController.ResetFallSpeed();
        Debug.Log("Speed button released. Resetting fall speed.");

        fadeOutCoroutine = StartCoroutine(FadeOutSound());
    }

    private void UpdateSpeedIndicator()
    {
        speedIndicatorSlider.value = indicatorValue;
    }

    private IEnumerator FadeOutSound()
    {
        AudioSource sfxSource = SoundManager.instance.sfxSource;
        Debug.Log("Fading out sound...");

        while (sfxSource.volume > 0.01f)
        {
            sfxSource.volume -= Time.deltaTime / 2;  // Adjust the divisor to control the speed of the fade-out
            yield return null;
        }

        sfxSource.volume = 0;
        SoundManager.instance.StopSFX();
        sfxSource.volume = 1;
        Debug.Log("Sound faded out.");
    }
}
