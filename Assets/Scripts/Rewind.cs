using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rewind : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.instance.PlaySFX("ItemPickup");

            PlayerController player = other.GetComponent<PlayerController>();
            PlayerPrefs.SetInt("TimeWarp", PlayerPrefs.GetInt("TimeWarp", 0) + 1);
            CollectibleRespawn collectible = gameObject.GetComponent<CollectibleRespawn>();
            if (collectible != null)
            {
                collectible.Collect();
            }
        }
    }
}
