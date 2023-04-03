using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupManager : MonoBehaviour
{
    public bool OnAlert = false;
    public float alertDuration = 30f;
    private float endTime = 0f;
    public bool forceAlert = false;
    private void Update()
    {
        if (TimeMethods.GetWaitComplete(endTime) && OnAlert && !forceAlert)
            OnAlert = false;
    }

    public void Alert()
    {
        endTime = TimeMethods.GetWaitEndTime(alertDuration);
        OnAlert = true;
    }

}
