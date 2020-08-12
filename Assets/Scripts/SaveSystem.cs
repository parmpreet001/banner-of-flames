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
        File.WriteAllText(Application.dataPath + "save.json", json);
    }
    public static SaveData LoadGame()
    {
        string json = File.ReadAllText(Application.dataPath + "save.json");
        SaveData save = new SaveData();
        JsonUtility.FromJsonOverwrite(json, save);
        return save;
    }
}
