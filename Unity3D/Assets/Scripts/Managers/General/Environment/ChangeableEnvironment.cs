using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChangeableEnvironment
{
    [SerializeField] protected GameObject[] EnvironmentVariations;
    protected int activeEnvironmentIdx = 0;
    public void ChangeEnvironment(int nxtIdx)
    {
        EnvironmentVariations[activeEnvironmentIdx].SetActive(false);
        activeEnvironmentIdx = nxtIdx;
        EnvironmentVariations[activeEnvironmentIdx].SetActive(true);
    }
}
