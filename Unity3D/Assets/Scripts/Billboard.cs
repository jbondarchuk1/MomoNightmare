using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public int g = 0;
    public Transform cam;

    private void Start()
    {
        if (cam == null)
        {
            cam = GameObject.Find("Main Camera").transform;
        }
    }
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
