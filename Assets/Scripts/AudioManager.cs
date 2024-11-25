using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Range(0, 1f)]
    public float masterLevel = 1;
    [Range(0, 1f)]
    public float musicLevel = 1;
    [Range(0, 1f)]
    public float sfxLevel = 1;
    [Range(0, 1f)]
    public float menuLevel = 1;

    public Sound[] sounds;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in sounds)
        {

            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            if (s.soundType == Sound.SoundType.Music)
            {
                s.source.volume = s.volume * musicLevel * masterLevel;
            }
            else if (s.soundType == Sound.SoundType.Menu)
            {
                s.source.volume = s.volume * menuLevel * masterLevel;
            }
            else if (s.soundType == Sound.SoundType.SFX)
            {
                s.source.volume = s.volume * sfxLevel * masterLevel;
            }
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Play("Theme");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
}