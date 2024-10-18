using Dan.Main;
using Dan.Models;
using Facebook.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dan.Demo
{
    public class LeaderBoard: MonoBehaviour
    {
        [SerializeField] private string _leaderboardPublicKey;

        [SerializeField] private TextMeshProUGUI _playerScoreText;
        [SerializeField] private TextMeshProUGUI[] _entryNamesFields;
        [SerializeField] private TextMeshProUGUI[] _entryRanksFields;
        [SerializeField] private TextMeshProUGUI[] _entryScoreFields;

        public GameObject LeaderboardPanel;
        public FacebookManager facebookManager; // Reference to FacebookManager

        [SerializeField] private TMP_InputField _playerUsernameInput;

        private int _playerScore;
        private string playerUsername;
        [SerializeField] private Image[] _entryBackgrounds;  // References to the parent background images

        private void Start()
        {
            Load();
            Submit();
        }
        public void LeadreboardLoad()
        {

            // Check if the player is logged in via Facebook
            if (facebookManager != null && FB.IsLoggedIn)
            {
                // Use Facebook username
                playerUsername = facebookManager.FB_userName.text;
            }
            else
            {
                // Use guest username
                playerUsername = PlayerPrefs.GetString("GuestName", "Guest");
            }
        }

        public void Load() => LeaderboardCreator.GetLeaderboard(_leaderboardPublicKey, OnLeaderboardLoaded);

        private void OnLeaderboardLoaded(Entry[] entries)
        {
            // Reset fields and background colors
            for (int i = 0; i < _entryNamesFields.Length; i++)
            {
                _entryNamesFields[i].text = "-";
                _entryRanksFields[i].text = "-";
                _entryScoreFields[i].text = "-";

                // Reset the background color to default (white or any other color)
                if (_entryBackgrounds.Length > i)
                    _entryBackgrounds[i].color = Color.white;  // Reset to default background color
            }

            // Get the current player's username (Facebook or Guest)
            string currentPlayerUsername = FB.IsLoggedIn ? facebookManager.FB_userName.text : PlayerPrefs.GetString("GuestName", "Guest");

            // Populate leaderboard entries
            for (int i = 0; i < entries.Length; i++)
            {
                _entryNamesFields[i].text = entries[i].Username;
                _entryRanksFields[i].text = entries[i].RankSuffix();
                _entryScoreFields[i].text = entries[i].Score.ToString();

                // Check if the entry belongs to the current player and change the background color
                if (entries[i].Username == currentPlayerUsername && _entryBackgrounds.Length > i)
                {
                    _entryBackgrounds[i].color = Color.green;  // Highlight player's entry background in green
                }
            }
        }



        public void Submit()
        {
            if (string.IsNullOrEmpty(playerUsername))
            {
                Debug.LogWarning("Username is not set!");
                return;
            }

            LeaderboardCreator.UploadNewEntry(_leaderboardPublicKey, playerUsername, _playerScore, Callback, ErrorCallback);
        }

        void Update()
        {
            _playerScoreText.text = "Your score: " + PlayerPrefs.GetInt("HighScore", 0);
            _playerScore = PlayerPrefs.GetInt("HighScore", 0);
        }

        public void DeleteEntry()
        {
            LeaderboardCreator.DeleteEntry(_leaderboardPublicKey, Callback, ErrorCallback);
        }

        public void ResetPlayer()
        {
            LeaderboardCreator.ResetPlayer();
        }

        private void Callback(bool success)
        {
            if (success)
                Load();
        }

        private void ErrorCallback(string error)
        {
            Debug.LogError(error);
        }

        public void OnOpenButtonClick()
        {
            LeaderboardPanel.SetActive(true);
            SoundManager.instance.PlaySFX("btnSound");

        }

        public void OnCloseButtonClick()
        {
            LeaderboardPanel.SetActive(false);
            SoundManager.instance.PlaySFX("btnSound");

        }

        public void DeleteTheEntry()
        {
            LeaderboardCreator.DeleteEntry(_leaderboardPublicKey, OnDeleteEntrySuccess, ErrorCallback);

        }
        private void OnDeleteEntrySuccess(bool success)
        {
            if (success)
            {
                Debug.Log("Leaderboard entry deleted successfully.");
            }
            else
            {
                Debug.LogError("Failed to delete leaderboard entry.");
            }
        }

    }
}
