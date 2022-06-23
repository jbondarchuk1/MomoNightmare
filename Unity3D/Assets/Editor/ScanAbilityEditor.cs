using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Scan))]
public class ScanAbilityEditor : Editor
{
    public void OnSceneGUI()
    {
        Scan scan = (Scan)target;
        //Gizmos.DrawRay(new Ray(scan.castOrigin.position, scan.cvc.transform.forward));
    }
}
