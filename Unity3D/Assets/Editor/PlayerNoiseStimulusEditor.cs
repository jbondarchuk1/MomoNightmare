using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerNoiseStimulus))]
public class PlayerNoiseStimulusEditor : Editor
{
    public void OnSceneGUI()
    {
        PlayerNoiseStimulus stimmy = (PlayerNoiseStimulus)target;
        Handles.DrawWireArc(stimmy.location.position, Vector3.up, Vector3.forward, 360, stimmy.intensity);
    }
}
