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
            return spawnedSelect.follow == target;

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
            GameObject spawnedObject = ObjectPooler.SpawnFromPool(Tag, t.position, Quaternion.identity);
            spawnedSelect = spawnedObject.GetComponent<Select>();
            spawnedSelect.follow = t;
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

    public void Select(float distanceToTarget)
    {

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
