using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeMethods;

public class Scan : ProjectileAbility
{
    public LayerMask targetMask; // Enemy Layer
    public LayerMask obstructionMask; // obstruction and ground layer
    public float radius = 1f;
    public float distance = Mathf.Infinity;
    public Transform castOrigin; // Camera

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(HandleScan());
    }

    private IEnumerator HandleScan()
    {
        WaitForSeconds wait = new WaitForSeconds(.2f);
        if (_inputs.mouseL && _inputs.mouseR)
        {
            _inputs.mouseL = false;
            if (GetWaitComplete(this.endTime))
            {
                this.endTime = Time.time + this.coolDownTimer;
                GameObject enemy = CapsuleRayShoot(castOrigin, targetMask, obstructionMask);
                if (enemy != null)
                {
                    enemy.GetComponent<EnemyManager>().canvas.layer = LayerMask.NameToLayer("PriorityUI");
                    enemy.GetComponentInChildren<EnemyUIManager>().SetLayer("PriorityUI");
                }
            }
        }
        yield return wait;
    }

}
