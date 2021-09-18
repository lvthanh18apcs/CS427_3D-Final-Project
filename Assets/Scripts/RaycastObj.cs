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

    //fquiz
    [SerializeField] GameObject rot1, rot2, rot3;

    //maindoor
    [SerializeField] UnityEngine.UI.Text w1, w2, w3, w4, w5, w6;

    //debug
    [SerializeField] GameObject crate;

    Animator anim;
    Renderer render;
    public GameObject obj;
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

            RaycastDialog();
            if(manager.cur_mode == (int)Mode.freeMode)
                manager.setUIText(validateInteration(obj.transform.tag));
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
        if (tag == "Rotate" || tag == "Value" || tag == "Pickup" || tag == "Color" || tag == "Switch" || tag == "Door" || tag == "Flow" || tag == "RRotate")
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
        if (pT1.text + pT2.text + pT3.text + pT4.text + pT5.text + pT6.text + pT7.text == "PHANTOM")
            return true;
        return false;
    }

    bool checkQuiz1()
    {
        if (getColor(cube1) == Color.red && getColor(cube2) == Color.green && getColor(cube3) == Color.red
            && getColor(cube4) == Color.white && getColor(cube5) == Color.white && getColor(cube6) == Color.blue
            && getColor(cube7) == Color.black && getColor(cube8) == Color.red && getColor(cube9) == Color.green)
            return true;
        return false;
    }

    bool checkQuiz2()
    {
        if (lamp1.enabled && lamp2.enabled && lamp3.enabled && lamp4.enabled && lamp5.enabled)
            return true;
        return false;
    }

    bool checkQuiz4()
    {
        if (checkLanternStatus(lantern1) && checkSmokeStatus(lantern2) && checkLanternStatus(lantern3)
            && checkLanternStatus(lantern4) && !checkSmokeStatus(lantern5) && !checkLanternStatus(lantern6)
            && !checkLanternStatus(lantern7) && checkSmokeStatus(lantern8) && !checkLanternStatus(lantern9))
            return true;
        return false;
    }

    bool checkQuiz5()
    {
        if (sph1.text + sph2.text + sph3.text + sph4.text + sph5.text + sph6.text == "SPIHME")
            return true;
        return false;
    }

    bool checkFQuiz()
    {
        RotateObjRealTime check3 = rot3.GetComponent<RotateObjRealTime>();
        RotateObjRealTime check2 = rot2.GetComponent<RotateObjRealTime>();
        RotateObjRealTime check1 = rot1.GetComponent<RotateObjRealTime>();
        if (check1.revealed && check2.revealed && check3.revealed)
            return true;
        return false;
    }

    bool checkMainDoor()
    {
        if (w1.text + w2.text + w3.text + w4.text + w5.text + w6.text == "YOUWIN")
            return true;
        return false;
    }

    public void emptyCrate()
    {
        anim = greifer.GetComponent<Animator>();
        anim.SetBool("isOpen", true);
        anim = deckel.GetComponent<Animator>();
        anim.SetBool("isOpen", true);
        crate.tag = "Untagged";
        crate.GetComponent<BoxCollider>().enabled = false;
    }

    public void downLever()
    {
        Animator anim = lever.GetComponent<Animator>();
        anim.SetBool("isUp", true);
    }

    public void upLever()
    {
        Animator anim = lever.GetComponent<Animator>();
        anim.SetBool("isUp", false);
    }

    public void RaycastDialog()
    {
        if (obj == null)
            return;
        string name = obj.name;
        switch (name)
        {
            case "Porcelain_Billiard":
                if (!manager.finish0)
                {
                    manager.enterStory(1);
                    manager.unsetObjective(1);
                }
                else if (manager.finish0 && manager.mapNflash)
                {
                    Animator anim = lever.GetComponent<Animator>();
                    if (anim.GetBool("isUp"))
                        manager.enterStory(12);
                }
                break;
            case "DanceCode":
                if (!manager.finish0)
                    manager.enterStory(2);
                break;
            case "RotateMask":
                if (!manager.finish0)
                    manager.enterStory(6);
                break;
            case "Armoury_Morning":
                if (!manager.mapNflash && manager.finish0)
                    manager.enterStory(8);
                else if (manager.mapNflash && !manager.finish1 && manager.finish0)
                    manager.enterStory(9);
                break;
            case "Armoury_Smoking":
                if (!manager.finish1 && manager.finish0)
                    manager.enterStory(10);
                break;
            case "Billiard_Armoury":
                if (!manager.finish2 && manager.finish0)
                    manager.enterStory(11);
                break;
            case "Morning_Upper":
                if (!manager.mapNflash && manager.finish0)
                    manager.enterStory(8);
                break;

            case "Upper_Dining":
                if (!manager.keyDin && manager.finish0)
                    manager.enterStory(15);
                break;
            case "Upper_Small":
                if (!manager.finish4 && manager.finish0)
                    manager.enterStory(16);
                break;
            case "Dinning_Serving":
                if (!manager.finish5 && manager.finish0)
                    manager.enterStory(17);
                break;
            case "Dining_Serving (1)":
                if (!manager.finish5 && manager.finish0)
                    manager.enterStory(17);
                break;
            case "Morning_Great":
                if (!manager.keyI && !manager.keyII && manager.finish0)
                    manager.enterStory(20);
                break;

            case "GreatKey1":
                manager.enterStory(14);
                manager.enterStory(13);
                if (manager.keyII)
                    manager.enterStory(19);
                break;

            case "GreatKey2":
                manager.enterStory(18);
                if (manager.keyI)
                    manager.enterStory(19);
                break;

            case "Lever":
                if (!manager.finish0)
                {
                    manager.unsetObjective(2);
                    manager.setObjective(3);
                }
                break;
            
        }
        if (name.Contains("Letter"))
            manager.enterStory(3);
        else if (name.Contains("win") && manager.finishf)
            manager.enterStory(23);
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
                        manager.enterView(name, 2.3f);
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
                    else if (name == "morning_hint")
                        manager.enterView(name,1.5f);
                    else if (name == "mecha")
                        manager.enterView(name,1.5f);
                    break;

                case "Value":
                    int r = Random.Range(0, 2);
                    if (r == 0)
                        manager.sound_player.PlaySFX("value1");
                    else if (r == 1)
                        manager.sound_player.PlaySFX("value2");
                    else if (r == 2)
                        manager.sound_player.PlaySFX("value3");
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
                    else if (name.Contains("win"))
                    {
                        if (checkMainDoor())
                        {
                            if (manager.finishf)
                                manager.FinishGame();
                            else
                                manager.LoadWinScene();
                        }
                    }
                    break;

                case "Pickup":
                    if (!name.Contains("key"))
                        manager.sound_player.PlaySFX("pickup");
                    if (name == "flashlight(Clone)")
                    {
                        manager.hasLight = true;
                        manager.setUIHint("Press F to turn on flashlight");
                        manager.takeFlashlight();
                        if (GameObject.Find("morning_map(Clone)") == null)
                            manager.Unlock2Ways();
                        //new objective find map here
                    }
                    else if (name == "morning_map(Clone)")
                    {
                        manager.setUIHint("Press M to view Museum Map");
                        if (manager.hasLight)
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
                    }
                    else if (name == "GreatKey2")
                    {
                        manager.enterView("DiningKey", 3.5f, "Key II to The Great Drawing Room");
                        manager.keyII = true;
                        manager.saveCheckpoint();
                    }

                    Destroy(obj);
                    obj = null;
                    break;

                case "Color":
                    Renderer render = obj.GetComponent<Renderer>();
                    Color next = nextColor(render.material.color);
                    render.material.color = next;
                    if (name.Contains("Cube"))
                    {
                        switch (name)
                        {
                            case "Cube":
                                manager.sound_player.PlaySFX("note4");
                                break;
                            case "Cube1":
                                manager.sound_player.PlaySFX("note1");
                                break;
                            case "Cube2":
                                manager.sound_player.PlaySFX("note9");
                                break;
                            case "Cube3":
                                manager.sound_player.PlaySFX("note5");
                                break;
                            case "Cube4":
                                manager.sound_player.PlaySFX("note3");
                                break;
                            case "Cube5":
                                manager.sound_player.PlaySFX("note2");
                                break;
                            case "Cube6":
                                manager.sound_player.PlaySFX("note8");
                                break;
                            case "Cube7":
                                manager.sound_player.PlaySFX("note7");
                                break;
                            case "Cube8":
                                manager.sound_player.PlaySFX("note6");
                                break;
                        }
                        if (checkQuiz1())
                            manager.SolveQuiz1();
                    }
                    break;

                case "Switch":
                    manager.sound_player.PlaySFX("switch");
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
                        if (name == "Upper_Dining")
                            manager.DinLightsOn();
                        if (name == "Porcelain_Billiard" && manager.finish0)
                        {
                            anim = lever.GetComponent<Animator>();
                            if (anim.GetBool("isUp"))
                            {
                                //play story here - dont want to get zapped again hehe :)
                                return;
                            }    
                        }
                        manager.sound_player.PlaySFX("door");
                        manager.liftDoor.upDoor(obj);
                        manager.lifted.Add(obj);
                    }
                    else
                        manager.setUIHint("Need some kind of key!");
                    break;
                
                case "RRotate":
                    if (checkFQuiz())
                        manager.SolveFQuiz();
                    manager.sound_player.PlaySFX("rrotate");
                    if (name == "RotBlock1")
                    {
                        RotateZBlock(rot1, -60);
                        RotateZBlock(rot2, -45);
                        RotateZBlock(rot3, 15);
                    }
                    else if (name == "RotBlock2")
                    {
                        RotateZBlock(rot2, 60);
                        RotateZBlock(rot3, 45);
                    }
                    else if (name == "RotBlock3")
                    {
                        RotateZBlock(rot3, 90);
                        RotateZBlock(rot1, -30);
                    }
                    if (checkFQuiz())
                        manager.SolveFQuiz();
                    else
                    {
                        if (checkFQuiz())
                            manager.SolveFQuiz();
                    } 
                        
                    break;

                case "Flow":
                    if (name == "Lever")
                    {
                        anim = lever.GetComponent<Animator>();
                        if (!anim.GetBool("isUp"))
                        {
                            if (manager.finish0)
                            {
                                manager.setUIHint("Shouldn't mess with it again");
                                return;
                            }
                            manager.LeverDown();
                        }
                        else
                        {
                            if (!manager.finish0)
                            {
                                manager.setUIHint("Does not have enough power");
                                return;
                            }
                            manager.LeverUp();
                        }
                        anim.SetBool("isUp", !anim.GetBool("isUp"));
                        manager.sound_player.PlaySFX("lever");
                    }

                    if (name == "basic_crate")
                    {
                        emptyCrate();
                    }

                    if (name == "keyIHolder")
                    {
                        if (manager.keyI)
                        {
                            Quaternion quar = new Quaternion();
                            quar.eulerAngles = new Vector3(90, 0, 0);
                            Instantiate(Resources.Load("GreatKey"), new Vector3(8.242f, 27.14f, -60.5f), quar);
                            obj.tag = "Untagged";
                            if (manager.keyiiholder.tag == "Untagged" && !manager.flholder.activeSelf)
                                manager.UnlockTheGreatDrawingRoom();
                        }
                        else
                            manager.setUIHint("Key I not obtained");
                    }
                    else if (name == "keyIIHolder")
                    {
                        if (manager.keyII)
                        {
                            Quaternion quar = new Quaternion();
                            quar.eulerAngles = new Vector3(90, 0, 0);
                            Instantiate(Resources.Load("GreatKey"), new Vector3(8.828f, 27.14f, -60.5f), quar);
                            obj.tag = "Untagged";
                            if (manager.keyiholder.tag == "Untagged" && !manager.flholder.activeSelf)
                                manager.UnlockTheGreatDrawingRoom();
                        }
                        else
                            manager.setUIHint("Key II not obtained");
                    }
                    else if (name == "flashlightholder")
                    {
                        manager.placeFlashlight();
                    }
                    break;

                default:
                    break;
            }
        }
    }

    void RotateZBlock(GameObject rotobj, float z_angle)
    {
        RotateObjRealTime rotate = rotobj.GetComponent<RotateObjRealTime>();
        rotate.RotateSelf(z_angle);
    }
}
