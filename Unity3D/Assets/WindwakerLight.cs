using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LayerManager;

public class WindwakerLight : MonoBehaviour
{
    MeshRenderer parentRenderer;
    MeshRenderer[] renderers;

    private void Awake()
    {
        parentRenderer = GetComponent<MeshRenderer>();
        renderers = GetComponentsInChildren<MeshRenderer>();
    }

    private void OnBecameVisible()
    {
        //HandleOcclude(false);
    }
    private void OnBecameInvisible()
    {
        //HandleOcclude(true);
    }

    private void HandleOcclude(bool occlude = false)
    {
        foreach (MeshRenderer r in renderers) r.enabled = !occlude;
        parentRenderer.enabled = true;

    }


}
