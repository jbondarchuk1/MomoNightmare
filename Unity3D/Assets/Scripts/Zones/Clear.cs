using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clear : Zone
{
    private bool inZone = false;
    private float startFog;
    public float clearFog = 0f;
    [Range(0f,10f)] public float clearRate = 5f;

    private void Start()
    {
        startFog = RenderSettings.fogDensity;
    }
    void Update()
    {
        FogFadeHandler();
    }

    private void FogFadeHandler()
    {
        if (inZone && RenderSettings.fogDensity > clearFog)
            RenderSettings.fogDensity -= Time.deltaTime * clearRate;
        else if (!inZone && RenderSettings.fogDensity < startFog)
            RenderSettings.fogDensity += Time.deltaTime * clearRate;
    }
}
