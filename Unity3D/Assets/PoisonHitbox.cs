using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonHitbox : MonoBehaviour
{
    [SerializeField] private float sleepTimer = 1f;
    [SerializeField] private int damage = 10;

    private bool isSleeping = false;

    HashSet<IDamageable> damaging = new HashSet<IDamageable>();
    [HideInInspector] public bool active = false;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered trigger");
        if (other.TryGetComponent(out IDamageable damageable))
        {
            Debug.Log(other.gameObject.name + " is damageable");
            damaging.Add(damageable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Something exited trigger");
        if (other.TryGetComponent(out IDamageable damageable))
            if (damaging.Contains(damageable)) damaging.Remove(damageable);
    }

    void Update()
    {
        if (!active) damaging.Clear();
        if (!isSleeping) StartCoroutine(ApplyPoisonDamage());
    }

    private void ClearPoison()
    {
        // GetComponent<Collider>().bounds.
        damaging.Clear();
    }

    private IEnumerator ApplyPoisonDamage()
    {
        isSleeping = true;
        foreach (IDamageable d in damaging) d.Damage(damage);
        yield return new WaitForSeconds(sleepTimer);
        isSleeping = false;
    }
}
