using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeEnemy : MonoBehaviour, IPoolUser
{
    [Header("Explosion")]
    public Transform center;
    public float radius = 5f;
    public float force = 70f;
    [SerializeField] private Transform parent;


    public ObjectPooler ObjectPooler { get; set; }
    [field: SerializeField] public string Tag { get; set; } = "Explosion";



    private void Start()
    {
        ObjectPooler = ObjectPooler.Instance;
    }

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
                Rigidbody[] deadParts = manager.childrenRigidbodies;
                manager.Die();
                foreach (Rigidbody dp in deadParts)
                    rbs.Add(dp);
            }
            else if (collider.gameObject.TryGetComponent(out Rigidbody rb))
            {
                rbs.Add(rb);
            }
        }
        foreach(Rigidbody rb in rbs)
        {
            rb.AddExplosionForce(force, position, radius);
            rb.transform.parent = parent;
        }

        MakeBoomBoomEffect();
    }
    private void MakeBoomBoomEffect()
    {
        GameObject boomboom = ObjectPooler.SpawnFromPool(Tag, transform.position, transform.rotation);
        SoundNoiseStimulus sns = boomboom.GetComponent<SoundNoiseStimulus>();

        sns.Emit();
    }

}
