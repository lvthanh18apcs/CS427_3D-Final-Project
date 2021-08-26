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
        if (tag == "Rotate" || tag == "Value" || tag == "Pickup" || tag == "Color")
            return true;
        return false;
    }

    Color nextColor(Color color)
    {
        Color ret = new Color(0, 0, 0);
        if (color == Color.white)
            ret = Color.red;
        else if (color == Color.red)
            ret = Color.green;
        else if (color == Color.green)
            ret = Color.blue;
        else if (color == Color.blue)
            ret = Color.black;
        else if (color == Color.black)
            ret = Color.white;
        return ret;
    }

    public void handleInteration()
    {
        if (obj != null)
        {
            switch (obj.transform.tag)
            {
                case "Rotate":
                    if (obj.transform.name == "DanceCode")
                        manager.enterView("R_test");
                    break;

                case "Value":
                    UnityEngine.UI.Text text = obj.GetComponentInChildren<UnityEngine.UI.Text>();
                    //Debug.Log("interacted: " + text);
                    int ch = text.text[0];
                    ch++;
                    if (ch > 90)
                        ch = 65;
                    text.text = "";
                    text.text += (char)ch;
                    break;

                case "Pickup":
                    if (obj.transform.name == "flashlight")
                        manager.hasLight = true;
                    Destroy(obj);
                    obj = null;
                    break;

                case "Color":
                    Renderer render = obj.GetComponent<Renderer>();
                    Color next = nextColor(render.material.color);
                    render.material.color = next;
                    break;

                default:
                    break;
            }
        }
    }
}
