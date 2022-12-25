using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeMethods;
using static LayerManager;
using static AbilitiesManager;

public class Scan : ProjectileAbility, IPoolUser
{
    public override Abilities Ability { get; } = Abilities.Scan;

    #region Private
        private LayerMask targetMask; // Enemy Layer
        private LayerMask obstructionMask; // obstruction and ground layer
        private GameObject enemyInView;
    #endregion Private

    #region Exposed In Editor
        [Header("Settings")]
        [SerializeField] private float castRadius = 1f;
        [SerializeField] private float distance = Mathf.Infinity;
        [SerializeField] private Layers targetLayerEnum = Layers.Enemy;
        [SerializeField] private Layers[] obstructionLayerEnum = new Layers[] { Layers.Obstruction };
        [SerializeField] private Transform castOrigin; // Camera 
    #endregion Exposed In Editor

    public ObjectPooler ObjectPooler { get; set; }
    public string Tag { get; set; } = "EnemyHighlight";
    private SelectHandler selectHandler;

    private new void Start()
    {
        base.Start();
        ObjectPooler = ObjectPooler.Instance;
        if (castOrigin == null) castOrigin = GameObject.Find("Main Camera").transform;
        targetMask = GetMask(targetLayerEnum);
        obstructionMask = GetMask(obstructionLayerEnum);
        selectHandler = new SelectHandler();
    }
    private GameObject CheckForEnemy()
    {
        Transform castCam = castOrigin;
        GameObject enemy = ShootRay(castCam, targetMask, obstructionMask);
        
        bool selected = false;
        if (enemy != null)
        {
            if(enemy.TryGetComponent(out EnemyManager em))
            {
                GameObject canvas = em.canvas;
                if (canvas.layer != LayerMask.NameToLayer("PriorityUI"))
                {
                    selectHandler.Select(enemy.transform);
                    selected = true;
                }
            }
        }
        if (!selected) selectHandler.Deselect();
        return enemy;
    }
    public override IEnumerator HandleAbility()
    {
        WaitForSeconds wait = new WaitForSeconds(.2f);
        enemyInView = CheckForEnemy();
        bool isShooting = _inputs.actionPressed && GetWaitComplete(this.endTime);
        SetShootAnimation(isShooting);
        this.endTime = TimeMethods.GetWaitEndTime(this.coolDownTimer);
        yield return wait;
    }
    public override void Shoot()
    {
        if (enemyInView != null)
        {
            if (enemyInView.TryGetComponent(out EnemyManager em))
            {
                GameObject canvas = em.canvas;
                canvas.SetActive(true);
                canvas.layer = LayerMask.NameToLayer("PriorityUI");
                enemyInView.GetComponentInChildren<EnemyUIManager>().SetLayer("PriorityUI");
            }
        }
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
