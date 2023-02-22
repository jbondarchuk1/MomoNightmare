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
    #endregion Public

    #region Private
    private AudioManager audioManager;
    private EnemyUIManager enemyUIManager;
    private StateInitializationData OverrideData;
    private FOV Fov { get; set; }
    #endregion Private

    public StateOverrides(FOV fov, StateEnum currState, AudioManager audioManager, EnemyUIManager enemyUIManager)
    {
        this.Fov = fov;
        this.CurrState = currState;
        this.audioManager = audioManager;
        this.enemyUIManager = enemyUIManager;
    }
    public StateInitializationData GetOverride()
    {
        StateInitializationData data = OverrideData; // external use of public override methods
        OverrideData = null;
        if (CheckAggro() && CurrState != StateEnum.Attack) data = Aggro(); // automatic aggro check outprioritizes other overrides
        
        // Enemy sees player holding an object but doesnt know where the player is
        if (Fov.FOVStatus == FOV.FOVResult.AlertObject
            && (CurrState != StateEnum.Attack
            || CurrState != StateEnum.Chase
            || CurrState != StateEnum.Zombify
            )) OverrideData = new StateInitializationData(StateEnum.Alert);

        return data;
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
    public void OverrideNotFoundState(StateInitializationData data)
    {
        // Non-overrideable
        HashSet<StateEnum> exclude = new HashSet<StateEnum>()
        {
            StateEnum.Attack,
            StateEnum.Chase,
            StateEnum.Zombify
        };
        if (!exclude.Contains(CurrState))
            OverrideData = data;
    }
    private bool CheckAggro()
    {
        return Fov.FOVStatus == FOV.FOVResult.Seen && CurrState != StateEnum.Attack && CurrState != StateEnum.Chase;
    }

    private StateInitializationData Aggro(GameObject AttackedObject)
    {
        StateInitializationData stateInitializationData = new StateInitializationData();
        stateInitializationData.State = StateEnum.Chase;
        stateInitializationData.Object = AttackedObject;
        return canAggro ? stateInitializationData: null;
    }
    private StateInitializationData Aggro()
    {
        audioManager.PlaySound("Alert", "Seen");
        audioManager.PlaySound("Grunt", "Roar");
        enemyUIManager.Exclamation();
        return Aggro(GameObject.Find("Player"));
    }
}

