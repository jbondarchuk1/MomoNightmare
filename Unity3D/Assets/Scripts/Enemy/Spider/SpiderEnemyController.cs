using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEnemyController : EnemyController
{
    [SerializeField] private AudioManager audioManager;

    protected new void Update()
    {
        base.Update();
        if (agent.velocity.sqrMagnitude > .1f) audioManager.PlaySound("Crawl");
    }
}
