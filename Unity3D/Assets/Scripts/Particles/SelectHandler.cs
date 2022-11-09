using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectHandler : IPoolUser
{
    public ObjectPooler ObjectPooler { get; set; }
    public string Tag { get; set; } = "Select";
    private Select spawnedSelect;

    public SelectHandler(ObjectPooler pooler)
    {
        ObjectPooler = ObjectPooler.Instance;
    }

    public bool isSelected()
    {
        return spawnedSelect != null;
    }
    public bool isSelected(Transform target)
    {
        if (spawnedSelect != null)
            if (spawnedSelect.follow == target) return true;

        return false;
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
        if (isSelected())
        {
            spawnedSelect.selected = false;
            spawnedSelect = null;
        }
    }
}
