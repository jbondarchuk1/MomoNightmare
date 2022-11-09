using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatRangeLevel;

public class EnvironmentManager : MonoBehaviour
{
    private Range playerNightmareLevel;
    [SerializeField] private List<ChangeableEnvironment> variations = new List<ChangeableEnvironment>();

    private void Update()
    {
        PlayerStats playerStats = PlayerManager.Instance.statManager;
        if (playerNightmareLevel != playerStats.NightmareRange)
        {
            playerNightmareLevel = playerStats.NightmareRange;
            int idx = ((int)playerNightmareLevel);
            foreach(ChangeableEnvironment variation in variations)
            {
                try
                {
                    variation.ChangeEnvironment(idx);
                }
                catch(Exception ex)
                {
                    Debug.LogWarning(ex);
                }
            }
        }



    }
}
