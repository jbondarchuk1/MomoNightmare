using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupManager : MonoBehaviour
{
    EnemyManager[] enemies;
    float endTime = 0f;
    public float alertDuration;
    [SerializeField] private float autoAggroRadius = 10;
    bool idle = false;
    public enum EnemyGroupState { Default, Aggro, Alert }
    [field: SerializeField] public EnemyGroupState State { get; private set; }
    private void Start()
    {
        enemies = GetComponentsInChildren<EnemyManager>();
    }
    private void Update()
    {
        CheckState();
        if (!idle) StartCoroutine(Tick(1));
    }
    private IEnumerator Tick(float wait)
    {
        idle = true;
        yield return new WaitForSeconds(wait);
        switch (State)
        {
            case EnemyGroupState.Aggro:
                endTime = TimeMethods.GetWaitEndTime(alertDuration);
                foreach (EnemyManager enemy in enemies)
                {
                    if (EnemyIsAggro(enemy)) break;
                    if (Vector3.Distance(enemy.transform.position, PlayerManager.Instance.transform.position) < autoAggroRadius)
                    {
                        Debug.Log("Force aggro enemy");
                        enemy.esm.Overrides.AggroExternal();
                    }
                    else enemy.esm.Overrides.Alert();
                }
                break;

            case EnemyGroupState.Alert:
                if (TimeMethods.GetWaitComplete(endTime))
                {
                    foreach (EnemyManager enemy in enemies)
                        enemy.esm.Overrides.OverrideNotFoundState(new StateInitializationData(EnemyStateManager.StateEnum.Patrol));
                    State = EnemyGroupState.Default;
                }
                else foreach (EnemyManager enemy in enemies)
                        if (!EnemyIsAggro(enemy)) enemy.esm.Overrides.Alert();
                break;

            default: break;
        }
        idle = false;
    }

    private void CheckState()
    {
        EnemyGroupState state = State;
        foreach (EnemyManager enemy in enemies)
        {
            if (EnemyIsAggro(enemy))
            {
                Debug.Log("Aggro enemy found!!!!!!!");
                state = EnemyGroupState.Aggro;
                break;
            }
        }

        if (State == EnemyGroupState.Aggro && state == EnemyGroupState.Default)
        {
            state = EnemyGroupState.Alert;
            endTime = TimeMethods.GetWaitEndTime(alertDuration);
        }
        State = state;
    }

    private bool EnemyIsAggro(EnemyManager enemy)
    {
        return enemy.esm.currState == EnemyStateManager.StateEnum.Chase || enemy.esm.currState == EnemyStateManager.StateEnum.Attack;
    }

    public void Alert()
    {
        endTime = TimeMethods.GetWaitEndTime(alertDuration);
        State = EnemyGroupState.Alert;
    }
}
