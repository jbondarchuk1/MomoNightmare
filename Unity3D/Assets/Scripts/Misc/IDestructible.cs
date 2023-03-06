using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IDestructable
{
    void DestroyObj();
    void ExplodeObj(Vector3 origin, float force = 5);
}
