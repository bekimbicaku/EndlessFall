using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;  // UI element to display tutorial instructions
    private bool isTiltingCompleted = false;
    private bool isTappingCompleted = false;

    public Image tilt;
    public Image tap;

    public GameObject tutorial;
    void Start()
    {
        // Check if the tutorial has already been completed
        if (PlayerPrefs.GetInt("TutorialCompleted", 0) == 1)
        {
            tutorial.SetActive(false);  // Disable the tutorial if it's already completed
        }
        else
        {
            // Slow down time during the tutorial
            Invoke("StartTutorial", 3f);  // Delay tutorial by 3 seconds
        }
    }

    void StartTutorial()
    {
        Time.timeScale = 0.5f;  // 50% speed

        // Start with the first tutorial step
        tutorialText.text = "Tilt the phone to move left or right.";
        tutorial.SetActive(true);

    }

    void Update()
    {
        // Check if the player tilts the phone
        if (!isTiltingCompleted && Mathf.Abs(Input.acceleration.x) > 0.2f)
        {
            isTiltingCompleted = true;
            NextStep();
        }

        // Check if the player taps and holds the screen
        if (isTiltingCompleted && !isTappingCompleted && Input.GetMouseButton(0))
        {
            isTappingCompleted = true;
            CompleteTutorial();
        }
    }

    void NextStep()
    {
        // Show the next tutorial step
        tutorialText.text = "Tap and hold to slow down.";
        tap.gameObject.SetActive(true);
        tilt.gameObject.SetActive(false);
    }

    void CompleteTutorial()
    {
        // Tutorial completed
        tutorialText.text = "Tutorial Completed!";
        PlayerPrefs.SetInt("TutorialCompleted", 1);  // Mark tutorial as completed
        PlayerPrefs.Save();

        // Restore normal time scale after tutorial
        Time.timeScale = 1f;
        Invoke("HideTutorial", 2f);  // Hide tutorial after 2 seconds
    }

    void HideTutorial()
    {
        tutorial.SetActive(false);  // Disable the tutorial UI
    }
}
