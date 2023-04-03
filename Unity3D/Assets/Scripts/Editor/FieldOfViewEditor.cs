using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FOV))]
public class FOVEditor : Editor
{

    public void OnSceneGUI()
    {
        FOV fovParent = (FOV)target;
        FOV.FOVValues fov = fovParent.fovValues[fovParent.currFOVIdx];
        Handles.color = Color.white;

        if (fov != null)
        {
            // draw large arc
            Handles.DrawWireArc(fovParent.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);

            Handles.color = new Color(1f, 1f, 1f, 0.6f);
            // draw inner arc
            Handles.DrawWireArc(fovParent.transform.position, Vector3.up, Vector3.forward, 360, fov.innerRadius);


            Color newColor = Color.red;
            newColor.a = 0.4f;
            Handles.color = newColor;
            // draw visit arc
            Handles.DrawWireArc(fovParent.transform.position, Vector3.up, Vector3.forward, 360, fov.visitRadius);

            Vector3 viewAngle1 = DirectionfromAngle(fovParent.transform.eulerAngles.y, -fov.angle / 2);
            Vector3 viewAngle2 = DirectionfromAngle(fovParent.transform.eulerAngles.y, fov.angle / 2);

            Vector3 outAngle1 = DirectionfromAngle(fovParent.transform.eulerAngles.y, -fov.rearOuterAngle);
            Vector3 outAngle2 = DirectionfromAngle(fovParent.transform.eulerAngles.y, fov.rearOuterAngle);

            Vector3 inAngle1 = DirectionfromAngle(fovParent.transform.eulerAngles.y, -fov.rearInnerAngle);
            Vector3 inAngle2 = DirectionfromAngle(fovParent.transform.eulerAngles.y, fov.rearInnerAngle);

            float rearAngleLength = fov.rearInnerAngle - fov.rearOuterAngle;

            Handles.color = Color.yellow;
            Handles.DrawLine(fovParent.transform.position, fovParent.transform.position + viewAngle1 * fov.radius);
            Handles.DrawLine(fovParent.transform.position, fovParent.transform.position + viewAngle2 * fov.radius);

            Handles.DrawLine(fovParent.transform.position, fovParent.transform.position + outAngle1 * fov.maxRearDistance);
            Handles.DrawLine(fovParent.transform.position, fovParent.transform.position + outAngle2 * fov.maxRearDistance);
            Handles.DrawWireArc(fovParent.transform.position, Vector3.up, outAngle1, -rearAngleLength, fov.maxRearDistance);


            Handles.DrawLine(fovParent.transform.position, fovParent.transform.position + inAngle1 * fov.maxRearDistance);
            Handles.DrawLine(fovParent.transform.position, fovParent.transform.position + inAngle2 * fov.maxRearDistance);
            Handles.DrawWireArc(fovParent.transform.position, Vector3.up, outAngle2, rearAngleLength, fov.maxRearDistance);


            if (fovParent.FOVStatus == FOV.FOVResult.Seen)
            {
                Handles.color = Color.green;
            }
        }
    }

    private Vector3 DirectionfromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
