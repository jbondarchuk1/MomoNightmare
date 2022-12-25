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
       
    [HideInInspector] public AudioSource source;

    [Range(0f,1f)] public float volume = 1f;
    [Range(1f,3f)] public float pitch = 1f;

    public bool loop = false;
    public bool playOnAwake = false;

    [Tooltip("0 is 2D, 1 is 3D")][Range(0f,1f)]public float spatialImpact = 0f;
}

/// <summary>
/// Sound groups represent interchangeable sounds in the game world
/// Sometimes we want random grunts to change each time or to play different
/// footsteps depending on what we're walking on. This lets us just choose one.
/// </summary>
[System.Serializable]
public class SoundGroup
{
    public string name = "";
    [SerializeField] private int activeIdx = 0;
    public List<Sound> sounds = new List<Sound>();
    
    public Sound getCurrentSound()
    {
        return sounds[activeIdx];
    }
    public void StopSound()
    {
        sounds[activeIdx].source.Stop();
    }
    public void StopSound(string name)
    {
        int idx = sounds.FindIndex(x => x.name == name);
        sounds[idx].source.Stop();
    }
    public void PlaySound()
    {
        sounds[activeIdx].source.Play();
    }
    public void PlaySound(int idx)
    {
        if (idx < 0 || idx >= sounds.Count)
        {
            Debug.LogError("Invalid index to play sound for Sound Group: " + name);
            return;
        }

        if (idx == activeIdx) sounds[activeIdx].source.Play();
        else
        {
            sounds[activeIdx].source.Stop();
            activeIdx = idx;
            sounds[activeIdx].source.Play();
        }
    }
    public void PlaySound(string name)
    {
        int idx = sounds.FindIndex(x => x.name == name);
        PlaySound(idx);
    }


}
    