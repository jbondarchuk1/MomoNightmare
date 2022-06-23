using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeEnemy : MonoBehaviour
{
    public Transform center;
    public float radius = 5f;
    public float force = 70f;
    public GameObject explosionEffect;
    [SerializeField] private Transform parent;
    private NoiseStimulus explosionStimulus;

    public void Explode()
    {
        Vector3 position = center.position;
        System.Random rnd = new System.Random();
        position.y -= 1;
        int nxt = 1;
        float factor = 0.7f;
        position.x -= rnd.Next(-nxt, nxt) * factor; 
        position.z -= rnd.Next(-nxt, nxt) * factor;
        HashSet<Rigidbody> rbs = new HashSet<Rigidbody>();
        Collider[] colliders = Physics.OverlapSphere(position, radius);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.TryGetComponent(out EnemyManager manager))
            {
                Rigidbody[] deadParts = GetRigidBodies(manager.Die());
                foreach (Rigidbody dp in deadParts)
                    rbs.Add(dp);
            }
            else if (collider.gameObject.TryGetComponent(out DestructibleObject destObj))
            {
                destObj.DestroyObj();
                MeshRenderer[] destroyedParts = destObj._destructibleObjects;

                foreach (MeshRenderer mr in destroyedParts)
                    if (mr.gameObject.TryGetComponent(out Rigidbody destObjRb))
                        rbs.Add(destObjRb);
            }
            if (collider.gameObject.TryGetComponent(out Rigidbody rb))
            {
                rbs.Add(rb);
            }
        }
        foreach(Rigidbody rb in rbs)
        {
            rb.AddExplosionForce(force, position, radius);
            rb.transform.parent = parent;
        }

        MakeBoomBoomEffect(center.position);
        MakeBoomBoomSound(2f);
    }
    private void MakeBoomBoomSound(float duration)
    {
        if (explosionStimulus == null)
        {
            explosionStimulus = gameObject.AddComponent<NoiseStimulus>();
        }
        explosionStimulus.endTime = TimeMethods.GetWaitTime(duration);
        explosionStimulus.location = center;
    }
    private Rigidbody[] GetRigidBodies(GameObject gameObject)
    {
        return gameObject.GetComponentsInChildren<Rigidbody>();
    }

    private void MakeBoomBoomEffect(Vector3 position)
    {
        GameObject boomboom = Instantiate(explosionEffect, position, transform.rotation);
        ExplosionHandler explosionControl = boomboom.GetComponent<ExplosionHandler>();
        explosionControl.spawnLocation = position;
    }

}
