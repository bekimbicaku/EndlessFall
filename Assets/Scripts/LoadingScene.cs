using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class LoadingScene : MonoBehaviour
{
    public GameObject LoadingScreen;         // Loading screen object
    public Slider LoadingBarSlider;          // Slider for the loading bar
    public TextMeshProUGUI LoadingPercentageText;       // Text to show the percentage

    // Call this method to load the scene
    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    // Coroutine to handle asynchronous loading
    IEnumerator LoadSceneAsync(int sceneId)
    {
        Time.timeScale = 1f;

        // Start loading the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        SoundManager.instance.PlaySFX("btnSound");

        // Activate the loading screen UI
        LoadingScreen.SetActive(true);

        // Loop until the scene is fully loaded
        while (!operation.isDone)
        {
            // Calculate the progress (0 to 1)
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            // Set the slider value (from 0 to 1)
            LoadingBarSlider.value = progressValue;

            // Convert progress to percentage and display it
            int percentage = Mathf.RoundToInt(progressValue * 100);
            LoadingPercentageText.text = percentage.ToString() + "%";

            yield return null;  // Wait for the next frame
        }
    }
}
