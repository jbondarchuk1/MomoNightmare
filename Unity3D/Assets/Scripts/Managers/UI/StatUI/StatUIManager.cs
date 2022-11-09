using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static AbilityUI;
using static AbilitiesManager;
using static StatRangeLevel;

public class StatUIManager : MonoBehaviour
{
    private NightmareUIManager nightmareUIManager;
    private StealthUIManager stealthUIManager;
    private AbilityUIManager abilityUIManager;

    #region Private
        private PlayerStats stats;
        private Slider healthBar;
        private Slider staminaBar;
        private GameObject reticle;
    #endregion Private
    public bool ActiveReticle { private get; set; } = false;

    #region Monobehaviour Methods
    void Start()
    {
        stats = PlayerManager.Instance.statManager;

        // standard UI
        healthBar = transform.Find("HealthBar").GetComponent<Slider>();
        staminaBar = transform.Find("StaminaBar").GetComponent<Slider>();
        reticle = transform.Find("Reticle").gameObject;

        // rangeUI
        nightmareUIManager = GetComponentInChildren<NightmareUIManager>();
        stealthUIManager = GetComponentInChildren<StealthUIManager>();

        // other UI
        abilityUIManager = GetComponentInChildren<AbilityUIManager>();

    }
    void Update()
    {
        StartCoroutine(HandleStatUI());
    }
    private IEnumerator HandleStatUI()
    {
        WaitForSeconds wait = new WaitForSeconds(.2f);
        HandleBaseStatUI();
        HandleStealthUI();
        HandleNightmareUI();
        reticle.SetActive(ActiveReticle);
        yield return wait;
    }
    #endregion Monobehaviour Methods

    #region Dependent on User Input
    public void SetActiveAbility(Abilities ability)
    {
        abilityUIManager.SetActiveAbility(ability);
    }
    #endregion Dependent on User Input

    #region Regular UI Handlers
    private void HandleBaseStatUI()
    {
        healthBar.value = (float)stats.health / stats.maxHealth;
        staminaBar.value = stats.stamina / stats.maxStamina;
    }
    private void HandleStealthUI()
    {
        stealthUIManager.SetUI(stats.StealthRange);
    }
    private void HandleNightmareUI()
    {
        nightmareUIManager.SetUI(stats.NightmareRange);
    }

    #endregion Regular UI Handlers

}
