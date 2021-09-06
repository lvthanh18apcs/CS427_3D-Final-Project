using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class RaycastObj : MonoBehaviour
{
    public static string selectedObj;
    [SerializeField] string internalObject;
    [SerializeField] RaycastHit theObj;

    //quiz2
    [SerializeField] Light lamp1, lamp2, lamp3, lamp4, lamp5;

    //quiz4
    [SerializeField] GameObject lantern1, lantern3, lantern4, lantern6, lantern7, lantern9;
    [SerializeField] VisualEffect lantern2, lantern5, lantern8;

    //quiz5
    [SerializeField] UnityEngine.UI.Text sph1, sph2, sph3, sph4, sph5, sph6;

    [SerializeField] GameObject lever, deckel, greifer;

    //quiz0
    [SerializeField] UnityEngine.UI.Text pT1, pT2, pT3, pT4, pT5, pT6, pT7;

    //quiz1
    [SerializeField] GameObject cube1, cube2, cube3, cube4, cube5, cube6, cube7, cube8, cube9;

    Animator anim;
    Renderer render;
    GameObject obj;
    GameManager manager;

    private void Start()
    {
        manager = (GameManager)gameObject.GetComponent(typeof(GameManager));
        smoke_reset();
    }

    void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out theObj, 10f))
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
        if (tag == "Rotate" || tag == "Value" || tag == "Pickup" || tag == "Color" || tag == "Switch" || tag == "Door")
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

    Color switchColor(Color color)
    {
        if (color == Color.white)
            return Color.red;
        else if (color == Color.red)
            return Color.white;
        else if (color.g != 0)
            return new Vector4(color.r,0,0, 1);
        else if (color.g == 0)
            return new Vector4(color.r, color.r, color.r, 1);
        return Color.black;
    }

    bool checkLanternStatus(GameObject lantern)
    {
        render = lantern.GetComponent<Renderer>();
        if (render.material.GetColor("Color_b2012523a2b74c3bba113ea76d523753") == Color.red)
            return true;
        else
            return false;
    }

    bool checkSmokeStatus(VisualEffect smoke)
    {
        Color now = (Color)smoke.GetVector4("Color");
        if (now.g == 0)
            return true;
        else
            return false;
    }

    void changeLanternStatus(GameObject lantern)
    {
        render = lantern.GetComponent<Renderer>();
        Color now = render.material.GetColor("Color_b2012523a2b74c3bba113ea76d523753");
        render.material.SetColor("Color_b2012523a2b74c3bba113ea76d523753", switchColor(now));
    }

    void changeSmokeStatus(VisualEffect smoke)
    {
        Color now = (Color)smoke.GetVector4("Color");
        smoke.SetVector4("Color", switchColor(now));
    }

    void resetLanternStatus(GameObject lantern)
    {
        render = lantern.GetComponent<Renderer>();
        render.material.SetColor("Color_b2012523a2b74c3bba113ea76d523753", Color.white);
    }

    void resetSmokeStatus(VisualEffect smoke)
    {
        Color now = (Color)smoke.GetVector4("Color");
        smoke.SetVector4("Color", new Color(now.r,now.r,now.r,1));
    }

    Color getColor(GameObject obj_col)
    {
        Renderer render = obj_col.GetComponent<Renderer>();
        return render.material.color;
    }

    bool checkQuiz0()
    {
        //if (pT1.text+pT2.text+pT3.text+pT4.text+pT5.text+pT6.text+pT7.text == "PHANTOM")
        return true;
    }

    bool checkQuiz1()
    {
        //if (getColor(cube1) == Color.red && getColor(cube2) == Color.green && getColor(cube3) == Color.red
        //    && getColor(cube4) == Color.grey && getColor(cube5) == Color.grey && getColor(cube6) == Color.blue
        //    && getColor(cube7) == Color.black && getColor(cube8) == Color.red && getColor(cube9) == Color.green)
        return true;
    }

    bool checkQuiz2()
    {
        //if (lamp1.enabled && lamp2.enabled && lamp3.enabled && lamp4.enabled && lamp5.enabled)
        return true;
    }

    bool checkQuiz4()
    {
        //if (checkLanternStatus(lantern1) && checkSmokeStatus(lantern2) && checkLanternStatus(lantern3)
        //    && checkLanternStatus(lantern4) && !checkSmokeStatus(lantern5) && !checkLanternStatus(lantern6)
        //    && !checkLanternStatus(lantern7) && checkSmokeStatus(lantern8) && checkLanternStatus(lantern9))
        return true;
    }

    bool checkQuiz5()
    {
        //if (sph1.text + sph2.text + sph3.text + sph4.text + sph5.text + sph6.text == "SPIHME")
        return true;
    }

    public void emptyCrate()
    {
        anim = greifer.GetComponent<Animator>();
        anim.SetBool("isOpen", true);
        anim = deckel.GetComponent<Animator>();
        anim.SetBool("isOpen", true);
        obj.tag = "Untagged";
        obj.GetComponent<BoxCollider>().enabled = false;
    }

    public void downLever()
    {
        Animator anim = lever.GetComponent<Animator>();
        anim.SetBool("isUp", true);
    }

    public void handleInteration()
    {
        if (obj != null)
        {
            string name = obj.transform.name;
            switch (obj.transform.tag)
            {
                case "Rotate":
                    if (name == "DanceCode")
                        manager.enterView(name,2);
                    else if (name == "armoury_letter_obj")
                        manager.enterView(name);
                    else if (name == "smoking_letter_obj")
                        manager.enterView(name);
                    else if (name == "small_letter1")
                        manager.enterView(name);
                    else if (name == "small_letter2")
                        manager.enterView(name);
                    else if (name == "dining_letter")
                        manager.enterView(name);
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
                    if (name.Contains("Letter"))
                    {
                        if (checkQuiz0())
                            manager.FinishQuiz0();
                    }
                    else if (name.Contains("Sphere"))
                    {
                        if (checkQuiz5())
                            manager.SolveQuiz5();
                    }    
                    break;

                case "Pickup":
                    if (name == "flashlight(Clone)")
                    {
                        manager.hasLight = true;
                        manager.setUIHint("Press F to turn on flashlight");
                        //new objective find map here
                    }
                    else if (name == "morning_map(Clone)")
                    {
                        manager.setUIHint("Press M to view Museum Map");
                        manager.Unlock2Ways();
                    }
                    else if (name == "DiningKey")
                    {
                        manager.enterView(name, 3.5f, "Key to Dining Room");
                        manager.UnlockDining();
                    }
                    else if (name == "redkey")
                    {
                        manager.enterView(name, 2.5f, "Heart Key");
                        manager.FinishQuiz1();
                    }
                    else if (name == "greenkey")
                    {
                        manager.enterView(name, 2.5f, "Club key");
                        manager.FinishQuiz2();
                    }
                    else if (name == "purplekey")
                    {
                        manager.enterView(name, 2.5f, "Diamond Key");
                        manager.FinishQuiz4();
                    }
                    else if (name == "bluekey")
                    {
                        manager.enterView(name, 2.5f, "Spade Key");
                        manager.FinishQuiz5();
                    }
                    else if (name == "GreatKey1")
                    {
                        manager.enterView("DiningKey", 3.5f, "Key I to The Great Drawing Room");
                        manager.keyI = true;
                        manager.saveCheckpoint();
                        if (manager.keyII)
                            manager.UnlockTheGreatDrawingRoom();
                    }
                    else if (name == "GreatKey2")
                    {
                        manager.enterView("DiningKey", 3.5f, "Key II to The Great Drawing Room");
                        manager.keyII = true;
                        manager.saveCheckpoint();
                        if (manager.keyI)
                            manager.UnlockTheGreatDrawingRoom();
                    }

                    obj.SetActive(false);
                    obj = null;
                    break;

                case "Color":
                    Renderer render = obj.GetComponent<Renderer>();
                    Color next = nextColor(render.material.color);
                    render.material.color = next;
                    if (name.Contains("Cube"))
                    {
                        if (checkQuiz1())
                            manager.SolveQuiz1();
                    }
                    break;

                case "Switch":
                    if (name == "Lever")
                    {
                        anim = lever.GetComponent<Animator>();
                        if (!anim.GetBool("isUp"))
                            manager.LeverDown();
                        else
                            manager.LeverUp();
                        anim.SetBool("isUp", !anim.GetBool("isUp"));
                    }

                    if (name == "basic_crate")
                    {
                        emptyCrate();
                    }
                    

                    if (name == "smoke_switch1")
                    {
                        lamp3.enabled = !lamp3.enabled;
                    }
                    else if (name == "smoke_switch2")
                    {
                        lamp3.enabled = !lamp3.enabled;
                        lamp4.enabled = !lamp4.enabled;
                    }
                    else if (name == "smoke_switch3")
                    {
                        lamp2.enabled = !lamp2.enabled;
                        lamp4.enabled = !lamp4.enabled;
                        lamp5.enabled = !lamp5.enabled;
                    }
                    else if (name == "smoke_switch4")
                    {
                        lamp1.enabled = !lamp1.enabled;
                        lamp5.enabled = !lamp5.enabled;
                    }
                    else if (name == "smoke_switch5")
                    {
                        lamp1.enabled = !lamp1.enabled;
                        lamp4.enabled = !lamp4.enabled;
                    }
                    else if (name == "smoke_reset")
                    {
                        smoke_reset();
                    }

                    if (name == "din_switch1")
                    {
                        changeLanternStatus(lantern7);
                        changeSmokeStatus(lantern8);
                        changeLanternStatus(lantern9);
                    }
                    else if (name == "din_switch2")
                    {
                        changeLanternStatus(lantern4);
                        changeSmokeStatus(lantern5);
                        changeLanternStatus(lantern6);
                    }
                    else if (name == "din_switch3")
                    {
                        changeLanternStatus(lantern1);
                        changeSmokeStatus(lantern2);
                        changeLanternStatus(lantern3);
                    }
                    else if (name == "din_switch4")
                    {
                        changeLanternStatus(lantern1);
                        changeLanternStatus(lantern4);
                        changeLanternStatus(lantern7);
                    }
                    else if (name == "din_switch5")
                    {
                        changeSmokeStatus(lantern2);
                        changeSmokeStatus(lantern5);
                        changeSmokeStatus(lantern8);
                    }
                    else if (name == "din_switch6")
                    {
                        changeLanternStatus(lantern3);
                        changeLanternStatus(lantern6);
                        changeLanternStatus(lantern9);
                    }
                    else if (name == "din_switch7")
                    {
                        changeLanternStatus(lantern3);
                        changeSmokeStatus(lantern5);
                        changeLanternStatus(lantern7);
                    }
                    else if (name == "din_reset")
                    {
                        resetLanternStatus(lantern1);
                        resetLanternStatus(lantern3);
                        resetLanternStatus(lantern4);
                        resetLanternStatus(lantern6);
                        resetLanternStatus(lantern7);
                        resetLanternStatus(lantern9);
                        resetSmokeStatus(lantern2);
                        resetSmokeStatus(lantern5);
                        resetSmokeStatus(lantern8);
                    }

                    if (name.Contains("smoke"))
                    {
                        if (checkQuiz2())
                            manager.SolveQuiz2();
                    }
                    else if (name.Contains("din"))
                    {
                        if (checkQuiz4())
                            manager.SolveQuiz4();
                    }
                    break;

                case "Door":
                    if (manager.checkDoorStatus(name, -1))
                    {
                        manager.liftDoor.upDoor(obj);
                        manager.lifted.Add(obj);
                    }
                    else
                        manager.setUIHint("Need some kind of key!");
                    break;

                default:
                    break;
            }
        }
    }
}
