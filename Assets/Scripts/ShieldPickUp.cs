using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.instance.PlaySFX("ShieldPickup");

            PlayerPrefs.SetInt("Shields", PlayerPrefs.GetInt("Shields", 0) + 1);
            PlayerPrefs.Save();

            CollectibleRespawn collectible = gameObject.GetComponent<CollectibleRespawn>();
            if (collectible != null)
            {
                collectible.Collect();
            }
        }
    }
}
