using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAim : MonoBehaviour
{
    InputManager inputManager;
    public float turnSpeed = 1;
    public float aimDuration = 0.3f;
    public Transform lookAt;
    private Vector2 mouseVal;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        mouseVal = inputManager.mouseVal;


        float lookx = mouseVal.x;
        float looky = -1 * mouseVal.y;
        Debug.Log(lookx);

        Quaternion newRot = lookAt.localRotation;
        newRot *= Quaternion.AngleAxis(lookx, Vector3.up);
        
        lookAt.localRotation = newRot;
    
    }
}
