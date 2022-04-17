using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    [InspectorName("Current State")] public State currState; // can set in editor, default to idle

    [HideInInspector] public State stateOverride; 
                      private EnemyNavMesh enm;
    [HideInInspector] public FOV fov;

    // STATES
    [HideInInspector] public Idle idle;
    [HideInInspector] public Patrol patrol;
    [HideInInspector] public Chase chase;
    [HideInInspector] public SearchPatrol searchPatrol;
    [HideInInspector] public Alert alert;
    [HideInInspector] public Attack attack;
    
    [HideInInspector] public StateOverrides overrides;


    // Interactability bools
    public bool canZombify = true;
    public bool canDetonate = true;

    void Awake()
    {
        RequireStates();
    }

    private void Start()
    {
        enm = GetComponentInParent<EnemyNavMesh>();
        fov = GetComponentInParent<FOV>();

        if (currState == null)
            currState = idle; // default to Idle
    }
    private void Update()
    {
        StartCoroutine(RunStateMachine());
    }

    private IEnumerator RunStateMachine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        State nextState;

        overrides.HandleRegularOverrides();

        if (stateOverride != null)
        {
            nextState = stateOverride;
            stateOverride = null;
        }
        else
        {
            nextState = currState.RunCurrentState(enm, fov);
        }

        // debug purposes to check ai state changes
        if (currState != nextState)
            Debug.Log(nextState.GetType());

        if (nextState != null)
            UpdateState(nextState);

        yield return wait;
    }

    private void UpdateState(State nextState)
    {
        currState = nextState;
    }

    private void RequireStates()
    {
        chase = GetComponentInChildren<Chase>();
        idle = GetComponentInChildren<Idle>();
        patrol = GetComponentInChildren<Patrol>();
        searchPatrol = GetComponentInChildren<SearchPatrol>();

        // NOT IMPLEMENTED YET
        alert = GetComponentInChildren<Alert>();
        attack = GetComponentInChildren<Attack>();

        // Extra -- CROSS DEPENDENCY
        overrides = transform.gameObject.AddComponent<StateOverrides>();
        overrides.esm = this;
    }

    public void SetOverrideState(State state)
    {
        stateOverride = state;
    }
    public void SetOverrideState(State state, bool trueReference)
    {
        if (!trueReference)
        {
            Type type = state.GetType();

            if      (type == typeof(Chase))         state = chase;
            else if (type == typeof(Idle))          state = idle;
            else if (type == typeof(Patrol))        state = patrol;
            else if (type == typeof(SearchPatrol))  state = searchPatrol;
            else if (type == typeof(Alert))         state = alert;
            else if (type == typeof(Attack))        state = attack;
            else                                    state = idle;
        }
        SetOverrideState(state);
    }
}

