using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GlobalVolumeChange : MonoBehaviour
{
    [SerializeField] private Volume toVolume;
    // Start is called before the first frame update
    private void OnEnable()
    {
        toVolume.priority = 1;
    }
    private void OnDisable()
    {
        toVolume.priority = -1;
    }
}
