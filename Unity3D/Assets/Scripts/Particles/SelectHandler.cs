using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SelectHandler : IPoolUser
{
    public ObjectPooler ObjectPooler { get; set; }
    public string Tag { get; set; } = "Select";
    private Select spawnedSelect;
    
    public float selectDistance = 0f;

    #region Constructors
    public SelectHandler()
    {
        ObjectPooler = ObjectPooler.Instance;
    }
    public SelectHandler(string tag):this()
    {
        Tag = tag;
    }
    public SelectHandler(string tag, float selectDistance): this(tag)
    {
        this.selectDistance = selectDistance;
    }
    #endregion Constructors

    public bool isSelected()
    {
        return spawnedSelect != null;
    }
    public bool isSelected(Transform target)
    {
        if (spawnedSelect != null)
            return spawnedSelect.transform.parent == target;

        return false;
    }
    /// <summary>
    /// Unconditional select
    /// </summary>
    /// <param name="t"></param>
    public void Select(Transform t)
    {
        if (spawnedSelect == null)
        {
            Vector3 pos = t.position;
            pos.y += 1.5f;
            GameObject spawnedObject = ObjectPooler.SpawnFromPool(Tag, pos, Quaternion.identity);
            spawnedSelect = spawnedObject.GetComponent<Select>();
            spawnedSelect.Follow(t, pos);
        }
    }

    /// <summary>
    /// Conditional select within distance.
    /// </summary>
    /// <param name="t"></param>
    public void Select(Transform t, float distanceToTarget)
    {
        if (distanceToTarget <= selectDistance)
        {
            Select(t);
        }
    }


    public void Deselect()
    {
        if (isSelected())
        {
            spawnedSelect.OnObjectDie();
            spawnedSelect = null;
        }
    }
}
