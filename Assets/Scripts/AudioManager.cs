using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public List<AudioClip> musicClips = new List<AudioClip>();
    public List<AudioClip> sfxClips = new List<AudioClip>();

    private Dictionary<string, AudioClip> musicDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sfxDict = new Dictionary<string, AudioClip>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (musicSource != null) musicSource.spatialBlend = 0f;
            if (sfxSource != null) sfxSource.spatialBlend = 0f;

            foreach (var clip in musicClips)
                if (clip != null && !musicDict.ContainsKey(clip.name))
                    musicDict.Add(clip.name, clip);

            foreach (var clip in sfxClips)
                if (clip != null && !sfxDict.ContainsKey(clip.name))
                    sfxDict.Add(clip.name, clip);
        }
        else
        {
            Destroy(gameObject);
        }

        AudioManager.Instance.PlayMusic("loop music", true);
    }


    // music track by name
    public void PlayMusic(string clipName, bool loop = true)
    {
        if (musicDict.ContainsKey(clipName))
        {
            musicSource.clip = musicDict[clipName];
            musicSource.loop = loop;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Music clip not found: " + clipName);
        }
    }

    // stop music
    public void StopMusic()
    {
        musicSource.Stop();
    }

    // sound effect by name
    public void PlaySFX(string clipName)
    {
        if (sfxDict.ContainsKey(clipName))
        {
            sfxSource.PlayOneShot(sfxDict[clipName]);
        }
        else
        {
            Debug.LogWarning("SFX clip not found: " + clipName);
        }
    }

    // volume
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
