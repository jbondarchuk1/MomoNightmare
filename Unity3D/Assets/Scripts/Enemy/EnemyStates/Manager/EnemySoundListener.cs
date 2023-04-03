using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyStateManager;

/// <summary>
/// Intermediary Class between State Manager and States that handle have methods to handle sounds they hear
/// </summary>
[System.Serializable]
public class EnemySoundListener 
{
    public enum ListenState { None, Quiet, Medium, Loud }
    public ListenState State { get; set; } = ListenState.None;

    public float quietClamp = 0f;
    public float lowClamp = 1f;
    public float mediumClamp = 2f;
    public float loudClamp = 3f;
    public Vector3? soundOrigin;

    public ListenState Listen(Vector3 soundOrigin, int intensity)
    {
        Debug.Log("Intensity: " + intensity);

        this.soundOrigin = soundOrigin;
        if (intensity > loudClamp) State = ListenState.Loud;
        else if (intensity > mediumClamp) State = ListenState.Medium;
        else if (intensity > quietClamp) State = ListenState.Quiet;
        else
        {
            this.soundOrigin = null;
            State = ListenState.None;
        }

        return State;
    }
}