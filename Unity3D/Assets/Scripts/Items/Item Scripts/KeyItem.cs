using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : Item
{
    private void Start()
    {
        itemSO.name = itemSO.name == "" ? "Key": itemSO.name;
    }


}
