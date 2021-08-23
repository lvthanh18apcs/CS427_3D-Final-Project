using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastObj : MonoBehaviour
{
    public static string selectedObj;
    public string internalObject;
    public RaycastHit theObj;
    public GameObject obj;
    GameManager manager;

    private void Start()
    {
        manager = (GameManager)gameObject.GetComponent(typeof(GameManager));
    }

    void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out theObj, 30f))
        {
            selectedObj = theObj.transform.gameObject.name;
            internalObject = theObj.transform.gameObject.name;
            obj = theObj.transform.gameObject;
        }
        else
        {
            selectedObj = null;
            internalObject = null;
            obj = null;
        }
    }

    public void handleInteration()
    {
        if (obj != null)
        {
            //rotate object
            if (obj.transform.name == "DanceCode")
            {
                manager.viewObject("R_test");
            }
        }
    }
}
