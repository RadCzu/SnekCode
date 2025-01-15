using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [System.Serializable]
    public class Sound
    {
        public string name;          
        public AudioClip clip;       
        public float volume = 1.0f;  
        public float pitch = 1.0f;
    }

    public List<Sound> sounds = new List<Sound>();

    private Dictionary<string, Sound> soundDictionary = new Dictionary<string, Sound>();
    private AudioSource audioSource;
    private AudioSource musicSource; 

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();

        foreach (var sound in sounds)
        {
            soundDictionary[sound.name] = sound;
        }
    }

    public void Play(string soundName)
    {
        if (soundDictionary.TryGetValue(soundName, out var sound))
        {
            audioSource.pitch = sound.pitch;
            audioSource.PlayOneShot(sound.clip, sound.volume);
            audioSource.pitch = 1.0f;
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found!");
        }
    }

    public void PlayMusic(string soundName)
    {
        if (soundDictionary.TryGetValue(soundName, out var sound))
        {
            musicSource.clip = sound.clip;
            musicSource.volume = sound.volume;
            musicSource.pitch = sound.pitch;
            musicSource.loop = true;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"Music '{soundName}' not found!");
        }
    }

    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
}
