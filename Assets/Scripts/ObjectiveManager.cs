using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Text objectiveUI;

    List<string> objectives;
    List<int> shown_objectives;
    string objective_buffer;
    void Start()
    {
        objective_buffer = "";
        objectives = new List<string>();
        shown_objectives = new List<int>();

        objectives.Add("Find a way to cut down electricity.");
        objectives.Add("Go home. My shift ends now.");

        shown_objectives.Add(0);
        shown_objectives.Add(1);
    }

    void addObjective(int id)
    {
        for(int i =0;i<shown_objectives.Count;++i)
            if (id == shown_objectives[i])
            {
                Debug.Log("Already shown");
                return;
            }
        shown_objectives.Add(id);
    }

    void deleteObjective(int id)
    {
        for(int i =0;i<shown_objectives.Count;++i)
            if (id == shown_objectives[i])
            {
                shown_objectives.RemoveAt(i);
                break;
            }
    }

    void Update()
    {
        objective_buffer = "";
        for (int i = 0; i < shown_objectives.Count; ++i)
            if (shown_objectives[i] < objectives.Count)
            {
                objective_buffer += i+1;
                objective_buffer += ". ";
                objective_buffer += objectives[shown_objectives[i]];
                objective_buffer += "\n";
            }
        
        objectiveUI.text = objective_buffer;
    }
}
