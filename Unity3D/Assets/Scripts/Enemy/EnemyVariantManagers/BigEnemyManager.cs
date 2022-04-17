using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemyManager : EnemyManager
{
    private void Start()
    {
        base.Start();
        esm.overrides.canHear = false;
    }

}
