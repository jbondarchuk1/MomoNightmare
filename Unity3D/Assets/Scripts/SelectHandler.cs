using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectHandler : MonoBehaviour, IPoolUser
{
    public ObjectPooler ObjectPooler { get; set; }
    public string Tag { get; set; } = "Select";
    protected Select spawnedSelect;
    
    protected void Start()
    {
        ObjectPooler = GameObject.Find("-- Pooler --").GetComponent<ObjectPooler>();
    }


    public void Select(Transform t)
    {
        if (spawnedSelect == null)
        {
            GameObject spawnedObject = ObjectPooler.SpawnFromPool(Tag, t.position, Quaternion.identity);
            spawnedSelect = spawnedObject.GetComponent<Select>();
        }
    }
    public void Deselect()
    {
        if (spawnedSelect != null)
        {
            spawnedSelect.selected = false;
            spawnedSelect = null;
            
        }
    }
}
