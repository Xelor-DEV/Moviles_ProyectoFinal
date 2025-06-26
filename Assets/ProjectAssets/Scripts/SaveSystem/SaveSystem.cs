using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/gameSave.sav";

    public static void SaveGame(AudioConfig audioConfig)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(savePath, FileMode.Create);

        SaveData data = new SaveData();
        data.masterVolume = audioConfig.MasterVolume;
        data.musicVolume = audioConfig.MusicVolume;
        data.sfxVolume = audioConfig.SfxVolume;

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadGame()
    {
        if (File.Exists(savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(savePath, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found in " + savePath);
            return null;
        }
    }
}

[Serializable]
public class SaveData
{
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;
}