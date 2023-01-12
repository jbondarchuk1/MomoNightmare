using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade
{
    private Sound sound;
    private float rate;
    private bool fadeIn;
    private float fadeInVol;

    public Fade(ref Sound source,float rate, float fadeInVol = -1f) :this(ref source, rate, true)
    {
        this.fadeInVol = fadeInVol;
        if (this.fadeInVol < 0f) this.fadeInVol = source.defaultVolume;
    }
    public Fade(ref Sound source, float rate, bool fadeIn)
    {
        this.sound = source;
        this.rate = rate;
        this.fadeIn = fadeIn;
    }
    
    /// <returns>boolean: the fade is complete -> T or F</returns>
    /// Use in Update
    public bool HandleFade()
    {
        float toVol = fadeIn ? fadeInVol : 0;

        sound.source.volume = Mathf.Lerp(sound.source.volume, toVol, Time.deltaTime * rate);
        if (sound.source.volume == 0) sound.source.Stop();

        return sound.source.volume == toVol;
    }
}
