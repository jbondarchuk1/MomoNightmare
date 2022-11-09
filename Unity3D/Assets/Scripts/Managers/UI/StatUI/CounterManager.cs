using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CounterManager : MonoBehaviour
{
    [SerializeField] private AbilitiesManager abilitiesManager;
    private AbilityBase activeAbility;
    private int count = 0;
    public TextMeshProUGUI textMesh;

    void Update()
    {
        AbilityBase ability = abilitiesManager.GetActiveAbility();
        if (ability != null) activeAbility = ability;
    }
}
