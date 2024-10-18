using UnityEngine;
using System.Collections.Generic;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Sound Settings")]
    public AudioSource bgmSource;              // Background music source
    public AudioSource sfxSource;              // Sound effects source
    public Sound[] bgmClips;                   // Array of background music clips
    public Sound[] sfxClips;                   // Array of sound effects clips

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Attach the AudioSource to each Sound
            foreach (Sound s in bgmClips)
            {
                s.source = bgmSource;
            }

            foreach (Sound s in sfxClips)
            {
                s.source = sfxSource;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayBGM(string name)
    {
        Sound s = Array.Find(bgmClips, x => x.name == name);

        if (s == null)
        {
            Debug.LogWarning("BGM: " + name + " not found!");
            return;
        }

        s.source.clip = s.clip;
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
        s.source.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxClips, x => x.name == name);

        if (s == null)
        {
            Debug.LogWarning("SFX: " + name + " not found!");
            return;
        }

        s.source.PlayOneShot(s.clip);
    }

    public void StopSFX()
    {
        sfxSource.Stop();
    }

    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
