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

        /*
        objectives.Add("Find a way to cut down electricity.");
        objectives.Add("Go home. My shift ends now.");
        */

        objectives.Add("Find a way to turn off lights.\n");
        objectives.Add("Enter password to unlock the door.\n" +
            "The hint must be somewhere in this Billiard room.\n");
        objectives.Add("Unlock doors to the Armoury and Upper Vestibule.\n");
        objectives.Add("Explore the Armoury room.\n");
        objectives.Add("Find a way to go to Porcelian, the room where the hammer is located.\n");
        objectives.Add("Activate the Shield Force.\n");
        objectives.Add("Explore the Dining room.\n");
        objectives.Add("The could be something in the Serving room. Find it.\n");
        objectives.Add("Comebine the runestones and flashlight to unlock the door to Great Dawing room.\n");
        objectives.Add("Find the decipher password, somewhere in the bunch of papers on the floor.\n");
        objectives.Add("Leave the museum.\n");
        shown_objectives.Add(0);
        shown_objectives.Add(1);
    }

    public string getObjective(int id)
    {
        return objectives[id];
    }

    public void addObjective(int id)
    {
        for(int i =0;i<shown_objectives.Count;++i)
            if (id == shown_objectives[i])
            {
                Debug.Log("Already shown");
                return;
            }
        shown_objectives.Add(id);
    }

    public void deleteObjective(int id)
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
