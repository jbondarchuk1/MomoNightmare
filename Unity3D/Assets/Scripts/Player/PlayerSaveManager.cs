using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class PlayerSaveManager : GeneralData
{
    #region Data Fields
    [System.Serializable]
    private class PlayerData
    {
        public float[] PlayerPos;
        public float[] PlayerRot;
        public int playerHealth;
    }

    PlayerData playerData;
    #endregion Data Fields
    PlayerManager playerManager;


    private void load(PlayerData data)
    {
        Vector3 pos = data.PlayerPos.ToVector3();
        Vector3 rot = data.PlayerRot.ToVector3();

        PlayerManager.Instance.TeleportTo(pos,rot);
        playerManager.statManager.health = data.playerHealth;

    }
    private void save()
    {
        if (playerData == null) playerData = new PlayerData();
        playerData.PlayerPos = playerManager.transform.position.ToFloatArray();
        playerData.PlayerRot = playerManager.transform.rotation.eulerAngles.ToFloatArray();
        playerData.playerHealth = playerManager.statManager.health;
    }
    private void Start()
    {
        playerManager = PlayerManager.Instance;
    }

    /// <summary>
    /// Serialize to a json string on save.
    /// </summary>
    /// <returns></returns>
    protected override string Serialize()
    {
        save();
        return JsonConvert.SerializeObject(playerData);
    }

    /// <summary>
    /// Essentially a load method to load our player
    /// </summary>
    /// <param name="json"></param>
    protected override void Deserialize(string json)
    {
        Debug.Log("loading player...");
        PlayerData loadData = JsonConvert.DeserializeObject<PlayerData>(json);
        load(loadData);
    }


}
