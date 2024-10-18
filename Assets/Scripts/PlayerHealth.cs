using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 500;
    public int currentHealth;
    public Slider healthSlider;
    public Image healthFill;
    PlayerController playerController;
    public Image dangerimg;
    public float fadeDuration = 1f;  // Duration for the fade in/out effect

    private Coroutine fadeCoroutine;
    private GameManager gameManager;


    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        UpdateHealthUI();
        playerController = FindObjectOfType<PlayerController>();
        gameManager = FindObjectOfType<GameManager>();

        // Initialize danger image as transparent
        Color dangerColor = dangerimg.color;
        dangerColor.a = 0f;
        dangerimg.color = dangerColor;
    }

    private void Update()
    {
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(float damage)
    {
        int intDamage = Mathf.CeilToInt(damage); // Convert float damage to int
        currentHealth -= intDamage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health is within bounds

        healthSlider.value = currentHealth;
        UpdateHealthUI();

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeDangerImage(true));  // Fade in danger image
        StartCoroutine(FadeOut());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void RestoreHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health is within bounds

        healthSlider.value = currentHealth;
        UpdateHealthUI();

    }

    private void UpdateHealthUI()
    {
        Color healthColor;
        if (currentHealth > 400)
        {
            healthColor = Color.green;
        }
        else if (currentHealth > 200)
        {
            healthColor = new Color(1.0f, 0.64f, 0.0f); // Orange
        }
        else
        {
            healthColor = Color.red;
        }
        healthFill.color = healthColor;
    }

    private void Die()
    {
        if(PlayerPrefs.GetInt("TimeWarp", 0) > 0)
        {
            playerController.ActivateTimeWarpPanel();
        }
        else
        {
            playerController.GameOver();
        }
    }

    private IEnumerator FadeDangerImage(bool fadeIn)
    {
        float startAlpha = dangerimg.color.a;
        float endAlpha = fadeIn ? 1f : 0f;  // Fully visible when fading in, fully transparent when fading out
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            Color dangerColor = dangerimg.color;
            dangerColor.a = newAlpha;
            dangerimg.color = dangerColor;
            yield return null;
        }

        Color finalColor = dangerimg.color;
        finalColor.a = endAlpha;
        dangerimg.color = finalColor;
    }
    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1f);
        if (currentHealth > 0 && fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeDangerImage(false));  // Fade out danger image
        }
    }
}
