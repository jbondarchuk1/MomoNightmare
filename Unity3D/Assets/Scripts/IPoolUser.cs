using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolUser
{
    public abstract ObjectPooler ObjectPooler { get; set; }
    public abstract string Tag { get; set; }
}
