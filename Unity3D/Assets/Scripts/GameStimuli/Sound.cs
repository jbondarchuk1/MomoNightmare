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
    public float pitch = 1f;

    public bool loop;

    public bool playOnStart = false;

}

[System.Serializable]
public class SoundGroup
{
    
    public int PlayingSoundIdx = -1;
    public string GroupName = "";
    public List<Sound> Sounds = new List<Sound>();

    

    public Sound SelectSound(string name)
    {
        return Sounds.Find(s => s.name == name);
    }
    public Sound SelectSound(int index)
    {
        return Sounds[index];
    }

}
