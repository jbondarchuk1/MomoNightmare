using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthZone : Zone
{
    public int StealthLevel { get; set; } = 1;

    private void OnTriggerEnter(Collider foreignEntity)
    {
        if (foreignEntity.name == "Player")
        {
            PlayerManager manager = foreignEntity.gameObject.GetComponent<PlayerManager>();
            manager.HandleZone(this);

        }
    }
    private void OnTriggerExit(Collider foreignEntity)
    {
        if (foreignEntity.name == "Player")
        {
            PlayerManager manager = foreignEntity.gameObject.GetComponent<PlayerManager>();
            manager.HandleZone(null);
        }
    }
}
