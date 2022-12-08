using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }   

    public GameData Load()
    {
        string path = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(path))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                loadedData = JsonConvert.DeserializeObject<GameData>(dataToLoad);
            }
            catch(Exception ex)
            {
                Debug.LogException(ex);
            }
        }
        return loadedData;
    }
    public void Save(GameData gameData)
    {
        string path = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            Debug.Log(path);
            string jsonData = JsonConvert.SerializeObject(gameData);

            using (FileStream stream = new FileStream(path,FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(jsonData);
                }
            }
        }
        catch(Exception ex)
        {
            Debug.LogError(ex);
        }
    }
}
