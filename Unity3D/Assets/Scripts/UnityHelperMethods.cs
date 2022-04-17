using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    static class UnityHelperMethods
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
}
