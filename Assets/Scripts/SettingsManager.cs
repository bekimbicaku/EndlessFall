using UnityEngine;
using UnityEngine.UI;
using TMPro;  
using Facebook.Unity;
using System.Collections;

public class SettingsManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    [Header("Player Profile UI")]
    public TextMeshProUGUI playerNameText;        // Player name UI element
    public Image playerProfilePicture;            // Player profile picture UI element
    public TextMeshProUGUI highScoreText;         // High score UI element

    public FacebookManager facebookManager;       // Reference to FacebookManager

    private string playerUsername;
    private Sprite playerProfilePic;
    private int playerHighScore;

    public TextMeshProUGUI coinsTxt;
    public TextMeshProUGUI shieldsTxt;
    public TextMeshProUGUI timeWarpTxt;

    public TextMeshProUGUI infoText;
    public Slider tiltSensitivitySlider; // Reference to the UI Slider
    public Slider slideSensitivitySlider;

    public Toggle tiltToggle;
    public Toggle slideToggle;

    private void Start()
    {
        // Load saved volume settings or set defaults
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        float savedSensitivity = PlayerPrefs.GetFloat("TiltSensitivity", 15f);
        tiltSensitivitySlider.value = savedSensitivity;
        float savedSlideSensitivity = PlayerPrefs.GetFloat("SlideSensitivity", 10f); // Default is 2
        slideSensitivitySlider.value = savedSlideSensitivity; // Set slider value

        LoadSettings();
        tiltToggle.onValueChanged.AddListener(delegate { SetTiltMovement(); });
        slideToggle.onValueChanged.AddListener(delegate { SetSlideMovement(); });
        // Add listener to save the value when the slider changes
        tiltSensitivitySlider.onValueChanged.AddListener(OnTiltSensitivityChanged);
        slideSensitivitySlider.onValueChanged.AddListener(OnSlideSensitivityChanged);
        // Apply the saved volume levels to SoundManager
        SoundManager.instance.SetBGMVolume(bgmSlider.value);
        SoundManager.instance.SetSFXVolume(sfxSlider.value);

        // Add listeners for slider value changes
        bgmSlider.onValueChanged.AddListener(AdjustBGMVolume);
        sfxSlider.onValueChanged.AddListener(AdjustSFXVolume);
       
    }
    public void OnTiltSensitivityChanged(float value)
    {
        // Save the new tilt sensitivity value in PlayerPrefs
        PlayerPrefs.SetFloat("TiltSensitivity", value);
        PlayerPrefs.Save(); // Ensure the value is saved
    }
    public void OnSlideSensitivityChanged(float value)
    {
        // Save the new slide sensitivity value in PlayerPrefs
        PlayerPrefs.SetFloat("SlideSensitivity", value); // Save slide sensitivity
        PlayerPrefs.Save(); // Ensure the value is saved
    }
    private void Update()
    {
        coinsTxt.text = PlayerPrefs.GetInt("Coins", 0).ToString();
        shieldsTxt.text = PlayerPrefs.GetInt("Shields", 0).ToString();
        timeWarpTxt.text = PlayerPrefs.GetInt("TimeWarp", 0).ToString();
        highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();

    }
    // Adjust the background music volume
    public void AdjustBGMVolume(float volume)
    {
        SoundManager.instance.SetBGMVolume(volume);
        PlayerPrefs.SetFloat("BGMVolume", volume);  // Save BGM volume setting
    }

    // Adjust the sound effects volume
    public void AdjustSFXVolume(float volume)
    {
        SoundManager.instance.SetSFXVolume(volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);  // Save SFX volume setting
    }

    // Load player profile details (name, picture, high score)
    public void LoadPlayerProfile()
    {
        // Check if the player is logged in via Facebook
        if (facebookManager != null && FB.IsLoggedIn)
        {
            // Use Facebook username and profile picture
            playerUsername = facebookManager.FB_userName.text;
            
        }
        else
        {
            // Use guest username and a default guest profile picture
            playerUsername = PlayerPrefs.GetString("GuestName", "Guest");
            playerProfilePicture.sprite = GetGuestProfilePicture();  // Load a default guest profile picture
        }

        
        // Update the UI with player details
        playerNameText.text = playerUsername;
    }

    // Example function to load a default guest profile picture
    private Sprite GetGuestProfilePicture()
    {
        string imageString = PlayerPrefs.GetString("GuestProfilePic", "");
        if (!string.IsNullOrEmpty(imageString))
        {
            byte[] imageBytes = System.Convert.FromBase64String(imageString);
            Texture2D texture = new Texture2D(128, 128);
            texture.LoadImage(imageBytes);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        return Resources.Load<Sprite>("DefaultGuestProfilePic"); // Return a default image if none is found
    }
    public void BuyShield()
    {
        if(PlayerPrefs.GetInt("Coins", 0) < 75)
        {
            infoText.text = "Not enough coins to buy a shield!";
            StartCoroutine(ClearTextAfterDelay(2f));
        }
        else
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) - 75);
            PlayerPrefs.SetInt("Shields", PlayerPrefs.GetInt("Shields", 0) + 1);
            SoundManager.instance.PlaySFX("purchase");

        }
    }

    public void BuyTimeWarp()
    {
        if (PlayerPrefs.GetInt("Coins", 0) < 100)
        {
            infoText.text = "Not enough coins to buy a time warp!";
            StartCoroutine(ClearTextAfterDelay(2f));
        }
        else
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) - 100);
            PlayerPrefs.SetInt("TimeWarp", PlayerPrefs.GetInt("TimeWarp", 0) + 1);
            SoundManager.instance.PlaySFX("purchase");

        }
    }
    public void SetTiltMovement()
    {
        if (tiltToggle.isOn)
        {
            PlayerPrefs.SetInt("MovementType", (int)PlayerController.MovementType.Tilt);
            slideToggle.isOn = false; // Uncheck the slide toggle if tilt is selected
        }
    }

    public void SetSlideMovement()
    {
        if (slideToggle.isOn)
        {
            PlayerPrefs.SetInt("MovementType", (int)PlayerController.MovementType.Slide);
            tiltToggle.isOn = false; // Uncheck the tilt toggle if slide is selected
        }
    }

    private void LoadSettings()
    {
        // Load saved movement type from PlayerPrefs
        int movementType = PlayerPrefs.GetInt("MovementType", (int)PlayerController.MovementType.Tilt);
        if (movementType == (int)PlayerController.MovementType.Tilt)
        {
            tiltToggle.isOn = true;
            slideToggle.isOn = false;
        }
        else if (movementType == (int)PlayerController.MovementType.Slide)
        {
            slideToggle.isOn = true;
            tiltToggle.isOn = false;
        }
    }
    IEnumerator ClearTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        infoText.text = "";
    }
}
