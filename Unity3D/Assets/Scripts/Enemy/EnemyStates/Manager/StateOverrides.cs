using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyStateManager;

public class StateOverrides
{
    // Fields and Attributes
    #region Public
    public bool canAggro = true;
    [HideInInspector] public StateEnum CurrState { private get; set; }
    private StateEnum delayOverrideState = StateEnum.None;
    #endregion Public

    #region Private
    private AudioManager audioManager;
    private EnemyUIManager enemyUIManager;
    private StateInitializationData OverrideData;
    private EnemyManager enemyManager;
    private FOV Fov { get; set; }
    #endregion Private

    public StateOverrides(FOV fov, StateEnum currState, AudioManager audioManager,
        EnemyUIManager enemyUIManager, EnemyManager enemyManager)
    {
        this.Fov = fov;
        this.CurrState = currState;
        this.audioManager = audioManager;
        this.enemyUIManager = enemyUIManager;
        this.enemyManager = enemyManager;
    }
    public StateInitializationData GetOverride()
    {
        StateInitializationData data = OverrideData; // external use of public override methods
        OverrideData = null;

        if (CheckAggro() && CurrState != StateEnum.Attack) data = Aggro(); // automatic aggro check outprioritizes other overrides
        // Enemy sees player holding an object but doesnt know where the player is
        else if (OverrideData == null
            && Fov.FOVStatus == FOV.FOVResult.AlertObject
            && (CurrState != StateEnum.Attack
            || CurrState != StateEnum.Chase
            || CurrState != StateEnum.Zombify
            )) OverrideData = new StateInitializationData(StateEnum.Alert);
        
        if (data == null) return data;

        if (CurrState == StateEnum.TakeDamage && data.State != StateEnum.TakeDamage)
        {
            delayOverrideState = data.State;
            data.State = StateEnum.TakeDamage;
            enemyManager.enemyAnimationEventHandler.OnEndDamage += SetState;
        }
        return data;
    }
    private void SetState() 
    {
        OverrideData = new StateInitializationData(delayOverrideState);
        delayOverrideState = StateEnum.None;
        enemyManager.enemyAnimationEventHandler.OnEndDamage -= SetState;
    }
    public void Zombify(Vector3 destination)
    {
        if (CurrState != StateEnum.Chase && CurrState != StateEnum.Attack)
        {
            canAggro = false;
            OverrideData = new StateInitializationData(StateEnum.Zombify, destination);
        }
    }
    public void Zombify(StateInitializationData data)
    {
        if (   CurrState != StateEnum.Chase && CurrState != StateEnum.Attack)
        {
            canAggro = false;
            OverrideData = data;
        }
    }
    public void Search(Vector3 location)
    {
        if (CurrState == StateEnum.Patrol)
        {
            OverrideNotFoundState(new StateInitializationData(StateEnum.SearchPatrol, location));
        }
    }
    public void Search()
    {
        if (CurrState == StateEnum.Patrol)
        {
            OverrideNotFoundState(new StateInitializationData(StateEnum.SearchPatrol, PlayerManager.Instance.gameObject));
        }
    }
    public void Alert()
    {
        if (CurrState != StateEnum.Zombify)
            OverrideNotFoundState(new StateInitializationData(StateEnum.Alert));
    }
    /// <summary>
    /// 
    /// </summary>
    public void TakeDamage()
    {
        
        OverrideData = new StateInitializationData(StateEnum.TakeDamage, CurrState);
    }
    public void OverrideNotFoundState(StateInitializationData data)
    {
        // Non-overrideable
        HashSet<StateEnum> exclude = new HashSet<StateEnum>()
        {
            StateEnum.Attack,
            StateEnum.Chase,
            StateEnum.Zombify,
            StateEnum.TakeDamage
        };
        if (!exclude.Contains(CurrState))
            OverrideData = data;
    }
    private bool CheckAggro()
    {
        return Fov.FOVStatus == FOV.FOVResult.Seen 
            && CurrState != StateEnum.Attack 
            && CurrState != StateEnum.Chase 
            && CurrState != StateEnum.TakeDamage;
    }
    public void AggroExternal()
    {
        Aggro();
    }
    private StateInitializationData Aggro(GameObject AttackedObject)
    {
        StateInitializationData stateInitializationData = new StateInitializationData();
        stateInitializationData.State = StateEnum.Chase;
        stateInitializationData.Object = AttackedObject;
        return stateInitializationData;
    }
    private StateInitializationData Aggro()
    {
        if (CurrState == StateEnum.TakeDamage)
            return new StateInitializationData(CurrState);
        canAggro = false;
        StateInitializationData data = Aggro(PlayerManager.Instance.gameObject);
        if ((CurrState == StateEnum.Attack || CurrState == StateEnum.Chase)) return data;
        enemyManager.enemyAnimationEventHandler.OnSurprise += EndSurprise;
        enemyManager.animator.SetBool("isSurprised", true);
        audioManager.PlaySound("Alert", "Seen");
        audioManager.PlaySound("Grunt", "Roar");
        enemyUIManager.Exclamation();

        return new StateInitializationData(CurrState);
    }

    private void EndSurprise()
    {
        OverrideData = new StateInitializationData();
        OverrideData.State = StateEnum.Chase;
        OverrideData.Object = PlayerManager.Instance.gameObject;
        enemyManager.animator.SetBool("isSurprised", false);
        canAggro = true;
    }
}

