using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUIManager : MonoBehaviour
{
    PlayerStats stats;
    Slider healthBar;
    Slider staminaBar;
    GameObject reticle;
    [SerializeField] GameObject abilityParent;
    List<GameObject> abilities = new List<GameObject>();
    int abilityIndex = 0;
    public bool activeReticle = false;

    void Start()
    {
        for (int i = 0; i < abilityParent.transform.childCount; i++)
        {
            abilities.Add(abilityParent.transform.GetChild(i).gameObject);
        }
        stats = GameObject.Find("Player").GetComponent<PlayerStats>();
        healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
        staminaBar = GameObject.Find("StaminaBar").GetComponent<Slider>();
        reticle = transform.GetChild(2).gameObject;


        Debug.Log(" " + stats.name + " " + healthBar.name + " " + staminaBar.name + " " + reticle.name);
    }

    void Update()
    {
        healthBar.value = (float)stats.health/stats.maxHealth;
        staminaBar.value = stats.stamina / stats.maxStamina;

        reticle.SetActive(activeReticle);
    }

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
