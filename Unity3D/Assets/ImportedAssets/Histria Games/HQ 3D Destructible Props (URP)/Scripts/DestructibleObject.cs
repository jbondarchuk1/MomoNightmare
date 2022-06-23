using UnityEngine;
using StarterAssets;

/// <summary>
/// DestructibleObject script controlls all aspects of Destructible Object
/// MAKE SURE THIS SCRIPT IS ALWAYS ON PARENTS
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class DestructibleObject : Destructible
{
    /// <summary> Array that contains all children of given destructible object </summary>
    public MeshRenderer[] _destructibleObjects;
    public StarterAssetsInputs _input;

    void Start()
    {
        _destructibleObjects = GetComponentsInChildren<MeshRenderer>();
    }

    /// <summary>  This function slices parent object into _destructibleObjects variable pieces </summary> 
    public override void DestroyObj() 
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;

        foreach (MeshRenderer _dObj in _destructibleObjects)
        {
            _dObj.gameObject.AddComponent<Rigidbody>();
            _dObj.gameObject.AddComponent<MeshCollider>();
            _dObj.GetComponent<MeshCollider>().convex = true;
            _dObj.transform.SetParent(null);
        }
        this.enabled = false;
    }
}
