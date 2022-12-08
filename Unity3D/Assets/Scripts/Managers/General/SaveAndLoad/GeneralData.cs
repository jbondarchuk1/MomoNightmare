using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class GeneralData: MonoBehaviour
{
    /// <summary>
    /// WARNING: do not name objects the same in the editor, this will overwrite the key
    /// </summary>
    public string Key { get; private set; } = "";
    private void Awake()
    {
        if (Key.Length == 0) Key = CreateKey();
    }
    /// <summary>
    /// Each Inherited class needs to cast to its own class and manually set the data within the object to
    /// the saved data.
    /// </summary>
    /// <param name="data"></param>
    protected abstract void Deserialize(string json);
    protected abstract string Serialize();
    protected string CreateKey()
    {
        return GetType().ToString() + gameObject.name;
    }
    public void LoadData(GameData gameData)
    {
        if (gameData.AllGameData.ContainsKey(this.Key))
        {
            string data = gameData.AllGameData[this.Key];
            Deserialize(data);
        }
    }
    public void SaveData(ref GameData gameData)
    {
        if (gameData.AllGameData.ContainsKey(Key))
            gameData.AllGameData[Key] = Serialize();
        else
            gameData.AllGameData.Add(Key, Serialize());
    }
}
