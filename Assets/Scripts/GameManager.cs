using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Mode: int
{
    freeMode = 0,
    viewMode = 1,
    storyMode = 2,
    pauseMode = 3,
    mapMode = 4
}

enum RenderLayer: int
{
    freeMode = 55, //first 5 layers
    viewMode = 64, //layer 6
    pauseMode = 128, //layer 7
    mapMode = 256 //layer 8
}

public class GameManager : MonoBehaviour
{
    [System.NonSerialized] public bool hasLight = true;
    int cur_mode;

    PlayerMovement movement;
    LookWithMouse mouseMovement;
    RaycastObj vision;
    StoryManager story;
    ObjectiveManager objectiveManager;
    CharacterController physics;
    CameraEffect cameffect;
    [System.NonSerialized] public LiftDoor liftDoor;

    [System.NonSerialized] public List<string> door_name = new List<string>();
    [System.NonSerialized] public List<bool> door = new List<bool>();
    [System.NonSerialized] public List<GameObject> lifted = new List<GameObject>();

    Light lightsrc;
    [SerializeField] Camera cam;
    [SerializeField] UnityEngine.UI.Text UIhint;
    [SerializeField] UnityEngine.UI.Text UIText;
    [SerializeField] UnityEngine.UI.Text timedObjective;
    [SerializeField] UnityEngine.UI.Text modelText;
    [SerializeField] UnityEngine.UI.Text objName;
    [SerializeField] UnityEngine.UI.Image storyPanel;
    [SerializeField] UnityEngine.UI.Text storyLine;
    [SerializeField] UnityEngine.UI.Text UIobjective;
    [SerializeField] UnityEngine.UI.Image navigator;
    

    [SerializeField] UnityEngine.UI.Slider soundSlider;
    [SerializeField] UnityEngine.UI.Slider sfxSlider;
    [SerializeField] UnityEngine.UI.Slider mouseSenseSlider;
    GameObject rotate_Obj;

    //gameSetup
    bool finish0 = false, finish1 = false, finish2 = false, finish4 = false, finish5 = false;
    [System.NonSerialized] public bool keyI = false, keyII = false, keyDin = false, mapNflash = false;
    [System.NonSerialized] public bool ready_to_play = false;
    [SerializeField] GameObject dining_key, key_I, key_II;
    [SerializeField] GameObject initLight;
    [SerializeField] GameObject quiz_setup1, quiz_setup2, quiz_setup4, quiz_setup5;
    [SerializeField] GameObject quiz1, quiz2, quiz4, quiz5;

    //quiz0
    [SerializeField] GameObject cynblock, lightning, lever, crate;

    //quiz1
    [SerializeField] GameObject cubeblock;

    //quiz2
    [SerializeField] GameObject greenkey;

    //quiz4
    [SerializeField] GameObject purplekey;

    //quiz5
    [SerializeField] GameObject sphereblock, bluekey;

    

    // Start is called before the first frame update
    void Start()
    {
        movement = (PlayerMovement)gameObject.GetComponent(typeof(PlayerMovement));
        mouseMovement = (LookWithMouse)gameObject.GetComponentInChildren<Camera>().GetComponent(typeof(LookWithMouse));
        vision = (RaycastObj)gameObject.GetComponent(typeof(RaycastObj));
        story = (StoryManager)gameObject.GetComponent(typeof(StoryManager));
        objectiveManager = (ObjectiveManager)gameObject.GetComponent(typeof(ObjectiveManager));
        physics = (CharacterController)gameObject.GetComponent(typeof(CharacterController));
        lightsrc = (Light)gameObject.GetComponentInChildren(typeof(Light));
        liftDoor = (LiftDoor)gameObject.GetComponent(typeof(LiftDoor));
        cameffect = (CameraEffect)gameObject.GetComponent(typeof(CameraEffect));
        storyPanel.enabled = false;
        storyLine.enabled = false;
        rotate_Obj = null;
        UIhint.enabled = false;
        timedObjective.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        cur_mode = (int)Mode.freeMode;
        lightsrc.enabled = false;

        OnSettingsOpen();
        door_name.Add("Armoury_Smoking"); door_name.Add("Armoury_Morning"); 
        door_name.Add("Porcelain_Billiard"); door_name.Add("Billiard_Armoury");
        door_name.Add("Smoking_Great"); door_name.Add("Great_Small");
        door_name.Add("Morning_Great"); door_name.Add("Morning_Upper");
        door_name.Add("Upper_Small"); door_name.Add("Upper_Dining");
        door_name.Add("Dining_Serving"); door_name.Add("Dining_Serving (1)");
        for (int i = 0; i < door_name.Count; ++i)
            door.Add(false);

        UpdateCheckpoint();
        if (!finish0)
            InitStart();
        else
        {
            if (mapNflash)
                Unlock2Ways();
            else
                initRestart();
            if (keyDin)
                UnlockDining();
            if (finish1)
                quiz1.SetActive(false);
            if (finish2)
                quiz2.SetActive(false);
            if (finish4)
                quiz4.SetActive(false);
            if (finish5)
                quiz5.SetActive(false);
            if (keyI)
            {
                vision.emptyCrate();
                key_I.SetActive(false);
            }
            if(keyII)
                key_II.SetActive(false);
        }
        ready_to_play = true;
    }

    public void UpdateCheckpoint()
    {
        //loading screen here
        //Checkpoints checkpoints = SaveManager.loadCheckpoints();
        //transform.position = checkpoints.lastpos;
        //finish0 = checkpoints.finish0;
        //finish1 = checkpoints.finish1;
        //finish2 = checkpoints.finish2;
        //finish4 = checkpoints.finish4;
        //finish5 = checkpoints.finish5;
        //keyI = checkpoints.keyI;
        //keyII = checkpoints.keyII;
        //keyDin = checkpoints.keyDin;
        //mapNflash = checkpoints.mapNflash;
    }

    public void saveCheckpoint()
    {
        //Checkpoints checkpoints = new Checkpoints(finish0, finish1, finish2, finish4, finish5, keyI, keyII, keyDin, mapNflash, transform.position);
        //SaveManager.saveCheckpoints(checkpoints);
    }

    public void InitStart()
    {
        quiz_setup1.SetActive(false);
        quiz_setup2.SetActive(false);
        quiz_setup4.SetActive(false);
        quiz_setup5.SetActive(false);
        changeDoorStatus(true, "", 3);
        changeDoorStatus(true, "", 1);
        changeDoorStatus(true, "", 7);
        lightning.SetActive(false);
        dining_key.SetActive(false);
    }

    public void LeverDown()
    {
        lever.tag = "Untagged";
        //initLight will be designed later
        initLight.SetActive(false);
        for (int i = 0; i < lifted.Count; ++i)
        {
            liftDoor.downDoorInstant(GameObject.Find(lifted[i].name));
            changeDoorStatus(false, lifted[i].name, -1);
        }

        changeDoorStatus(false, "", 3);
        changeDoorStatus(false, "", 1);
        changeDoorStatus(false, "", 7);
        changeDoorStatus(true, "", 2);
        //playStory
        lightsrc.enabled = true;
    }

    public void LeverUp()
    {
        if (finish1 && finish2 && finish4 && finish5)
        {
            crate.tag = "Switch";
            cynblock.SetActive(true);
        }
        else
        {
            vision.downLever();
            ShowHint("Do not have enough keys");
        }
    }

    public void FinishQuiz0()
    {
        if (!finish0)
        {
            finish0 = true;
            liftDoor.downDoorInstant(GameObject.Find(door_name[2]));
            cynblock.SetActive(false);
            lightning.SetActive(true);
            changeDoorStatus(false, "", 2);
            StartCoroutine(LightningStrike());
        }
    }

    public void initRestart()
    {
        Destroy(lightning);
        vision.downLever();
        transform.position = new Vector3(16f, 28f, -67f);
        Quaternion tmp = new Quaternion();
        tmp.eulerAngles = new Vector3(0, -25, 0);
        transform.rotation = tmp;
        movement.enabled = true;
        hasLight = false;
        lightsrc.enabled = false;
        initLight.SetActive(false);
        Instantiate(Resources.Load("flashlight"));
        Instantiate(Resources.Load("morning_map"));
        saveCheckpoint();
        dining_key.SetActive(true);
        quiz_setup1.SetActive(true);
        quiz_setup2.SetActive(true);
        quiz_setup4.SetActive(true);
        quiz_setup5.SetActive(true);
        changeDoorStatus(true, "", 2);
    }

    public void Unlock2Ways()
    {
        changeDoorStatus(true, "", 1);
        changeDoorStatus(true, "", 7);
        mapNflash = true;
        saveCheckpoint();
    }

    public void UnlockDining()
    {
        keyDin = true;
        saveCheckpoint();
        changeDoorStatus(true, "", 9);
    }

    //finish Armoury quiz
    public void SolveQuiz1()
    {
        cubeblock.SetActive(false);
    }

    //when the key is picked up
    public void FinishQuiz1()
    {
        finish1 = true;
        saveCheckpoint();
        changeDoorStatus(true, "", 0);
    }

    public void SolveQuiz2()
    {
        greenkey.transform.tag = "Pickup";
        Rigidbody rigid = greenkey.GetComponent<Rigidbody>();
        rigid.useGravity = true;
    }

    public void FinishQuiz2()
    {
        finish2 = true;
        saveCheckpoint();
        changeDoorStatus(true, "", 3);
        changeDoorStatus(true, "", 2);
        if (finish5)
            UnlockKeyI();
    }

    public void SolveQuiz4()
    {
        purplekey.transform.tag = "Pickup";
        Rigidbody rigid = purplekey.GetComponent<Rigidbody>();
        rigid.useGravity = true;
    }

    public void FinishQuiz4()
    {
        finish4 = true;
        saveCheckpoint();
        changeDoorStatus(true, "", 8);
    }

    public void SolveQuiz5()
    {
        sphereblock.SetActive(false);
        bluekey.GetComponent<Rigidbody>().useGravity = true;
    }

    public void FinishQuiz5()
    {
        finish5 = true;
        saveCheckpoint();
        changeDoorStatus(true, "", 10);
        changeDoorStatus(true, "", 11);
        if (finish2)
            UnlockKeyI();
    }

    public void UnlockKeyI()
    {
        crate.tag = "Dialog";
        lever.tag = "Switch";
        //object to go and find the crate
    }

    public void UnlockTheGreatDrawingRoom()
    {
        // :)
    }

    void changeDoorStatus(bool status, string name = "", int id = -1)
    {
        int index = id;
        for (int i = 0; i < door_name.Count; ++i)
            if (door_name[i] == name)
                index = i;
        if (index == -1)
            return;
        door[index] = status;
    }

    public bool checkDoorStatus(string name = "", int id = -1)
    {
        int index = id;
        for (int i = 0; i < door_name.Count; ++i)
            if (door_name[i] == name)
                index = i;
        if (index == -1)
            return false;
        return door[index];
    }

    public void setUIText(bool enable, string message = "Click to interact")
    {
        if (enable)
        {
            UIText.enabled = true;
            UIText.text = message;
        }
        else
            UIText.enabled = false;
    }

    public void setUIHint(string hint)
    {
        StartCoroutine(ShowHint(hint));
    }

    public void setObjective(int id)
    {
        objectiveManager.addObjective(id);
        StartCoroutine(ShowObjective(objectiveManager.getObjective(id)));
    }

    public void unsetObjective(int id)
    {
        objectiveManager.deleteObjective(id);
    }

    //max distance is 5
    public void enterView(string name, float distance = 1.8f, string objname = "")
    {
        physics.radius = 0.1f;
        freeze(true);
        Cursor.visible = true;
        cur_mode = (int)Mode.viewMode;
        Cursor.lockState = CursorLockMode.None;
        cam.cullingMask = (int)RenderLayer.viewMode;

        var prefab = Resources.Load(name);
        rotate_Obj = (GameObject)Instantiate(prefab,cam.transform.position + cam.transform.forward*distance , cam.transform.rotation);
        objName.text = objname;
    }
    public void exitView()
    {
        physics.radius = 1;
        freeze(false);
        Cursor.visible = false;
        cur_mode = (int)Mode.freeMode;
        Cursor.lockState = CursorLockMode.Locked;
        cam.cullingMask = (int)RenderLayer.freeMode;

        Destroy(rotate_Obj);
        rotate_Obj = null;
    }

    public void enterStory(int storyNum)
    {
        freeze(true);
        cur_mode = (int)Mode.storyMode;
        storyPanel.enabled = true;
        storyLine.enabled = true;
        story.runStory(storyLine, storyNum);
    }
    public void exitStory()
    {
        storyPanel.enabled = false;
        storyLine.enabled = false;
        freeze(false);
        cur_mode = (int)Mode.freeMode;
    }

    public void enterMap()
    {
        float offset_x = -72f, offset_z = -120;
        float offset_convert_x = -960, offset_convert_y = -540;
        freeze(true);
        cur_mode = (int)Mode.mapMode;
        cam.cullingMask = (int)RenderLayer.mapMode;
        float x = transform.position.x, z = transform.position.z;
        x -= offset_x; x /= (71+72); x = 1 - x;
        z -= offset_z; z /= (118 + 120); 
        float convert_x = z * 1920 + offset_convert_x, convert_y = x * 1080 + offset_convert_y;
        Vector3 newpos = new Vector3(convert_x, convert_y, 0);
        navigator.rectTransform.anchoredPosition = newpos;
        navigator.rectTransform.localEulerAngles = new Vector3(0, 0, -transform.eulerAngles.y-90);
    }

    public void exitMap()
    {
        freeze(false);
        cur_mode = (int)Mode.freeMode;
        cam.cullingMask = (int)RenderLayer.freeMode;
    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        cur_mode = (int)Mode.pauseMode;
        freeze(true);
        cam.cullingMask = (int)RenderLayer.pauseMode;
    }
    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cur_mode = (int)Mode.freeMode;
        freeze(false);
        cam.cullingMask = (int)RenderLayer.freeMode;
    }

    public void OnResumeClick()
    {
        ResumeGame();
    }
    public void OnSettingsOpen()
    {
        Settings set = SaveManager.loadSettings();
        soundSlider.value = set.sound;
        sfxSlider.value = set.sfx;
        mouseSenseSlider.value = set.mouseSensitivity;
    }
    public void OnSettingsClose()
    {
        Settings set = new Settings(soundSlider.value, sfxSlider.value, mouseSenseSlider.value);
        SaveManager.saveSettings(set);
    }
    public void OnQuitClick()
    {
        Debug.Log("Quit");
//#if UNITY_EDITOR
//        // Application.Quit() does not work in the editor so
//        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
//        UnityEditor.EditorApplication.isPlaying = false;
//#else
//        Application.Quit();
//#endif
    }

    void freeze(bool yes)
    {
        movement.enabled = !yes;
        mouseMovement.enabled = !yes;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(transform.rotation.w);
        //if (Input.GetKeyDown(KeyCode.Escape))
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (cur_mode == (int)Mode.freeMode)
            {
                PauseGame();
            }
            else if (cur_mode == (int)Mode.pauseMode)
            {
                ResumeGame();
            }
            else if (cur_mode == (int)Mode.viewMode)
            {
                exitView();
            }
            else if (cur_mode == (int)Mode.mapMode)
            {
                exitMap();
            }
        }
        else if (Input.GetMouseButtonDown(0) && cur_mode == (int)Mode.freeMode)
        {
            vision.handleInteration();
        }
        else if (Input.GetKeyDown(KeyCode.F2) && cur_mode == (int)Mode.freeMode)
        {
            enterStory(0);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            if (cur_mode == (int)Mode.freeMode)
                enterMap();
            else if (cur_mode == (int)Mode.mapMode)
                exitMap();
        }

        if (Input.GetKeyDown(KeyCode.F) && hasLight)
        {
            lightsrc.enabled = !lightsrc.enabled;
        }
    }

    IEnumerator LightningStrike()
    {
        yield return new WaitForSecondsRealtime(7);
        //some camera animation
        initRestart();
        cameffect.FadeOut();
        yield return new WaitForSecondsRealtime(2);
        cameffect.FadeIn();
    }

    IEnumerator ShowHint(string hint)
    {
        UIhint.text = hint;
        UIhint.enabled = true;
        yield return new WaitForSeconds(2);
        UIhint.enabled = false;
    }

    IEnumerator ShowObjective(string objective)
    {
        timedObjective.text = objective;
        timedObjective.enabled = true;
        yield return new WaitForSeconds(3);
        timedObjective.enabled = false;
    }
}
