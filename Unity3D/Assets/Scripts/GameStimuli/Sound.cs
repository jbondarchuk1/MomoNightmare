using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Sound Class represents an in game sound played through Unity's audio manager
/// For game elements concerning the in game noise system, please visit scripts with the term "noise"
/// </summary>
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
       
    [HideInInspector]
    public AudioSource source;

    [Range(0f,1f)]
    public float volume;
    [Range(1f,3f)]
    public float pitch;
    public bool loop;

}
