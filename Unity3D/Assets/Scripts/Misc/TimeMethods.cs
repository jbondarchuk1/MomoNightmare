using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeMethods
{
    /// <summary>
    /// Returns the GameTime that a wait should end
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static float GetWaitEndTime(float duration)
    {
        return Time.time + duration;
    }

    /// <summary>
    /// Given the time a wait should end, returns a bool if the wait is complete
    /// </summary>
    /// <param name="endTime"></param>
    /// <returns></returns>
    public static bool GetWaitComplete(float endTime)
    {
        if (Time.time >= endTime)
        {
            return true;
        }
        return false;
    }
}
