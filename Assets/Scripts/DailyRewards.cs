using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using TMPro;

namespace DailyRewardSystem
{
    public enum RewardType
    {
        Shields,
        Coins,
        TimeWarp
    }

    [Serializable]
    public struct Reward
    {
        public RewardType Type;
        public int Amount;
    }

    public class DailyRewards : MonoBehaviour
    {

        [Header("Main Menu UI")]
        [SerializeField] TextMeshProUGUI shieldsText;
        [SerializeField] TextMeshProUGUI coinsText;
        [SerializeField] TextMeshProUGUI timeWarpText;

        [Space]
        [Header("Reward UI")]
        [SerializeField] GameObject rewardsCanvas;
        [SerializeField] Button openButton;
        [SerializeField] Button closeButton;
        [SerializeField] Image rewardImage;
        [SerializeField] TextMeshProUGUI rewardAmountText;
        [SerializeField] Button claimButton;
        [SerializeField] GameObject noMoreRewardsPanel;


        [Space]
        [Header("Rewards Images")]
        [SerializeField] Sprite iconShieldsSprite;
        [SerializeField] Sprite iconCoinsSprite;
        [SerializeField] Sprite iconTimeWarpSprite;

        [Space]
        [Header("Rewards Database")]
        [SerializeField] RewardsDatabase rewardsDB;

        [Space]
        [Header("Timing")]
        //wait 23 Hours to activate the next reward (it's better to use 23h instead of 24h)
        [SerializeField] double nextRewardDelay = 23f;
        //check if reward is available every 5 seconds
        [SerializeField] float checkForRewardDelay = 5f;
        [SerializeField] Animator rewardReminderAnimator; // Reference to the Animator

        private int nextRewardIndex;
        private bool isRewardReady = false;

        void Start()
        {
            Initialize();

            StopAllCoroutines();
            StartCoroutine(CheckForRewards());
        }

        void Initialize()
        {
            nextRewardIndex = PlayerPrefs.GetInt("Next_Reward_Index", 0);

            //Update Mainmenu UI (metals,coins,gems)
            UpdateMetalsTextUI();
            UpdateCoinsTextUI();
            UpdateGemsTextUI();

            //Add Click Events
            openButton.onClick.RemoveAllListeners();
            openButton.onClick.AddListener(OnOpenButtonClick);

            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(OnCloseButtonClick);

            claimButton.onClick.RemoveAllListeners();
            claimButton.onClick.AddListener(OnClaimButtonClick);

            //Check if the game is opened for the first time then set Reward_Claim_Datetime to the current datetime
            if (string.IsNullOrEmpty(PlayerPrefs.GetString("Reward_Claim_Datetime")))
                PlayerPrefs.SetString("Reward_Claim_Datetime", DateTime.Now.ToString());
        }

        IEnumerator CheckForRewards()
        {
            while (true)
            {
                if (!isRewardReady)
                {
                    DateTime currentDatetime = DateTime.Now;
                    DateTime rewardClaimDatetime = DateTime.Parse(PlayerPrefs.GetString("Reward_Claim_Datetime", currentDatetime.ToString()));

                    //get total Hours between this 2 dates
                    double elapsedHours = (currentDatetime - rewardClaimDatetime).TotalHours;

                    if (elapsedHours >= nextRewardDelay)
                        ActivateReward();
                    else
                        DesactivateReward();
                }

                yield return new WaitForSeconds(checkForRewardDelay);
            }
        }

        void ActivateReward()
        {
            isRewardReady = true;

            noMoreRewardsPanel.SetActive(false);
            rewardReminderAnimator.SetTrigger("ShowReminder"); // Trigger the reminder animation

            //Update Reward UI
            Reward reward = rewardsDB.GetReward(nextRewardIndex);

            //Icon
            if (reward.Type == RewardType.Shields)
                rewardImage.sprite = iconShieldsSprite;
            else if (reward.Type == RewardType.Coins)
                rewardImage.sprite = iconCoinsSprite;
            else
                rewardImage.sprite = iconTimeWarpSprite;

            //Amount
            rewardAmountText.text = string.Format("+{0}", reward.Amount);

        }

        void DesactivateReward()
        {
            isRewardReady = false;

            noMoreRewardsPanel.SetActive(true);
            rewardReminderAnimator.SetTrigger("HideReminder"); // Trigger to hide the animation
        }

        void OnClaimButtonClick()
        {
            Reward reward = rewardsDB.GetReward(nextRewardIndex);
            SoundManager.instance.PlaySFX("btnSound");

            //check reward type
            if (reward.Type == RewardType.Shields)
            {
                Debug.Log("<color=white>" + reward.Type.ToString() + " Claimed : </color>+" + reward.Amount);
                GameData.Shields += reward.Amount;
                UpdateMetalsTextUI();

            }
            else if (reward.Type == RewardType.Coins)
            {
                Debug.Log("<color=yellow>" + reward.Type.ToString() + " Claimed : </color>+" + reward.Amount);
                GameData.Coins += reward.Amount;
                UpdateCoinsTextUI();

            }
            else
            {//reward.Type == RewardType.Gems
                Debug.Log("<color=green>" + reward.Type.ToString() + " Claimed : </color>+" + reward.Amount);
                GameData.TimeWarp += reward.Amount;
                UpdateGemsTextUI();

                isRewardReady = false;
            }

            //Save next reward index
            nextRewardIndex++;
            if (nextRewardIndex >= rewardsDB.rewardsCount)
                nextRewardIndex = 0;

            PlayerPrefs.SetInt("Next_Reward_Index", nextRewardIndex);

            //Save DateTime of the last Claim Click
            PlayerPrefs.SetString("Reward_Claim_Datetime", DateTime.Now.ToString());
            SoundManager.instance.PlaySFX("purchase");

            DesactivateReward();
        }

        //Update Mainmenu UI (metals,coins,gems)--------------------------------
        void UpdateMetalsTextUI()
        {
            shieldsText.text = GameData.Shields.ToString();
        }

        void UpdateCoinsTextUI()
        {
            coinsText.text = GameData.Coins.ToString();
        }

        void UpdateGemsTextUI()
        {
            timeWarpText.text = GameData.TimeWarp.ToString();
        }

        //Open | Close UI -------------------------------------------------------
        void OnOpenButtonClick()
        {
            rewardsCanvas.SetActive(true);
            SoundManager.instance.PlaySFX("btnSound");

        }

        void OnCloseButtonClick()
        {
            rewardsCanvas.SetActive(false);
            SoundManager.instance.PlaySFX("btnSound");

        }
    }

}
