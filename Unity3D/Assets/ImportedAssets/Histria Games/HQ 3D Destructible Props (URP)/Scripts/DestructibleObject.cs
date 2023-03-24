using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// DestructibleObject script controlls all aspects of Destructible Object
/// MAKE SURE THIS SCRIPT IS ALWAYS ON PARENTS
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class DestructibleObject : MonoBehaviour, IDestructable, IPoolUser
{
    /// <summary> Array that contains all children of given destructible object </summary>
    private MeshRenderer[] _destructibleObjects;
    private List<Rigidbody> rigidbodies = new List<Rigidbody>();

    [SerializeField] private float breakImpulse = 1f;
    public bool isBroken = false;

    public ObjectPooler ObjectPooler { get; set; }
    [field: SerializeField] public string Tag { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        ObjectPooler = ObjectPooler.Instance;
        // Auto assigns all _destructibleObjects objects of which this one is a parent,
        // This way there is no need to manually drag and drop game objects from inspector
        // Make sure that childrens have mesh renderer on them
        _destructibleObjects = GetComponentsInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // This is just for testing, object is destroyed when player press q,
        // you can add your own condition here instead
        if (Input.GetKeyDown(KeyCode.Q))
                DestroyObj();
    }

    // This function slices parent object into _destructibleObjects variable pieces
    public void DestroyObj() 
    {
        // Removes parental rigidbody and collider so we dont collide with them
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
        // Now we go throught array of _destructibleObjects and make all children object independent!
        // add rigidbody and meshcollider on them so we get that effect of destructibliy
        foreach (MeshRenderer _dObj in _destructibleObjects)
        {
            // We need a default rigidbody for _destructibleObjects to have physics once separated from parental object 
            rigidbodies.Add( _dObj.gameObject.AddComponent<Rigidbody>());

            // Mesh collider is best for smooth collision, but you can use other colliders
            _dObj.gameObject.AddComponent<MeshCollider>();
            _dObj.GetComponent<MeshCollider>().convex = true;

            // This makes sure _destructibleObjects become independent and move their own way
            _dObj.transform.SetParent(null);
        }
        isBroken = true;
        // This is here temporary to remove script from updating
        this.enabled = false;

        if (Tag != "") ObjectPooler.SpawnFromPool(Tag, transform.position, transform.rotation);
    }

    public void ExplodeObj(Vector3 origin, float force = 5)
    {
        DestroyObj();
        foreach (Rigidbody rb in rigidbodies)
            rb.AddExplosionForce(force*3, origin, force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.impulse);
        if (collision.impulse.magnitude >= breakImpulse)
            DestroyObj();
    }
}
