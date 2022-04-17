using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector]
    public PlayerStealth playerStealth; // expose to the world

    private PlayerMovement playerMovementManager;
    private PlayerStats statManager;
    private PlayerStimulus stimulusManager;
    private GameObject effects;

    private void Awake()
    {
        playerMovementManager = GetComponent<PlayerMovement>();
        statManager = GetComponent<PlayerStats>();
        stimulusManager = GetComponentInChildren<PlayerStimulus>();
        effects = GameObject.Find("Effects");
    }

    private void Start()
    {
        playerStealth = statManager.playerStealth;
    }
    private void Update()
    {
        StartCoroutine(PlayerRoutines());
    }
    private IEnumerator PlayerRoutines()
    {
        WaitForSeconds wait = new WaitForSeconds(2f);
        statManager.HandleStealthStats();
        stimulusManager.HandleStimuli();

        yield return wait;
    }

    public void HandleZone(Zone zone)
    {
        if (zone == null)
            statManager.currentZone = null;

        else if (zone.name.Contains("Stealth"))
        {
            StealthZone stealthZone = (StealthZone)zone;
            statManager.currentZone = stealthZone;
        }

    }
}
