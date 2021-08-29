using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastObj : MonoBehaviour
{
    public static string selectedObj;
    [SerializeField] string internalObject;
    [SerializeField] RaycastHit theObj;
    [SerializeField] Light lamp1, lamp2, lamp3, lamp4, lamp5;
    GameObject obj;
    GameManager manager;

    private void Start()
    {
        manager = (GameManager)gameObject.GetComponent(typeof(GameManager));
        smoke_reset();
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
        if (tag == "Rotate" || tag == "Value" || tag == "Pickup" || tag == "Color" || tag == "Switch")
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

    void smoke_reset()
    {
        lamp1.enabled = false;
        lamp2.enabled = false;
        lamp3.enabled = false;
        lamp4.enabled = false;
        lamp5.enabled = false;
    }

    public void handleInteration()
    {
        if (obj != null)
        {
            switch (obj.transform.tag)
            {
                case "Rotate":
                    if (obj.transform.name == "DanceCode")
                        manager.enterView("R_test",5);
                    else if (obj.transform.name == "armoury_letter_obj")
                        manager.enterView("armoury_letter_obj");
                    break;

                case "Value":
                    UnityEngine.UI.Text text = obj.GetComponentInChildren<UnityEngine.UI.Text>();
                    //Debug.Log("interacted: " + text);
                    int ch = text.text[0];
                    ch++;
                    if (ch > 90)
                        ch = 65;
                    if (ch < 65)
                        ch = 65;
                    text.text = "";
                    text.text += (char)ch;
                    break;

                case "Pickup":
                    if (obj.transform.name == "flashlight")
                        manager.hasLight = true;
                    else if (obj.transform.name == "DiningKey")
                        manager.enterView("DiningKey", 4, "Key to Dining Room");
                    else if (obj.transform.name == "redkey")
                        manager.enterView("redkey", 3, "Heart Key");
                    
                    Destroy(obj);
                    obj = null;
                    break;

                case "Color":
                    Renderer render = obj.GetComponent<Renderer>();
                    Color next = nextColor(render.material.color);
                    render.material.color = next;
                    break;

                case "Switch":
                    if (obj.transform.name == "smoke_switch1")
                    {
                        lamp3.enabled = !lamp3.enabled;
                    }
                    else if (obj.transform.name == "smoke_switch2")
                    {
                        lamp3.enabled = !lamp3.enabled;
                        lamp4.enabled = !lamp4.enabled;
                    }
                    else if (obj.transform.name == "smoke_switch3")
                    {
                        lamp2.enabled = !lamp2.enabled;
                        lamp4.enabled = !lamp4.enabled;
                        lamp5.enabled = !lamp5.enabled;
                    }
                    else if (obj.transform.name == "smoke_switch4")
                    {
                        lamp1.enabled = !lamp1.enabled;
                        lamp5.enabled = !lamp5.enabled;
                    }
                    else if (obj.transform.name == "smoke_switch5")
                    {
                        lamp1.enabled = !lamp1.enabled;
                        lamp4.enabled = !lamp4.enabled;
                    }
                    else if (obj.transform.name == "smoke_reset")
                    {
                        smoke_reset();
                    }
                    break;

                //case "Untagged":
                //    if (obj.transform.name == "greenkey")
                //    {
                //        obj.transform.tag = "Pickup";
                //        Rigidbody rigid = obj.GetComponent<Rigidbody>();
                //        rigid.useGravity = true;
                //    }
                //    break;

                default:
                    break;
            }
        }
    }
}
