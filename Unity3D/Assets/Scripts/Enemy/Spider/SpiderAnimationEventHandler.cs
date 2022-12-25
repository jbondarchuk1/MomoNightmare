using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAnimationEventHandler : MonoBehaviour
{
    public delegate void SpiderAttack();

    public static event SpiderAttack OnSpiderAttack;

    public void InvokeAttack()
    {
        OnSpiderAttack?.Invoke();
    }
}
