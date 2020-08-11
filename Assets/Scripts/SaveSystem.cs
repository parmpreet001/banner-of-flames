using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveData(SaveData save)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.bin";
        FileStream fileStream = new FileStream(path, FileMode.Create);

        SaveData saveData = new SaveData(save);

        formatter.Serialize(fileStream, saveData);
        fileStream.Close();
    }

    public static SaveData LoadData(SaveData save)
    {
        string path = Application.persistentDataPath + "/save.bin";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);

            SaveData saveData = formatter.Deserialize(fileStream) as SaveData;
            fileStream.Close();
            return saveData;
        }
        else
        {
            Debug.LogError("Save not found");
            return null;
        }
    }
}
