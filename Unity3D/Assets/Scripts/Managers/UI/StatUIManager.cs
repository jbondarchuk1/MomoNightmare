using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUIManager : MonoBehaviour
{
    #region Private
    private PlayerStats stats;
    private Slider healthBar;
    private Slider staminaBar;
    private GameObject reticle;
    [SerializeField] private GameObject abilityParent;
    private List<GameObject> abilities = new List<GameObject>();
    private int abilityIndex = 0;
    #endregion Private
    public bool ActiveReticle { private get; set; } = false;

    void Start()
    {
        for (int i = 0; i < abilityParent.transform.childCount; i++)
            abilities.Add(abilityParent.transform.GetChild(i).gameObject);
        
        stats = GameObject.Find("Player").GetComponent<PlayerStats>();
        healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
        staminaBar = GameObject.Find("StaminaBar").GetComponent<Slider>();
        reticle = transform.GetChild(2).gameObject;
    }

    void Update()
    {
        healthBar.value = (float)stats.health/stats.maxHealth;
        staminaBar.value = stats.stamina / stats.maxStamina;

        reticle.SetActive(ActiveReticle);
    }


    // TODO: Refactor UI in Unity
    // Make UI appear all in one spot, not a long line.
    public void SetActiveAbility(int index)
    {
        ToggleAbility(abilities[abilityIndex]);
        abilityIndex = index;
        ToggleAbility(abilities[abilityIndex]);
    }

    private void ToggleAbility(GameObject abilityUI)
    {
        GameObject on  = abilityUI.transform.GetChild(0).gameObject;
        GameObject off = abilityUI.transform.GetChild(1).gameObject;

        on.SetActive(!on.activeSelf);
        off.SetActive(!on.activeSelf);
    }


}
