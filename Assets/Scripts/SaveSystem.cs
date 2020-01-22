using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveProgress(PlayerStats playerStats){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/data.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(playerStats);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadData(){
        string path = Application.persistentDataPath + "/data.save";
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }else{
            Debug.LogError("Save file not found.");
            return null;
        }
    }
}
