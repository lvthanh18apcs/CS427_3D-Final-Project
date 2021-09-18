using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Text objectiveUI;

    List<string> objectives = new List<string>();
    List<bool> shown_objectives = new List<bool>();
    List<int> cur_objectives = new List<int>();
    string objective_buffer;

    void WriteString(List<int> tmp)
    {
        string path = Application.persistentDataPath + "/obj.txt";
        StreamWriter writer = new StreamWriter(path, false);
        string str = "";
        for(int i = 0; i < tmp.Count; ++i)
        {
            str += tmp[i].ToString();
            if (i + 1 != tmp.Count)
                str += " ";
        }
        writer.WriteLine(str);
        writer.Close();
    }
    List<int> ReadString()
    {
        string path = Application.persistentDataPath + "/obj.txt";
        if (!File.Exists(path))
            return new List<int>();
        StreamReader reader = new StreamReader(path);
        string tmp = reader.ReadToEnd();
        reader.Close();
        string[] split = tmp.Split(' ');
        List<int> ret = new List<int>();
        for (int i = 0; i < split.Length; ++i)
        {
            int dummy;
            if (int.TryParse(split[i], out dummy))
                ret.Add(dummy);
        }
        ret.Sort();
        ret = ret.Distinct().ToList();
        return ret;

    }

    void Awake()
    {
        objective_buffer = "";

        objectives.Add("Go home."); // 0
        objectives.Add("Find The Porcelain Room."); // 1
        objectives.Add("Find the electricity panel."); // 2
        objectives.Add("Pull the lever down."); // 3
        objectives.Add("Find a way to shut down the force field."); // 4
        objectives.Add("Find a flashlight."); // 5
        objectives.Add("Find the museum map."); // 6
        objectives.Add("Explore the armoury room"); // 7
        objectives.Add("Find a way to the smoking room"); // 8
        objectives.Add("Find a way to the billiard room"); // 9
        objectives.Add("Try activating the field force"); // 10
        objectives.Add("Find the key to the dining room"); // 11
        objectives.Add("Find the key to the small drawing room"); // 12
        objectives.Add("Find the key to the serving room"); // 13
        objectives.Add("Find the passcode"); // 14

        for (int i = 0; i < objectives.Count; ++i)
            shown_objectives.Add(false);
        cur_objectives = ReadString();
    }

    public string getObjective(int id)
    {
        return objectives[id];
    }

    public void addObjective(int id)
    {
        if (shown_objectives[id])
        {
            Debug.Log("Shown " + id);
            return;
        }
        for (int i = 0; i < cur_objectives.Count; ++i)
            if (id == cur_objectives[i])
                return;
        cur_objectives.Add(id);
        shown_objectives[id] = true;
        WriteString(cur_objectives);
    }

    public void deleteObjective(int id)
    {
        for(int i =0;i<cur_objectives.Count;++i)
            if (id == cur_objectives[i])
            {
                cur_objectives.RemoveAt(i);
                break;
            }
        WriteString(cur_objectives);
    }

    void Update()
    {
        objective_buffer = "";
        for (int i = 0; i < cur_objectives.Count; ++i)
        {
            objective_buffer += i + 1;
            objective_buffer += ". ";
            objective_buffer += objectives[cur_objectives[i]];
            objective_buffer += "\n";
        }
        objectiveUI.text = objective_buffer;
    }
}
