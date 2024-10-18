using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCollider : MonoBehaviour
{
    private bool hasPlayedSound = false;
    private Coroutine fadeOutCoroutine;
    public string name = "Portal";
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasPlayedSound)
        {

            SoundManager.instance.PlaySFX(name);
            hasPlayedSound = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && hasPlayedSound)
        {
            fadeOutCoroutine = StartCoroutine(FadeOutSound());
            hasPlayedSound = false;

        }
    }
    void OnParticleSystemStopped()
    {
        hasPlayedSound = false; 
    }
    private IEnumerator FadeOutSound()
    {
        AudioSource sfxSource = SoundManager.instance.sfxSource;

        while (sfxSource.volume > 0.01f)
        {
            sfxSource.volume -= Time.deltaTime / 2;  // Adjust the divisor to control the speed of the fade-out
            yield return null;
        }

        sfxSource.volume = 0;
        SoundManager.instance.StopSFX();
        sfxSource.volume = 1;

    }
}
