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

    [Header("Pop Values")]
    [SerializeField] private int popForce = 100;
    #endregion Exposed In Editor

    #region Private
    private LayerMask targetMask; // Enemy Layer
    private LayerMask obstructionMask; // obstruction and ground layer
    #endregion Private

    private void Start()
    {
        targetMask = GetMask(targetLayerEnum);
        obstructionMask = GetMask(obstructionLayerEnum);
    }
    public override IEnumerator HandleAbility()
    {
        WaitForSeconds wait = new WaitForSeconds(.2f);
        if (_inputs.actionPressed && GetWaitComplete(this.endTime))
        {
            this.endTime = GetWaitEndTime(this.coolDownTimer);
            Transform castCam = Cam.transform;

            Ray ray = new Ray(castCam.position, castCam.forward);
            GameObject obj = ShootRay(ray, targetMask, obstructionMask);
                
            if (obj != null) HandlePop(obj); 
        }
        yield return wait;
    }
    public override void EnterAbility() { }
    public override void ExitAbility() { }
    private void HandlePop(GameObject obj)
    {
        if (obj.TryGetComponent(out BreakableInteractableManager im))
            im.Pop(popForce);
    }

}
