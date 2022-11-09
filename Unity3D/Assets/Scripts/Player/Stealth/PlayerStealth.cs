using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatRangeLevel;

[System.Serializable]
public class PlayerStealth
{
    // Fields
    public int SoundIndex { get; set; } = 0;
    #region MovementBools
    public bool Jumping { get; set; } = false;
    public bool Crouching { get; set; } = false;
    public bool Grounded { get; set; } = false;
    public bool Landing { get; set; } = false;
    public bool Sprinting { get; set; } = false;
    public float Speed { get; set; } = 0f;
    #endregion MovementBools
    public StealthZone StealthZone { get; set; } = null;
    #region ExposedInEditor
        [SerializeField] private StatRangeLevel StealthRangeLevel = new StatRangeLevel();
    #endregion ExposedInEditor

    // Constructors
    public PlayerStealth() { }

    // Methods
    public int RefreshValues()
    {
        RefreshSoundIndex();
        return SoundIndex;
    }
    public Range GetStealthRangeLevel()
    {
        int soundLevel = this.SoundIndex; // max of 4
        bool inStealthZone = this.StealthZone != null;

        float overallStealthNumber = soundLevel;
        if (soundLevel < 3 && inStealthZone)
            overallStealthNumber /=  StealthZone.StealthLevel; // divide by how good the stealth zone is, default of 1

        float percentOfMaxSound = 100f - (overallStealthNumber / 4f) * 100f;

        return StealthRangeLevel.GetRangeLevel(percentOfMaxSound);
    }
    private void RefreshSoundIndex()
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
