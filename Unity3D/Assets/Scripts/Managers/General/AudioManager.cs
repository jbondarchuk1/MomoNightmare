using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

/// <summary>
/// Audio Manager Behaviour allows a flexible setup for playing sounds.
/// Add this on multiple game objects to emit sounds from that game object.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public List<SoundGroup> soundGroups = new List<SoundGroup>();

    void Awake()
    {
        InitializeSounds();
    }
    private void InitializeSounds()
    {
        foreach (SoundGroup sg in soundGroups)
        {
            foreach (Sound s in sg.sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.playOnAwake = s.playOnAwake;
                s.source.spatialBlend = s.spatialImpact;
            }
            if (sg.getCurrentSound().playOnAwake) sg.PlaySound();
        }
    }
    
    /// <summary>
    /// Plays currently selected sound in sound group
    /// </summary>
    public void Play(string groupName, bool restartIfPlaying = false)
    {
        SoundGroup sg = soundGroups.Find(x => x.name == groupName);
        if (!sg.getCurrentSound().source.isPlaying || restartIfPlaying)
            sg.PlaySound();
    }

    /// <summary>
    /// Plays a selected sound in the group of sounds
    /// </summary>
    public void Play(string groupName, string soundName, bool restartIfPlaying = false)
    {
        SoundGroup sg = soundGroups.Find(x => x.name == groupName);
        if (!sg.getCurrentSound().source.isPlaying || restartIfPlaying)
            sg.PlaySound(soundName);
    }

    /// <summary>
    /// stop the selected sound from a group
    /// </summary>
    public void Stop(string groupName)
    {
        SoundGroup sg = soundGroups.Find(x => x.name == groupName);
        sg.StopSound();
    }
    public void Stop(string groupName, string soundName)
    {
        SoundGroup sg = soundGroups.Find(x => x.name == groupName);
        sg.StopSound(soundName);
    }
}
