using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeMethods;
using static LayerManager;
using static AbilitiesManager;

public class PopAbility : ProjectileAbility
{
    public override Abilities Ability { get; } = Abilities.Pop;

    #region Exposed In Editor
    [Header("Ray Values")]
    [SerializeField] private Layers targetLayerEnum = Layers.Enemy;
    [SerializeField] private Layers[] obstructionLayerEnum = new Layers[] { Layers.Obstruction, Layers.Ground };
    [SerializeField] private float castRadius = 1f;
    [SerializeField] private float castDistance = Mathf.Infinity;

    [Header("Pop Values")]
    [SerializeField] private int popForce = 1;
    #endregion Exposed In Editor

    #region Private
    private GameObject castOrigin; // Camera
    private LayerMask targetMask; // Enemy Layer
    private LayerMask obstructionMask; // obstruction and ground layer
    #endregion Private

    private new void Start()
    {
        if (castOrigin == null) castOrigin = GameObject.Find("Main Camera");
        targetMask = GetMask(targetLayerEnum);
        obstructionMask = GetMask(obstructionLayerEnum);
        base.Start();
    }
    public override IEnumerator HandleAbility()
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
                GameObject obj = ShootRay(ray, targetMask, obstructionMask);

                if (obj != null)
                {
                    HandlePop(obj); 
                }
            }
        }
        yield return wait;
    }
    public override void EnterAbility() { }
    public override void ExitAbility() { }
    private void HandlePop(GameObject obj)
    {
        InteractableManager im = obj.GetComponent<InteractableManager>();
        im.Pop(popForce);
    }

}
