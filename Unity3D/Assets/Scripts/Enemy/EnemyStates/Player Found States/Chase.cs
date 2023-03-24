using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemyStateManager;
using static LayerManager;

/// <summary>
/// 
/// </summary>
public class Chase : State
{
    public override StateEnum StateEnum { get; } = StateEnum.Chase;

    #region Exposed In Editor

        [SerializeField] private bool hitBoxActive = false;
        [SerializeField] private float attackDistance = 1f;
        [SerializeField] private float maxChaseDistance = 20f;

    #endregion Exposed In Editor

    #region Private
        private GameObject PlayerRef;
        private GameObject AttackedObject;

    #endregion Private

    #region Start and Update
    private void Start()
    {
        PlayerRef = PlayerManager.Instance.gameObject;
    }
    #endregion Start and Update

    /// <summary>
    /// Chase state returns either chase or search patrol of last seen position.
    /// </summary>
    public override StateInitializationData RunCurrentState(EnemyNavMesh enm, FOV fov)
    {
        enm.SetSpeed(NavMeshSpeed);
        if (PlayerInRange()) return new StateInitializationData(StateEnum.Attack, AttackedObject);

        if (AttackedObject.activeInHierarchy && Vector3.Distance(transform.position, AttackedObject.transform.position) < maxChaseDistance)
        {
            ChaseAfter(enm);
            return new StateInitializationData(StateEnum, AttackedObject);
        }

        Debug.Log("Exiting chase");
        ExitState();
        enm.Chase(null);
        return new StateInitializationData(StateEnum.Patrol);
        
    }
    public void InitializeState()
    {
        Debug.Log("Entering Chase State");
        this.AttackedObject = PlayerRef;
    }
    public override void InitializeState(StateInitializationData data)
    {
        this.AttackedObject = data.Object;
    }
    public override void ExitState()
    {
        AttackedObject = null;
    }
    public override StateInitializationData Listen(Vector3 soundOrigin, int intensity)
    {
        return new StateInitializationData(StateEnum);
    }
    private void ChaseAfter(EnemyNavMesh enm)
    {
        enm.Chase(AttackedObject);
    }

    private bool PlayerInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackDistance, GetMask(Layers.Target));
        Debug.Log("Num colliders: " + colliders.Length);
        return colliders.Length != 0;
    }


}
