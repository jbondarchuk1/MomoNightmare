using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupManager : MonoBehaviour
{
    private List<EnemyData> enemyGroup = new List<EnemyData>();
    public  bool alert = false;
    public  bool aggro = false;
    public  Vector3 lastSeen;
    public  float groupAggroRange = 1f;
    public  float groupSearchRange = 5f;
    public  PlayerStats playerStats;
    private float endTime = 0f;
    
    public class EnemyData
    {
        public string Name { get; set; } = "";
        public GameObject Obj { get; set; } = null;
        public EnemyStateManager stateManager { get; set; } = null;

        public EnemyData() { }

        public override string ToString()
        {
            return Name + "\n" + Obj.transform.ToString() + "\n" + "Has State Manager: " + (stateManager != null);
        }
    }

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject enemyObj = transform.GetChild(i).gameObject;
            EnemyData enemy = new EnemyData();
            enemy.Name = enemyObj.name;
            enemy.Obj = enemyObj;
            enemy.stateManager = enemyObj.GetComponentInChildren<EnemyStateManager>();
            enemyGroup.Add(enemy);
        }
    }

    void Update()
    {
        foreach (EnemyData enemy in enemyGroup)
        {
            EnemyStateManager stateManager = enemy.stateManager;
            if (stateManager == null)
                Debug.Log("No state manager available for " + enemy.Name);
            else
            {
                if (stateManager.currState == stateManager.chase && !alert)
                {
                    alert = true;
                    SetWaitTime(45);
                    AlertAll(enemy.Obj.transform.position);
                }
                else if (alert && !Waiting())
                {
                    DeescalateAlert();
                }
            }

        }
    }

    private void AlertAll(Vector3 alertedEnemy)
    {
        Debug.Log("ALERTING: Num Enemies total: " + enemyGroup.Count);
        foreach (EnemyData enemy in enemyGroup)
        {
            EnemyStateManager stateManager = enemy.stateManager;
            float distToAggro = Vector3.Distance(enemy.Obj.transform.position, alertedEnemy);
            State overrideState;
            
            if (distToAggro <= groupAggroRange)
            {
                overrideState = stateManager.chase;
            }
            else
            {
                stateManager.alert.SetPatrol();
                stateManager.patrol.SetPatrol();
                overrideState = stateManager.alert;
            }
            //Debug.Log(enemy.Name + "\t" + overrideState.name);


            stateManager.SetOverrideState(overrideState);
        }
    }

    private void DeescalateAlert()
    {
        foreach (EnemyData enemy in enemyGroup)
        {
            Debug.Log("Deescalating");
            EnemyStateManager stateManager = enemy.stateManager;
            stateManager.alert.DeescalateAlert();
            stateManager.patrol.SetPatrol();
            stateManager.SetOverrideState(stateManager.patrol);
        }
    }

    private void SetWaitTime(float seconds)
    {
        endTime = Time.time + seconds;
    }
    private bool Waiting()
    {
        if (Time.time > endTime)
        {
            endTime = 0f;
            return false;
        }
        return true;
    }
    
}
