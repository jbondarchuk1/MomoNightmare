using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/// <summary>
/// NOTE: Override protected Load Method to upcast to inhereted members
/// </summary>
[System.Serializable]
public abstract class GameData
{
    public string fileName = ".momo";
    protected BinaryFormatter formatter = new BinaryFormatter();
    protected string basePath = Application.persistentDataPath;

    public void Save()
    {
        string path = basePath + fileName;
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream,this);
            stream.Close();
        }
    }

    /// <summary>
    /// Override load with public method
    /// </summary>
    protected GameData Load()
    {
        GameData data;
        string path = basePath + fileName;
        using (FileStream stream = new FileStream(path, FileMode.Open))
        {
            data = (GameData)formatter.Deserialize(stream);
            stream.Close();
        }
        return data;
    }
}
