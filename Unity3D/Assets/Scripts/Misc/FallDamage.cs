 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FallDamage
{
    [SerializeField] private float fallDamageStartHeight = 1f;
    [SerializeField] private float fallDamageInstaKillHeight = 10f;

    [SerializeField][Range(0, 10)] private int fallDamageMultiplier = 1;

    private float startY = 0f;

    public void StartFall(float startY)
    {
        this.startY = startY;
    }

    /// <summary>
    /// End fall will return the damage calculated by the sub component
    /// </summary>
    /// <param name="endY"></param>
    /// <returns></returns>
    public int EndFall(float endY)
    {
        float distance = startY - endY;
        Reset();
        if (distance > fallDamageStartHeight)
        {
            if (distance >= fallDamageInstaKillHeight) return int.MaxValue;
            else return GetDamage(distance);
        }
        return 0;
    }
    private int GetDamage(float distance)
    {
        return (int) (distance * fallDamageMultiplier);
    }
    public void Reset()
    {
        startY = 0f;
    }
}
