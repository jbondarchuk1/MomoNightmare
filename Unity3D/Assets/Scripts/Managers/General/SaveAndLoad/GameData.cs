using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;


/// <summary>
/// TODO: Add a map for each 
/// </summary>
[System.Serializable]
public class GameData
{
    /// <summary>
    /// key, json data
    /// </summary>
    public Dictionary<string, string> AllGameData { get; private set; } = new Dictionary<string, string>();
    public HashSet<string> loadedKeys = new HashSet<string>();

    public void PurgeUnusedGameData()
    {
        List<string> keys = AllGameData.Keys.ToList();
        foreach (string key in keys)
        {
            if (!loadedKeys.Contains(key))
                AllGameData.Remove(key);
        }
        loadedKeys.Clear();
    }

}
