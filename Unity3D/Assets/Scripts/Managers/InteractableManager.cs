using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    public bool canPickUp = true;
    [HideInInspector] public BodyPartGore gore = null;

    public class Clamp
    {
        public float[][] clampVals { get; set; } = new float[][] { new float[] { 0f, 1f }, new float[] { 0f, 1f } };
        public Clamp(float min1, float max1, float min2, float max2)
        {
            this.clampVals = new float[][] { new float[] { min1, max1 }, new float[] { min2, max2 } };
        }
    }
    private void Start()
    {
        TryGetComponent(out gore);
    }

    RequireComponent reqRigidbody = new RequireComponent(typeof(Rigidbody));
    RequireComponent reqCollider = new RequireComponent(typeof(Collider));
    RequireComponent reqRigidNoiseStimulus = new RequireComponent(typeof(RigidBodyNoiseStimulus));

    private Rigidbody rb;
    private Collider collider;
    private RigidBodyNoiseStimulus rigidbodyNoise; 

    /// <summary>
    /// TODO:: Fix hard coded clamp values.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.GetMask("Enemy"))
        {
            Debug.Log("Collided with enemy");
            EnemyStateManager esm = collision.gameObject.GetComponentInChildren<EnemyStateManager>();
            if (esm.currState == esm.patrol)
            {
                Clamp clamps = new Clamp(0f, 5f, 5f, Mathf.Infinity);
                esm.patrol.HandleCollision(this.rb.velocity, clamps.clampVals);
            }
        }
    }
}