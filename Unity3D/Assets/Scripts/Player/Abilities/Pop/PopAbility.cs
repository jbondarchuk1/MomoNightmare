using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeMethods;

public class PopAbility : ProjectileAbility
{
    [Header("Ray Values")]
    public GameObject castOrigin; // Camera
    public LayerMask targetMask; // Enemy Layer
    public LayerMask obstructionMask; // obstruction and ground layer
    public float radius = 1f;
    public float distance = Mathf.Infinity;

    [Header("Pop Values")]
    public int popForce = 1;

    private void Start()
    {
        if (castOrigin == null) castOrigin = GameObject.Find("Main Camera");
    }
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
                GameObject obj = RayShoot(ray, targetMask, obstructionMask);

                if (obj != null)
                {
                    HandlePop(obj); 
                }
            }
        }
        yield return wait;
    }

    private void HandlePop(GameObject obj)
    {
        InteractableManager im = obj.GetComponent<InteractableManager>();
        im.Pop(popForce);
    }

}
