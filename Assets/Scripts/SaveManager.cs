using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager
{
    public static void saveSettings(Settings settings)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/settings.shadow";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, settings);
        stream.Close();
    }

    public static Settings loadSettings()
    {
        string path = Application.persistentDataPath + "/settings.shadow";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Settings settings = formatter.Deserialize(stream) as Settings;
            stream.Close();
            return settings;
        }
        else
        {
            Debug.Log("Save file not found at " + path + "\nLoading original settings..");
            return new Settings(80, 80, 400);
        }
    }
}
