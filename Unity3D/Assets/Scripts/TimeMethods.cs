using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeMethods
{
    public static float GetWaitTime(float waitTime)
    {
        return Time.time + waitTime;
    }

    public static bool GetWaitComplete(float endTime)
    {
        if (Time.time >= endTime)
        {
            return true;
        }
        return false;
    }
}
