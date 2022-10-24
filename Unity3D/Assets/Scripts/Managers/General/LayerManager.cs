using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wrapper around Unity's LayerMasks. Now we are able to use a semantic enumeration
/// without designating layers as strings.
/// </summary>
public static class LayerManager
{
    public enum Layers {
        Default, TransparentFX, IgnoreRaycast, Water, UI, Target, Obstruction, Lighting,
        Ground, PostProcessing, Routes, Trees, Enemy, Zones, RayOnly, Interactable, PriorityUI
    }
    public static string[] LayerNames = "Default TransparentFX IgnoreRaycast Water UI Target Obstruction Lighting Ground PostProcessing Routes Trees Enemy Zones RayOnly Interactable PriorityUI".Replace("\n",String.Empty).Split(' ');

    public static LayerMask GetMask(Layers[] layers)
    {
        List<string> layerNames = new List<string>();
        foreach (Layers layer in layers)
            layerNames.Add(LayerNames[(int)layer]);

        LayerMask mask = new LayerMask();
        foreach (string name in layerNames)
        {
            mask |= LayerMask.GetMask(name);
        }

        return mask;
    }
    public static LayerMask GetMask(Layers layer)
    {
        return LayerMask.GetMask(LayerNames[(int)layer]);
    }
    public static int GetLayer(Layers layer)
    {
        return LayerMask.NameToLayer(LayerNames[(int)layer]);
    }
}
