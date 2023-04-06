using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeEnemy : MonoBehaviour
{
    EnemyManager em;
    private void Start() =>  em = GetComponent<EnemyManager>();  

    public void Explode(Vector3 position, float radius, int force, int damage)
    {
        try
        {
            if (TryGetComponent(out EnemyManager manager))
            {
                manager.Damage(damage, true);

                Rigidbody[] deadParts = manager.childrenRigidbodies;
                if (manager.enemyStats.health <= 0)
                {
                    manager.Die();
                    foreach (Rigidbody rb in deadParts) rb.AddExplosionForce(force, position, radius);
                }
            }
        }
        catch(Exception ex)
        {
            Debug.LogException(ex);
        }
    }

}
