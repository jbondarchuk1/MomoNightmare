using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO Include ECS subscenes
public class SubsceneReferences : MonoBehaviour
{
    public float loadDistance = 10f;
    public static SubsceneReferences Instance { get; private set; }

    // public SubScene[] Subscenes;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else GameObject.Destroy(this);
    }
}
