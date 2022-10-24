using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    #region Public
        #region InvisibleInInspector
        public enum StateEnum { None, Patrol, SearchPatrol, Alert, Chase, Attack, Zombify };
            [HideInInspector] public StateOverrides Overrides { get; set; }
            public StateEnum currState;
        #endregion InvisibleInInspector
    
        // Interactability bools
        public bool canZombify = true;

    #endregion Public

    #region Private
        private EnemyNavMesh enm;
        [HideInInspector] public FOV fov;
    #endregion Private

    #region States
        // Player Not Found
        private Patrol patrol;
        private SearchPatrol searchPatrol;
        private Alert alert;

        // Player Found
        private Chase chase;
        private Attack attack;

        // Controlled
        private Zombified zombified;
    #endregion States

    void Awake()
    {
        RequireStates();
    }

    private void Start()
    {
        enm = GetComponentInParent<EnemyNavMesh>();
        fov = GetComponentInParent<FOV>();
        currState = StateEnum.Patrol; // default to patrol
        Overrides = new StateOverrides(fov, currState);
    }
    private void Update()
    {
        StartCoroutine(RunStateMachine());
    }

    private IEnumerator RunStateMachine()
    {
        Overrides.CurrState = currState;
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        StateInitializationData stateInitializationData = Overrides.GetOverride();
        StateEnum nextState = stateInitializationData == null ? currState :stateInitializationData.State;

        // Override takes priority
        // only run the state if we are staying in this state
        if (stateInitializationData == null && currState == nextState)
        {
            stateInitializationData = GetState().RunCurrentState(enm, fov);
        }

        ChangeToState(stateInitializationData);
        yield return wait;
    }

    private void ChangeToState(StateInitializationData data)
    {
        GetState().ExitState();
        InitializeState(data);
        currState = data.State;
    }

    private void RequireStates()
    {
        // Player Not Found
        patrol = GetComponentInChildren<Patrol>();
        searchPatrol = GetComponentInChildren<SearchPatrol>();
        alert = GetComponentInChildren<Alert>();

        // Player Found
        chase = GetComponentInChildren<Chase>();
        attack = GetComponentInChildren<Attack>();
        
        // controlled
        zombified = GetComponentInChildren<Zombified>();
    }

    public State GetState(StateEnum state)
    {
        switch (state)
        {
            case StateEnum.Patrol:
                return patrol;
            case StateEnum.Zombify:
                return zombified;
            case StateEnum.SearchPatrol:
                return searchPatrol;
            case StateEnum.Alert:
                return alert;
            case StateEnum.Chase:
                return chase;
            case StateEnum.Attack:
                return attack;
            default: return patrol;
        }
    }
    public State GetState()
    {
        return GetState(currState);
    }

    public State InitializeState(StateInitializationData data)
    {
        State state = GetState(data.State);
        state.InitializeState(data);
        return state;
    }
}

