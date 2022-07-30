using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public SoundGroup[] soundGroups = new SoundGroup[] { };

    // Start is called before the first frame update
    void Awake()
    {
        foreach (SoundGroup soundGroup in soundGroups)
        {
            foreach(Sound sound in soundGroup.Sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
                sound.source.playOnAwake = sound.playOnStart;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (SoundGroup soundGroup in soundGroups)
        {
            string gn = soundGroup.GroupName;
            if (soundGroup.PlayingSoundIdx >= 0) // && !soundGroup.PlayingSound.source.isPlaying)
            {
                Sound playingSound = soundGroup.SelectSound(soundGroup.PlayingSoundIdx);
                if (playingSound.source.isPlaying == false)
                    Play(gn, playingSound.name);
            }
        }
    }

    public void Play(string groupName, string name)
    {
        SoundGroup sg = Array.Find(soundGroups, soundGroup => soundGroup.GroupName == groupName);
        List<Sound> sounds = sg.Sounds;
        Sound s = sounds.Find(x => x.name == name);
        
        if (s == null)
        {
            Debug.LogWarning($"Sound \"{name}\" not found");
        }
        s.source.Play();
    }

    public void Stop(string groupName, string name)
    {
        SoundGroup sg = Array.Find(soundGroups, soundGroup => soundGroup.GroupName == groupName);
        List<Sound> sounds = sg.Sounds;
        Sound s = sounds.Find(x => x.name == name);

        if (s == null)
        {
            Debug.LogWarning($"Sound \"{name}\" not found");
        }
        s.source.Stop();
    }
}
