using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public Transform bottom;
    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalVector("_PositionMoving", bottom.position);
    }
}
