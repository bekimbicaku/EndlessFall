using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            SoundManager.instance.PlaySFX("Coin");
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + 1);
            PlayerPrefs.Save();
            if (player != null)
            {
                player.Coin();
            }
            CollectibleRespawn collectible = gameObject.GetComponent<CollectibleRespawn>();
            if (collectible != null)
            {
                collectible.Collect();
            }
        }
    }
}
