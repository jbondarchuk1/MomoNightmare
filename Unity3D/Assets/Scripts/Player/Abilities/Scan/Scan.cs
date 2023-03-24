using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeMethods;
using static LayerManager;
using static AbilitiesManager;
using static TimeMethods;

public class Scan : ProjectileAbility, IPoolUser
{
    public override Abilities Ability { get; } = Abilities.Scan;

    #region Private
        private LayerMask targetMask; // Enemy Layer
        private LayerMask obstructionMask; // obstruction and ground layer
        private GameObject enemyInView;
        private Transform castOrigin; // Camera 
        private float highlightEndTime = 0f;
    #endregion Private

    #region Exposed In Editor
    [Header("Settings")]
        [SerializeField] private float castRadius = 1f;
        [SerializeField] private float distance = Mathf.Infinity;
        [SerializeField] private Layers targetLayerEnum = Layers.Enemy;
        [SerializeField] private Layers[] obstructionLayerEnum = new Layers[] { Layers.Obstruction };
    #endregion Exposed In Editor

    [field:SerializeField]public string HighlightTag { get; private set; } = "EnemyHighlight";
    [field:SerializeField]public string ScanConfirmTag { get; private set; } = "ScanConfirm";
    private SelectHandler selectHandler;
    [SerializeField] private float highlightWaitTime = 1f;

    private new void Start()
    {
        base.Start();
        ObjectPooler = ObjectPooler.Instance;
        if (castOrigin == null) castOrigin = PlayerManager.Instance.camera;
        targetMask = GetMask(targetLayerEnum);
        obstructionMask = GetMask(obstructionLayerEnum);
        selectHandler = new SelectHandler();
    }
    private GameObject CheckForEnemy()
    {
        Transform castCam = castOrigin;
        Ray ray = new Ray(castCam.position, castCam.forward);
        GameObject enemy = ShootCapsuleRay(100f, .3f, ray, targetMask, obstructionMask);

        if (enemy != null)
        {
            EnemyManager em = enemy.GetComponentInChildren<EnemyManager>();
            if (!em.enemyUIManager.isSelected)
            {
                selectHandler.Select(em.gameObject.transform);
                highlightEndTime = GetWaitEndTime(highlightWaitTime);
            }
        }
        else if (GetWaitComplete(highlightEndTime) && selectHandler.isSelected())
        {
            highlightEndTime = 0f;
            selectHandler.Deselect();
        }
        return enemy;
    }
    public override IEnumerator HandleAbility()
    {
        if (_inputs == null) _inputs = StarterAssetsInputs.Instance;

        WaitForSeconds wait = new WaitForSeconds(.2f);
        enemyInView = CheckForEnemy();
        bool isShooting = _inputs.actionPressed && GetWaitComplete(this.endTime);
        SetShootAnimation(isShooting);
        yield return wait;
    }
    public override void Shoot()
    {
        PlayerManager.Instance.audioManager.PlaySound("ProjectileSpawn", "Scan");

        if (enemyInView != null)
        {
            if (enemyInView.TryGetComponent(out EnemyManager em))
            {
                em.enemyUIManager.SpotEnemy();
                Vector3 pos = enemyInView.transform.position;
                pos.y += 1.5f;
                ObjectPooler.SpawnFromPool(ScanConfirmTag, pos, Quaternion.identity);
            }
        }
        
        this.endTime = GetWaitEndTime(this.coolDownTimer);
        SetShootAnimation(false);
        EnableWandParticles();
        enemyInView = null;
    }
    public override void EnterAbility()
    {
        PlayerAnimationEventHandler.OnShoot += Shoot;
    }
    public override void ExitAbility()
    {
        PlayerAnimationEventHandler.OnShoot -= Shoot;
    }
}
