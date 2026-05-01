using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FacebookManager : MonoBehaviour
{
    public TextMeshProUGUI FB_userName;
    public Image FB_profilePic;
    public RawImage rawImg;

    #region Initialize
    public GameObject mainGamePanel;
    public GameObject loginPanel;
    private void Awake()
    {
        // Social SDK removed for release hardening; keep menu flow local-only.
    }
    void Start()
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
            loginPanel.SetActive(true);
            mainGamePanel.SetActive(false);
        }
    }
    void SetInit()
    {
        PlayerPrefs.SetInt("isLoggedIn", 1);
        PlayerPrefs.Save();
        ShowMainGamePanel();
        DealWithFbMenus(true);
    }

    void onHidenUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    void DealWithFbMenus(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            string guestName = PlayerPrefs.GetString("GuestName", "Guest");
            if (FB_userName != null)
            {
                FB_userName.text = guestName;
            }

            Texture2D savedProfile = GuestManager.GetGuestProfilePic();
            if (savedProfile != null)
            {
                if (rawImg != null) rawImg.texture = savedProfile;
                if (FB_profilePic != null)
                {
                    FB_profilePic.sprite = Sprite.Create(savedProfile, new Rect(0, 0, savedProfile.width, savedProfile.height), new Vector2(0.5f, 0.5f));
                }
            }
        }
        else
        {
            print("Not logged in");
        }
    }



    #endregion


    //login
    public void Facebook_LogIn()
    {
        SetInit();
    }




    //logout
    public void Facebook_LogOut()
    {
        StartCoroutine(LogOut());
    }
    IEnumerator LogOut()
    {
        PlayerPrefs.SetInt("isLoggedIn", 0);
        PlayerPrefs.Save();
        if (FB_userName != null) FB_userName.text = "";
        if (rawImg != null) rawImg.texture = null;
        if (loginPanel != null) loginPanel.SetActive(true);
        if (mainGamePanel != null) mainGamePanel.SetActive(false);
        yield return null;
    }


    #region other

    public void FacebookSharefeed()
    {
        Debug.Log("Share is disabled in this build.");
    }

    public static void SpentCoins(int coins, string item)
    {
        Debug.Log($"SpentCoins analytics disabled. Coins: {coins}, Item: {item}");
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

    #endregion

}