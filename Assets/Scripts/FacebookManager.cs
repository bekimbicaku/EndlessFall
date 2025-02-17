using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;
using Facebook.Unity;
using System;


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
        FB.Init(SetInit, onHidenUnity);

        if (!FB.IsInitialized)
        {
            FB.Init(() =>
            {
                if (FB.IsInitialized)
                    FB.ActivateApp();
                else
                    print("Couldn't initialize");
            },
            isGameShown =>
            {
                if (!isGameShown)
                    Time.timeScale = 0;
                else
                    Time.timeScale = 1;
            });
        }
        else
            FB.ActivateApp();
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
        if (FB.IsLoggedIn)
        {
            Debug.Log("Facebook is Login!");
            string s = "client token" + FB.ClientToken + "User Id" + AccessToken.CurrentAccessToken.UserId + "token string" + AccessToken.CurrentAccessToken.TokenString;
            PlayerPrefs.SetInt("isLoggedIn", 1);
            PlayerPrefs.Save();

            // Shfaqim panelin kryesor ose loj�n
            ShowMainGamePanel();
        }
        else
        {
            Debug.Log("Facebook is not Logged in!");
        }
        DealWithFbMenus(FB.IsLoggedIn);
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
            FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
            FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
        }
        else
        {
            print("Not logged in");
        }
    }
    void DisplayUsername(IResult result)
    {
        if (result.Error == null)
        {
            string name = "" + result.ResultDictionary["first_name"];
            if (FB_userName != null) FB_userName.text = name;
            FB_userName.text = name;
            Debug.Log("" + name);
        }
        else
        {
            Debug.Log(result.Error);
        }
    }
    void DisplayProfilePic(IGraphResult result)
    {
        if (result.Texture != null)
        {
            Debug.Log("Profile Pic");
            rawImg.texture = result.Texture;
            if (FB_profilePic != null) FB_profilePic.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());
            /*JSONObject json = new JSONObject(result.RawResult);

            StartCoroutine(DownloadTexture(json["picture"]["data"]["url"].str, profile_texture));*/
        }
        else
        {
            Debug.Log(result.Error);
        }
    }



    #endregion


    //login
    public void Facebook_LogIn()
    {
        List<string> permissions = new List<string>();
        permissions.Add("public_profile");
        //permissions.Add("user_friends");
        FB.LogInWithReadPermissions(permissions, AuthCallBack);

    }
    void AuthCallBack(IResult result)
    {
        if (FB.IsLoggedIn)
        {
            SetInit();
            //AccessToken class will have session details
            var aToken = AccessToken.CurrentAccessToken;

            print(aToken.UserId);

            foreach (string perm in aToken.Permissions)
            {
                print(perm);
            }
        }
        else
        {
            print("Failed to log in");
        }

    }




    //logout
    public void Facebook_LogOut()
    {
        StartCoroutine(LogOut());
    }
    IEnumerator LogOut()
    {
        FB.LogOut();
        while (FB.IsLoggedIn)
        {
            print("Logging Out");
            yield return null;
        }
        print("Logout Successful");
        // if (FB_profilePic != null) FB_profilePic.sprite = null;
        if (FB_userName != null) FB_userName.text = "";
        if (rawImg != null) rawImg.texture = null;
    }


    #region other

    public void FacebookSharefeed()
    {
        string url = "https:developers.facebook.com/docs/unity/reference/current/FB.ShareLink";
        FB.ShareLink(
            new Uri(url),
            "Checkout COCO 3D channel",
            "I just watched " + "22" + " times of this channel",
            null,
            ShareCallback);

    }

    private static void ShareCallback(IShareResult result)
    {
        Debug.Log("ShareCallback");
        SpentCoins(2, "sharelink");
        if (result.Error != null)
        {
            Debug.LogError(result.Error);
            return;
        }
        Debug.Log(result.RawResult);
    }
    public static void SpentCoins(int coins, string item)
    {
        var param = new Dictionary<string, object>();
        param[AppEventParameterName.ContentID] = item;
        FB.LogAppEvent(AppEventName.SpentCredits, (float)coins, param);
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
    /*public void GetFriendsPlayingThisGame()
    {
        string query = "/me/friends";
        FB.API(query, HttpMethod.GET, result =>
        {
            Debug.Log("the raw" + result.RawResult);
            var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
            var friendsList = (List<object>)dictionary["data"];

            foreach (var dict in friendsList)
            {
                GameObject go = Instantiate(friendstxtprefab);
                go.GetComponent<Text>().text = ((Dictionary<string, object>)dict)["name"].ToString();
                go.transform.SetParent(GetFriendsPos.transform, false);
                FriendsText[1].text += ((Dictionary<string, object>)dict)["name"];
            }
        });
    }*/

    #endregion

}