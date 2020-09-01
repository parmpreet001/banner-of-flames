using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveSystem
{
    public static void SaveGame(int level, List<GameObject> playerUnits)
    {
        SaveData save = new SaveData(level, playerUnits);
        string json = JsonUtility.ToJson(save);
        File.WriteAllText(Application.persistentDataPath + "save.json", json);
    }
    public static SaveData LoadGame()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "save.json");
        SaveData save = new SaveData();
        JsonUtility.FromJsonOverwrite(json, save);
        return save;
    }
}
