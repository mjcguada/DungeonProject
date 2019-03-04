using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound  {

    public string name;
    public AudioClip audio;
    public bool loop = false;
    [HideInInspector] public float volume;
    [HideInInspector] public AudioSource AudioSource_;

}
