using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UseShield : MonoBehaviour
{
    private PlayerController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

        if (playerController != null)
        {
            Debug.Log("PlayerController found.");
        }
        else
        {
            Debug.LogError("PlayerController not found.");
        }
    }

    public void OpenShield()
    {
        int shieldCount = PlayerPrefs.GetInt("Shields", 0);
        Debug.Log("Shield Count: " + shieldCount);

        if (shieldCount > 0)
        {
            if (playerController != null)
            {
                playerController.ActivateShield();
                Debug.Log("Shield activated.");
            }
            else
            {
                Debug.LogError("PlayerController is null when trying to activate shield.");
            }

            PlayerPrefs.SetInt("Shields", shieldCount - 1);
            Debug.Log("New Shield Count: " + PlayerPrefs.GetInt("Shields", 0));
        }
        else
        {
            Debug.Log("Not enough shields to activate.");
        }
    }
}
