using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Facebook.Unity;
using Dan.Demo;

public class GuestManager : MonoBehaviour
{
    public TextMeshProUGUI guestUserName;
    public RawImage guestProfilePic;
    public Button chooseImageButton;
    public Button confirmButton;

    public List<Texture2D> availableProfilePics; // Lista e fotove t� paracaktuara
    private int currentImageIndex = 0;           // Indeksi i fotos aktuale
    private Texture2D selectedImage;

    public GameObject mainGamePanel;
    public GameObject settingsPanel;
    public GameObject loginPanel;

    public TextMeshProUGUI warningTxt;

    public LeaderBoard leaderBoard;

    private void Start()
    {
        // Kontrollojm� n�se lojtari �sht� regjistruar m� par�
        if (PlayerPrefs.HasKey("isLoggedIn") && PlayerPrefs.GetInt("isLoggedIn") == 1)
        {
            // Lojtari �sht� regjistruar, fshihni panelin e loginit dhe shfaqni loj�n
            HideLoginPanel();
            ShowMainGamePanel();
        }
        else
        {
            // Lojtari nuk �sht� regjistruar, shfaqim panelin e loginit
            loginPanel.SetActive(true);
            mainGamePanel.SetActive(false);
        }

        // Fill the image list and make sure there are available images
        if (availableProfilePics == null || availableProfilePics.Count == 0)
        {
            Debug.LogWarning("No profile pictures are available to choose from!");
        }
        else
        {
            // Display the first image but don't set it as selected yet
            guestProfilePic.texture = availableProfilePics[currentImageIndex];
            selectedImage = null;  // Default to null until confirmed
        }

        chooseImageButton.onClick.AddListener(ChooseNextImage);
        confirmButton.onClick.AddListener(SetGuestInfo);
    }

    // Lejon lojtarin t� zgjedh� foton e radh�s nga lista
    public void ChooseNextImage()
    {
        currentImageIndex = (currentImageIndex + 1) % availableProfilePics.Count; // Ciklo n�p�r lista
        selectedImage = availableProfilePics[currentImageIndex];  // Now explicitly set the selected image
        guestProfilePic.texture = selectedImage;
    }

    // Vendos informacionin e lojtarit guest
    public void SetGuestInfo()
    {
        if (guestUserName != null && !string.IsNullOrEmpty(guestUserName.text) && selectedImage != null)
        {
            if (guestUserName.text.Length <= 10)
            {
                string guestName = guestUserName.text;
                SaveGuestInfo(guestName, selectedImage);

                loginPanel.SetActive(false);
                mainGamePanel.SetActive(true);
            }
            else
            {
                warningTxt.text = "Name must be 10 characters or fewer!";
                StartCoroutine(ClearTextAfterDelay(3f));
            }
        }
        else
        {
            // Display warnings for missing input
            if (string.IsNullOrEmpty(guestUserName.text))
            {
                warningTxt.text = "Please enter a name!";
            }
            else if (selectedImage == null)
            {
                warningTxt.text = "Please select a profile picture!";
            }
            StartCoroutine(ClearTextAfterDelay(3f));
        }
    }
    IEnumerator ClearTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        warningTxt.text = "";
    }

    // Ruaj informacionin e guest-it n� PlayerPrefs ose nj� burim tjet�r t� dh�nash
    void SaveGuestInfo(string name, Texture2D profilePic)
    {
        PlayerPrefs.SetString("GuestName", name);
        PlayerPrefs.SetInt("isLoggedIn", 1);
        PlayerPrefs.Save();

        ShowMainGamePanel();
        if (profilePic != null)
        {
            byte[] imageBytes = profilePic.EncodeToPNG();
            string imageString = System.Convert.ToBase64String(imageBytes);
            PlayerPrefs.SetString("GuestProfilePic", imageString);
        }

        Debug.Log("Guest info saved: Name - " + name);
    }

    // Merr emrin e guest-it p�r t'u p�rdorur n� leaderboard ose n� loj�
    public static string GetGuestName()
    {
        return PlayerPrefs.GetString("GuestName", "Guest");
    }

    // Merr foton e profilit t� guest-it p�r t'u p�rdorur n� leaderboard ose n� loj�
    public static Texture2D GetGuestProfilePic()
    {
        string imageString = PlayerPrefs.GetString("GuestProfilePic", "");
        if (!string.IsNullOrEmpty(imageString))
        {
            byte[] imageBytes = System.Convert.FromBase64String(imageString);
            Texture2D texture = new Texture2D(128, 128);
            texture.LoadImage(imageBytes);
            return texture;
        }

        return null;
    }

    // Funksion shembull p�r t� integruar me leaderboard-in
    public void SubmitScoreToLeaderboard(int score)
    {
        string playerName = FB.IsLoggedIn ? AccessToken.CurrentAccessToken.UserId : GetGuestName();
        Texture2D playerImage = FB.IsLoggedIn ? null : GetGuestProfilePic(); // P�r Facebook, foto do t� menaxhohet ndryshe

        // Integroni informacionin e lojtarit (emri, foto, rezultati) me leaderboard-in tuaj k�tu
        Debug.Log("Submitting score to leaderboard: Name - " + playerName + ", Score - " + score);
        // Thirrja drejt sistemit tuaj t� leaderboard-it
    }
    void HideLoginPanel()
    {
        // Kodi p�r t� fshehur panelin e loginit
        loginPanel.SetActive(false);
    }

    void ShowMainGamePanel()
    {
        // Kodi p�r t� shfaqur panelin kryesor t� loj�s
        mainGamePanel.SetActive(true);
    }

    public void DeleteAccount()
    {
        // Clear all PlayerPrefs data
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // Check if the player is logged in with Facebook
        if (FB.IsLoggedIn)
        {
            // Log out from Facebook
            FB.LogOut();
        }

        // Optionally, reset other game-related variables here if needed
        // Example:
        // ResetPlayerState();
        leaderBoard.DeleteTheEntry();
        loginPanel.SetActive(true);
        mainGamePanel.SetActive(false);
        settingsPanel.SetActive(false);
        Debug.Log("Account deleted and logged out successfully.");
    }

   
}
