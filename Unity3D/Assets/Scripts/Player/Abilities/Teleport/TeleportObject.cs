using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportObject : MonoBehaviour
{
    public Transform TeleportationTarget { get; set; }

    private void Awake()
    {
        if (transform.childCount > 0)
        {
            TeleportationTarget = transform.GetChild(0);
        }
    }
}
