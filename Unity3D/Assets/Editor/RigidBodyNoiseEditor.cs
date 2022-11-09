using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(RigidBodyNoiseStimulus))]
public class RigidBodyNoiseEditor : Editor
{
    public void OnSceneGUI()
    {
        try
        {
            if (target != null)
            {
                RigidBodyNoiseStimulus stimmy = (RigidBodyNoiseStimulus)target;
                Handles.DrawWireArc(stimmy.Location.position, Vector3.up, Vector3.forward, 360, stimmy.intensity);
            }
        }
        catch(Exception ex)
        {
            // Debug.LogException(ex);
        }

    }
}
