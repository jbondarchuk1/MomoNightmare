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
    private void Update()
    {
        foreach (SoundGroup sg in soundGroups)
            StartCoroutine(sg.HandleFades());
    }
    private void InitializeSounds()
    {
        foreach (SoundGroup sg in soundGroups)
            sg.GenerateSources(this.gameObject);
    }

    public void PlaySound(string groupName, string soundName = "", bool overwrite = false)
    {
        try
        {
            SoundGroup sg = soundGroups.Find(x => x.name == groupName);
            if (soundName == "") sg.PlaySound(overwrite);
            else sg.PlaySound(soundName, overwrite);
        }
        catch(Exception ex)
        {
            Debug.LogException(ex);
        }

    }
    public void FadeToSound(string groupName, string soundName = "", float fadeRate = 0f)
    {
        try
        {
            SoundGroup sg = soundGroups.Find(x => x.name == groupName);
            if (soundName == "") sg.PlaySound(fadeRate);
            else sg.PlaySound(soundName, fadeRate);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

    }
    public void StopSound(string groupName, bool stopAllSounds = false)
    {
        try 
        {
            SoundGroup sg = soundGroups.Find(x => x.name == groupName);
            sg.StopSound(stopAllSounds);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

    }
    public void SetVolume(string groupName, float vol)
    {
        SoundGroup sg = soundGroups.Find(x => x.name == groupName);
        sg.SetVolume(vol);
    }
    public void ResetVolume(string groupName)
    {
        SoundGroup sg = soundGroups.Find(x => x.name == groupName);
        sg.ResetVolume();
    }

}
