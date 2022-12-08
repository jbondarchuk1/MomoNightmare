using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public static class UnityHelperMethods
{
    /// <summary>
    /// Returns float array of size 3 for x,y,z
    /// </summary>
    /// <param name="v3"></param>
    /// <returns></returns>
    public static float[] ToFloatArray(this Vector3 v3)
    {
        float[] result = new float[3];
        result[0] = v3.x;
        result[1] = v3.y;
        result[2] = v3.z;
        return result;
    }
    public static Vector3 ToVector3(this float[] arr)
    {
        try
        {
            return new Vector3(arr[0], arr[1], arr[2]);
        }
        catch(Exception ex)
        {
            Debug.LogError(ex.Message);
            return Vector3.zero;
        }
    }
}
