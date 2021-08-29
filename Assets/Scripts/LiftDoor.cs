using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftDoor : MonoBehaviour
{
    List<GameObject> doors;
    List<int> rates;
    List<int> count;
    public float distancePerUpdate;
    int threshold;
    // Start is called before the first frame update
    void Start()
    {
        doors = new List<GameObject>();
        rates = new List<int>();
        count = new List<int>();
        threshold = (int)(40 * (40 / (50 * distancePerUpdate)));
    }

    public void upDoor(GameObject door)
    {
        bool exists = false;
        for (int i = 0; i < doors.Count; ++i)
            if (door.transform.name == doors[i].transform.name && rates[i] < 0)
            {
                rates[i] = -1;
                count[i] = threshold - count[i];
                exists = true;
                break;
            }
        if (!exists)
        {
            doors.Add(door);
            rates.Add(1);
            count.Add(1);
        }
    }

    public void downDoor(GameObject door)
    {
        bool exists = false;
        for (int i = 0; i < doors.Count; ++i)
            if (door.transform.name == doors[i].transform.name && rates[i] > 0)
            {
                rates[i] = 1;
                count[i] = threshold - count[i];
                exists = true;
                break;
            }
        if (!exists)
        {
            doors.Add(door);
            rates.Add(-1);
            count.Add(1);
        }
    }

    private void FixedUpdate()
    {
        for(int i = 0; i < doors.Count; ++i)
        {
            if (count[i] > threshold)
            {
                doors.RemoveAt(i);
                count.RemoveAt(i);
                rates.RemoveAt(i);
                --i;
            }
            else
            {
                Vector3 move = doors[i].transform.position;
                move.y += distancePerUpdate * rates[i];
                doors[i].transform.position = move;
                count[i]++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
