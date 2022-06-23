using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEffect : IgnoreCollision
{
    PlayerTiedEffectsManager effects;
    PlayerStats stats;

    private void Start()
    {
    }

    private IEnumerator HandleUnderwater()
    {
        WaitForSeconds wait = new WaitForSeconds(2f);
        if (stats != null)
        {
            if (stats.stamina - 10 > 0)
                stats.stamina -= 10;
            else if (stats.stamina < 0)
            {
                stats.stamina = 0;

            }
        }
        yield return wait;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            stats = other.gameObject.GetComponent<PlayerStats>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        stats = null;
    }
}
