using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CounterManager : MonoBehaviour
{
    [SerializeField] private AbilitiesManager abilitiesManager;
    private ProjectileAbility activeAbility;
    private int count = 0;
    public TextMeshProUGUI textMesh;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ProjectileAbility projectileAbility = abilitiesManager.GetActiveProjectileAbility();
        if (projectileAbility != null) activeAbility = projectileAbility;


        //bool flag = true;
        //if (activeAbility != null)
        //{
        //    if (activeAbility.ammo != Mathf.Infinity)
        //    {
        //        count = (int)activeAbility.ammo;
        //        textMesh.text = count.ToString();
        //        flag = false;
        //    }
        //}

        //if (flag) textMesh.text = "";
    }
}
