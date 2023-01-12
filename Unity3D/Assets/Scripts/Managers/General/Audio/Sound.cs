using System;
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
    [HideInInspector] public AudioSource source;

    public string name;
    [Range(0f, 1f)] public float defaultVolume = 1f;
    
    [SerializeField][Range(1f, 3f)] private float defaultPitch = 1f;
    [SerializeField] private AudioClip clip;
    [SerializeField] private bool loop = false;
    [SerializeField][Tooltip("0 is 2D, 1 is 3D")][Range(0f, 1f)] private float defaultSpatialImpact = 0f;
    [SerializeField] private bool playOnAwake = false;
    [SerializeField] private float maxDistance3D = 10f;
    [SerializeField] private AudioRolloffMode rollOff = AudioRolloffMode.Linear;

    /// <summary>
    /// use in a start method
    /// </summary>
    /// <param name="gameObject">adds functionality without clutter, attach to the gameObject of the audioManager</param>
    public void GenerateSource(GameObject gameObject)
    {
        AudioSource src = gameObject.AddComponent<AudioSource>();
        src.maxDistance = maxDistance3D;
        src.rolloffMode = rollOff;
        src.volume = defaultVolume;
        src.spatialBlend = defaultSpatialImpact;
        src.pitch = defaultPitch;
        src.clip = clip;
        src.playOnAwake = playOnAwake;
        src.loop = loop;
        if (src.playOnAwake) src.Play();
        source = src;
    }
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
    public List<Sound> sounds = new List<Sound>();

    [SerializeField] private int activeIdx = 0;
    [Range(0, 1)] public float volume = 1f;
    private List<Fade> fades = new List<Fade>();

    private void AddFade(int soundFromIdx, float rate, int soundToIdx)
    {
        AddFadeOut(soundFromIdx, rate);
        AddFadeIn(soundToIdx, rate);
    }
    private void AddFadeOut(int soundIdx, float rate)
    {
        Sound from = sounds[soundIdx];
        fades.Add(new Fade(ref from, rate, false));
    }
    private void AddFadeIn(int soundIdx, float rate, float toVolumeOverride = -1f)
    {
        Sound to = sounds[soundIdx];
        to.source.volume = 0;
        if (!to.source.isPlaying)
            to.source.Play();
        if (toVolumeOverride > 0f) 
            fades.Add(new Fade(ref to, rate, toVolumeOverride));
        else fades.Add(new Fade(ref to, rate, to.defaultVolume));
    }
    public void PlaySound(bool overwrite = false)
    {
        if (sounds[activeIdx].source.isPlaying && !overwrite) return;
        sounds[activeIdx].source.Play();
    }
    public void PlaySound(float fadeRate)
    {
        string name = sounds[activeIdx].name;
        PlaySound(name, fadeRate);
    }
    public void PlaySound(string soundName, bool overwrite = false)
    {
        int soundIdx = sounds.FindIndex(x => x.name == soundName);

        if (!sounds[soundIdx].source.isPlaying || overwrite)
        {
            sounds[activeIdx].source.Stop();
            activeIdx = soundIdx;
            sounds[activeIdx].source.Play();
        }
    }
    public void PlaySound(string soundName, float fadeRate)
    {
        int soundIdx = sounds.FindIndex(x => x.name == soundName);

        if (soundIdx == activeIdx && sounds[soundIdx].source.isPlaying) return;
        else if (sounds[activeIdx].source.isPlaying) AddFade(activeIdx, fadeRate, soundIdx);
        else if (fadeRate > 0) AddFadeIn(soundIdx, fadeRate);
        else PlaySound(soundName);

        activeIdx = soundIdx;
    }
    public void StopSound(bool stopAll = false)
    {
        ResetVolume();
        if (stopAll)
            foreach (Sound s in sounds)
                s.source.Stop();

        else sounds[activeIdx].source.Stop();
    }
    public void GenerateSources(GameObject gameObject)
    {
        foreach (Sound s in sounds)
            s.GenerateSource(gameObject);
    }
    public IEnumerator HandleFades()
    {
        List<int> delete = new List<int>();
        for (int i = 0; i < fades.Count; i++)
        {
            if (fades[i].HandleFade())
                delete.Add(i);
        }
        for (int i = 0; i < delete.Count; i++)
            fades.RemoveAt(i);
        yield return new WaitForSeconds(0.2f);
    }

    public void SetVolume(float volume)
    {
        sounds[activeIdx].source.volume = sounds[activeIdx].defaultVolume * volume;
    }
    public void ResetVolume()
    {
        sounds[activeIdx].source.volume = sounds[activeIdx].defaultVolume;
    }
}
    