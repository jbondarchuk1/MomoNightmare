using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(RigidBodyNoiseStimulus))]
public class InteractableManager : MonoBehaviour
{
    #region Required
    private Rigidbody rb;
    private RigidBodyNoiseStimulus rigidbodyNoise;
    #endregion Required

    [Header("General Properties")]
    public bool canBreak = false;
    public bool canPickUp = true;
    public bool canDamage = false;
    public bool canPop = false;
    [HideInInspector] public BodyPartGore gore = null;
    [Tooltip("Optional")] public int flatDamage = 0;

    private void Start()
    {
        TryGetComponent(out gore);
        TryGetComponent(out rb);
        TryGetComponent(out rigidbodyNoise);
        if (!canBreak) canPop = false;
    }

    /// <summary>
    /// TODO:: Fix hard coded clamp values.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collided with " + collision.gameObject.name);

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EnemyManager em = collision.gameObject.GetComponent<EnemyManager>();
            EnemyStateManager esm = collision.gameObject.GetComponentInChildren<EnemyStateManager>();
            if (esm.currState == esm.patrol)
            {
                Clamp clamps = new Clamp(0f, 5f, 5f, Mathf.Infinity);
                esm.patrol.HandleCollision(this.rb.velocity, clamps.ClampVals);
            }
            if (canDamage) em.DamageEnemy(GetDamageAmount());
        }
    }

    /// <summary>
    /// Grab some amount of damage dealt to an enemy or player by the interactable rigidbody
    /// </summary>
    private int GetDamageAmount()
    {
        return flatDamage;
    }
    private int GetDamageAmount(float impulse)
    {
        return (int)impulse;
    }
    private int GetDamageAmount(Clamp clamp, float impulse)
    {
        return clamp.GetLevel(impulse);
    }

    /// <summary>
    /// Breaks in place with a sound but no force.
    /// </summary>
    public void Break()
    {
        if (canBreak)
        {
            rigidbodyNoise.Break();
            if (TryGetComponent<Destructible>(out Destructible obj))
                obj.DestroyObj();
        }
    }

    public void Pop(int force)
    {
        Debug.Log("Pop");
        Break();
        rb.AddForce(Vector3.up * force + Vector3.right * Random.Range(-1f,1f) * force/2 + Vector3.forward * Random.Range(-1f, 1f) * force/2);
    }
}