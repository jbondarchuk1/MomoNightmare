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

    private new void Start()
    {
        base.Start();
        targetMask = GetMask(targetLayerEnum);
        obstructionMask = GetMask(obstructionLayerEnum);
    }
    public override IEnumerator HandleAbility()
    {
        WaitForSeconds wait = new WaitForSeconds(.2f);
        SetShootAnimation(_inputs.actionPressed && GetWaitComplete(this.endTime));
        yield return wait;
    }
    public override void Shoot()
    {
        PlayerManager.Instance.audioManager.PlaySound("ProjectileSpawn", "Pop");

        this.endTime = GetWaitEndTime(this.coolDownTimer);
        Transform castCam = Cam.transform;

        Ray ray = new Ray(castCam.position, castCam.forward);
        GameObject obj = ShootRay(ray, targetMask, obstructionMask);

        if (obj != null) HandlePop(obj);
    }
    public override void EnterAbility() 
    {
        PlayerAnimationEventHandler.OnShoot += Shoot;
    }
    public override void ExitAbility() 
    { 
        PlayerAnimationEventHandler.OnShoot -= Shoot;
    }
    private void HandlePop(GameObject obj)
    {
        if (obj.TryGetComponent(out BreakableInteractableObject im))
            im.Pop(popForce);
    }
}
