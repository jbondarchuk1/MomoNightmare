using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO FACTOR IN LANDING
public class PlayerStealth
{
    // Exposed GET
    public int SoundIndex { get; set; } = 0;


    public bool Jumping { get; set; } = false;
    public bool Crouching { get; set; } = false;
    public bool Grounded { get; set; } = false;
    public bool Landing { get; set; } = false;
    public bool Sprinting { get; set; } = false;
    public float Speed { get; set; } = 0f;
    public StealthZone StealthZone { get; set; } = null;
    // public GroundType groundType { get; set; } = null;

    public PlayerStealth() { }

    public int UpdateValues()
    {
        UpdateSoundIndex();
        return SoundIndex;
    }

    private void UpdateSoundIndex()
    {
        if (Grounded)
        {
            if (Sprinting)
                SoundIndex = 3;
            else if (Crouching)
                SoundIndex = 1;
            else if (Speed > 0f)
                SoundIndex = 2;

            else SoundIndex = 0;
        }
        else if (Jumping)
        {
            SoundIndex = 4;
        }

    }
}
