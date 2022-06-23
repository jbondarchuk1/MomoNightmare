using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class UnityHelperMethods
    {
        public static bool ClampVector3(Vector3 vector, float xMin, float xMax, float yMin, float yMax, float zMin, float zMax)
        {
            if ((vector.x > xMin && vector.x < xMax)
                && (vector.y > yMin && vector.y < yMax)
                && (vector.z > zMin && vector.z < zMax))
                {
                    return true;
                }
            else return false;
        }
        public static bool ClampVector3(Vector3 vector, float minMagnitude, float maxMagnitude)
        {
            return vector.magnitude < maxMagnitude && vector.magnitude > minMagnitude;
        }
    }

    /// <summary>
    /// Clamp values to different ranges.
    /// </summary>
    public class Clamp
    {
        public float[] LevelVals { get; set; } = new float[] { 0f, 0f };
        public float[][] ClampVals { get; set; } = new float[][] { new float[] { 0f, 1f }, new float[] { 0f, 1f } };
        public Clamp(float min1, float max1, float min2, float max2)
        {
            this.ClampVals = new float[][] { new float[] { min1, max1 }, new float[] { min2, max2 } };
        }
        public int GetLevel(float value)
        {
            for (int i = 0; i < this.ClampVals.Length; i++)
            {
                if (value > this.ClampVals[i][0] && value < this.ClampVals[i][1])
                    return i;
            }
            return -1;
        }
    } // end Clamp
}
