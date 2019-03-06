using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    
    public Sound[] sounds;

    private void Awake()
    {
        foreach (Sound s in sounds) {
            s.AudioSource_ = gameObject.AddComponent<AudioSource>();
            s.AudioSource_.clip = s.audio;
            s.AudioSource_.loop = s.loop;
        }
    }

    public void PlaySound(string name)
    {
        if (GameManager.instance.sonidoActivo)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.AudioSource_.Play();
        }        
    }

    public void setVolume(string name, float volume)
    {
        if (volume <= 1 && volume >= 0)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.volume = volume;
            s.AudioSource_.volume = volume;
        }
    }

    public float getVolume(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s.volume;
    }


}
