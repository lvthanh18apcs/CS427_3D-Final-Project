using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastObj : MonoBehaviour
{
    public static string selectedObj;
    [SerializeField] string internalObject;
    [SerializeField] RaycastHit theObj;
    GameObject obj;
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

            if (validateInteration(obj.transform.tag))
                manager.setUIText(true);
        }
        else
        {
            selectedObj = null;
            internalObject = null;
            obj = null;
            manager.setUIText(false, "");
        }
    }

    bool validateInteration(string tag)
    {
        if (tag == "Rotate" || tag == "Value" || tag == "Pickup")
            return true;
        return false;
    }

    public void handleInteration()
    {
        if (obj != null)
        {
            //Debug.Log("Interacted");
            if (obj.transform.tag == "Rotate")
            {
                if (obj.transform.name == "DanceCode")
                    manager.enterView("R_test");
            }
            else if (obj.transform.tag == "Value")
            {
                UnityEngine.UI.Text text = obj.GetComponentInChildren<UnityEngine.UI.Text>();
                //Debug.Log("interacted: " + text);
                int ch = text.text[0];
                ch++;
                if (ch > 90)
                    ch = 65;
                text.text = "";
                text.text += (char)ch;
            }
            else if (obj.transform.tag == "Pickup")
            {
                if (obj.transform.name == "flashlight")
                    manager.hasLight = true;
                Destroy(obj);
                obj = null;
            }
        }
    }
}
