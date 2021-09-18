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

    public static void saveCheckpoints(Checkpoints checkpoints)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/checkpoints.shadow";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, checkpoints);
        stream.Close();
    }

    public static Checkpoints loadCheckpoints()
    {
        string path = Application.persistentDataPath + "/checkpoints.shadow";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Checkpoints checkpoints = formatter.Deserialize(stream) as Checkpoints;
            stream.Close();
            return checkpoints;
        }
        else
        {
            Debug.Log("Save file not found at " + path + "\nLoading new game..");
            return new Checkpoints(false, false, false, false, false, false, false, false,false, new Vector3(44, 28, -12), new Vector3(0,0,0));
        }
    }
}
