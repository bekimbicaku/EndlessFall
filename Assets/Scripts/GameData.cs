using TMPro;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public TextMeshProUGUI coinTxt;
    public TextMeshProUGUI shieldTxt;

    // Static fields for coins, shields, and time warp
    private static int _coins;
    private static int _shields;
    private static int _timeWarp;

    void Awake()
    {
        // Load saved data from PlayerPrefs
        _coins = PlayerPrefs.GetInt("Coins", 0);
        _shields = PlayerPrefs.GetInt("Shields", 0);
        _timeWarp = PlayerPrefs.GetInt("TimeWarp", 0);

    }

    // Call this method to update the UI text for coins and shields
    public void Update()
    {
        coinTxt.text = PlayerPrefs.GetInt("Coins", 0).ToString();
        shieldTxt.text = PlayerPrefs.GetInt("Shields", 0).ToString();
    }

    // Properties for managing coins, shields, and time warp
    public static int Shields
    {
        get { return _shields; }
        set
        {
            _shields = value;
            PlayerPrefs.SetInt("Shields", value);
            PlayerPrefs.Save();
        }
    }

    public static int Coins
    {
        get { return _coins; }
        set
        {
            _coins = value;
            PlayerPrefs.SetInt("Coins", value);
            PlayerPrefs.Save();

        }
    }

    public static int TimeWarp
    {
        get { return _timeWarp; }
        set
        {
            _timeWarp = value;
            PlayerPrefs.SetInt("TimeWarp", value);
            PlayerPrefs.Save();
        }
    }
}
