using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerUpManager : MonoBehaviour
{
    public Slider shieldSlider;
    public Slider magnetSlider;
    public Slider doublePointsSlider;

    // Initialize sliders and deactivate them initially
    void Start()
    {
        shieldSlider.gameObject.SetActive(false);
        magnetSlider.gameObject.SetActive(false);
        doublePointsSlider.gameObject.SetActive(false);
    }

    // Activate the shield slider and manage its value over time
    public void ActivateShieldSlider(float duration)
    {
        shieldSlider.gameObject.SetActive(true);
        StartCoroutine(UpdateSlider(shieldSlider, duration));
    }

    // Activate the magnet slider and manage its value over time
    public void ActivateMagnetSlider(float duration)
    {
        magnetSlider.gameObject.SetActive(true);
        StartCoroutine(UpdateSlider(magnetSlider, duration));
    }

    // Activate the double points slider and manage its value over time
    public void ActivateDoublePointsSlider(float duration)
    {
        doublePointsSlider.gameObject.SetActive(true);
        StartCoroutine(UpdateSlider(doublePointsSlider, duration));
    }

    // Coroutine to update the slider value over time
    private IEnumerator UpdateSlider(Slider slider, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            slider.value = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        slider.gameObject.SetActive(false); // Deactivate slider when done
    }
}
