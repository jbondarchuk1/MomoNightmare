using Cinemachine;
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
    public GameObject castOrigin; // Camera

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
                Camera castCam = castOrigin.GetComponent<Camera>();

                Ray ray = new Ray(castCam.transform.position, castCam.transform.forward);
                
                GameObject enemy = RayShoot(ray, targetMask, obstructionMask);

                if (enemy != null)
                {
                    GameObject canvas = enemy.GetComponent<EnemyManager>().canvas;
                    canvas.SetActive(true);
                    canvas.layer = LayerMask.NameToLayer("PriorityUI");
                    enemy.GetComponentInChildren<EnemyUIManager>().SetLayer("PriorityUI");
                }
            }
        }
        yield return wait;
    }

}
