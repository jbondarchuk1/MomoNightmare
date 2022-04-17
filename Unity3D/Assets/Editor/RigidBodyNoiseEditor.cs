using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RigidBodyNoiseStimulus))]
public class RigidBodyNoiseEditor : Editor
{
    public void OnSceneGUI()
    {
        RigidBodyNoiseStimulus stimmy = (RigidBodyNoiseStimulus)target;
        Handles.DrawWireArc(stimmy.location.position, Vector3.up, Vector3.forward, 360, stimmy.intensity);
    }
}
