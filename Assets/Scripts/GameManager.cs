using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;

enum Mode: int
{
    freeMode = 0,
    viewMode = 1,
    storyMode = 2,
    pauseMode = 3,
    mapMode = 4,
    uniqueMode=5,
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
    public int cur_mode;

    PlayerMovement movement;
    LookWithMouse mouseMovement;
    RaycastObj vision;
    StoryManager story;
    ObjectiveManager objectiveManager;
    CharacterController physics;
    CameraEffect cameffect;
    GameObject rotatingObj;
    [System.NonSerialized] public AudioManager sound_player;
    [System.NonSerialized] public LiftDoor liftDoor;

    [System.NonSerialized] public List<string> door_name = new List<string>();
    [System.NonSerialized] public List<bool> door = new List<bool>();
    [System.NonSerialized] public List<GameObject> lifted = new List<GameObject>();

    Light lightsrc;
    [SerializeField] Camera cam;
    [SerializeField] UnityEngine.UI.Image UIhint;
    [SerializeField] UnityEngine.UI.Image UIText;
    [SerializeField] UnityEngine.UI.Image timedObjective;
    [SerializeField] UnityEngine.UI.Text hinttxt, texttxt, timedtxt;
    [SerializeField] UnityEngine.UI.Text modelText;
    [SerializeField] UnityEngine.UI.Text objName;
    [SerializeField] UnityEngine.UI.Image storyPanel;
    [SerializeField] UnityEngine.UI.Text storyLine;
    [SerializeField] UnityEngine.UI.Text UIobjective;
    [SerializeField] UnityEngine.UI.Image navigator;
    [SerializeField] UnityEngine.UI.Image loadingImg;
    [SerializeField] UnityEngine.UI.Text loadingTxt;
    [SerializeField] GameObject saveIcon;
    

    [SerializeField] UnityEngine.UI.Slider soundSlider;
    [SerializeField] UnityEngine.UI.Slider sfxSlider;
    [SerializeField] UnityEngine.UI.Slider mouseSenseSlider;
    GameObject rotate_Obj;

    //gameSetup
    [System.NonSerialized]public bool finish0 = false, finish1 = false, finish2 = false, finish4 = false, finish5 = false;
    [System.NonSerialized] public bool finishf = false, win = false;
    bool loadFromFile = false;
    [System.NonSerialized] public bool keyI = false, keyII = false, keyDin = false, mapNflash = false;
    [SerializeField] GameObject dining_key, key_I, key_II;
    [SerializeField] GameObject initLight;
    [SerializeField] GameObject quiz_setup1, quiz_setup2, quiz_setup4, quiz_setup5, fquiz_setup;
    [SerializeField] GameObject quiz1, quiz2, quiz4, quiz5;

    //quiz0
    [SerializeField] GameObject cynblock, lightning, lever, crate;

    //quiz1
    [SerializeField] GameObject cubeblock;

    //quiz2
    [SerializeField] GameObject greenkey;

    //quiz4
    [SerializeField] GameObject purplekey;
    [SerializeField] Light din_l1, din_l2;

    //quiz5
    [SerializeField] GameObject sphereblock, bluekey;

    //morning_setup
    public GameObject flholder, keyiholder, keyiiholder, morning_setup;

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
        sound_player = gameObject.GetComponentInChildren<AudioManager>();
        storyPanel.enabled = false;
        storyLine.enabled = false;
        rotate_Obj = null;
        UIhint.enabled = false; UIText.enabled = false; timedObjective.enabled = false;
        hinttxt.enabled = false; texttxt.enabled = false;timedtxt.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        cur_mode = (int)Mode.freeMode;
        lightsrc.enabled = false;

        //freeze(true);
        OnSettingsOpen();
        door_name.Add("Armoury_Smoking"); door_name.Add("Armoury_Morning"); 
        door_name.Add("Porcelain_Billiard"); door_name.Add("Billiard_Armoury");
        door_name.Add("Smoking_Great"); door_name.Add("Great_Small");
        door_name.Add("Morning_Great"); door_name.Add("Morning_Upper");
        door_name.Add("Upper_Small"); door_name.Add("Upper_Dining");
        door_name.Add("Dining_Serving"); door_name.Add("Dining_Serving (1)");
        for (int i = 0; i < door_name.Count; ++i)
            door.Add(false);

        cameffect.SimpleFadeOut();

        UpdateCheckpoint();
        if (!finish0)
            InitStart();
        else
        {
            initRestart();
            if (mapNflash)
                Unlock2Ways();
            if (keyDin)
                UnlockDining();
            if (finish1)
            {
                quiz1.SetActive(false);
                FinishQuiz1();
                changeDoorStatus(true, "", 0);
            }
            if (finish2)
            {
                quiz2.SetActive(false);
                FinishQuiz2();
                changeDoorStatus(true, "", 3);
                changeDoorStatus(true, "", 2);
            }
            if (finish4)
            {
                FinishQuiz4();
                quiz4.SetActive(false);
                changeDoorStatus(true, "", 8);
            }
            if (finish5)
            {
                FinishQuiz5();
                quiz5.SetActive(false);
                changeDoorStatus(true, "", 10);
                changeDoorStatus(true, "", 11);
            }
            if (keyI)
            {
                vision.upLever();
                lightning.SetActive(false);
                cynblock.SetActive(true);
                vision.emptyCrate();
                key_I.SetActive(false);
            }
            if (keyII)
                key_II.SetActive(false);
        }
        cameffect.SimpleFadeIn();
    }

    public void UpdateCheckpoint()
    {
        Checkpoints cp = SaveManager.loadCheckpoints();
        if (transform.position != new Vector3(cp.posx, cp.posy, cp.posz))
            loadFromFile = true;
        transform.position = new Vector3(cp.posx, cp.posy, cp.posz);
        transform.eulerAngles = new Vector3(cp.rotx, cp.roty, cp.rotz);
        finish0 = cp.finish0;
        finish1 = cp.finish1;
        finish2 = cp.finish2;
        finish4 = cp.finish4;
        finish5 = cp.finish5;
        keyI = cp.keyI;
        keyII = cp.keyII;
        keyDin = cp.keyDin;
        mapNflash = cp.mapNflash;
    }

    public void saveCheckpoint()
    {
        StartCoroutine(saveIcons());
        Checkpoints checkpoints = new Checkpoints(finish0, finish1, finish2, finish4, finish5, keyI, keyII, keyDin, mapNflash, transform.position, transform.eulerAngles);
        SaveManager.saveCheckpoints(checkpoints);
    }

    IEnumerator saveIcons()
    {
        saveIcon.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        saveIcon.SetActive(false);
    }

    public void InitStart()
    {
        morning_setup.SetActive(false);
        din_l1.enabled = false;
        din_l2.enabled = false;
        quiz_setup1.SetActive(false);
        quiz_setup2.SetActive(false);
        quiz_setup4.SetActive(false);
        quiz_setup5.SetActive(false);
        fquiz_setup.SetActive(false);
        changeDoorStatus(true, "", 3);
        changeDoorStatus(true, "", 1);
        changeDoorStatus(true, "", 7);
        lightning.SetActive(false);
        dining_key.SetActive(false);
        enterStory(0);
    }

    public void LeverDown()
    {
        unsetObjective(3);
        unsetObjective(0);
        //lever.tag = "Untagged";
        sound_player.PlaySFX("lightshut");
        sound_player.NextBG();
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
        enterStory(4);
        StartCoroutine(enterStory4());
    }

    IEnumerator enterStory4()
    {
        yield return new WaitForSecondsRealtime(3);
        setUIHint("Press F to turn on/off flashlights");
    }

    public void LeverUp()
    {
        if (finish2)
        {
            unsetObjective(10);
            lightning.SetActive(false);
            crate.tag = "Flow";
            cynblock.SetActive(true);
        }
        else
        {
            vision.downLever();
        }
    }

    public void FinishQuiz0()
    {
        if (finish0)
            return;
        finish0 = true;
        liftDoor.downDoorInstant(GameObject.Find(door_name[2]));
        cynblock.SetActive(false);
        lightning.SetActive(true);
        changeDoorStatus(false, "", 2);
        if (cur_mode == (int)Mode.mapMode)
            exitMap();
        else if (cur_mode == (int)Mode.pauseMode)
            ResumeGame();
        else if (cur_mode == (int)Mode.storyMode)
            exitStory();
        else if (cur_mode == (int)Mode.viewMode)
            exitView();
        cur_mode = (int)Mode.uniqueMode;
        StartCoroutine(LightningStrike());
    }

    public void initRestart()
    {
        cur_mode = (int)Mode.freeMode;
        unsetObjective(4); unsetObjective(3); unsetObjective(2); unsetObjective(1); unsetObjective(0);
        finish0 = true;
        morning_setup.SetActive(true);
        fquiz_setup.SetActive(false);
        vision.downLever();

        if (!loadFromFile)
        {
            transform.position = new Vector3(16f, 28f, -67f);
            Quaternion tmp = new Quaternion();
            tmp.eulerAngles = new Vector3(0, -25, 0);
            transform.rotation = tmp;
        }
        movement.enabled = true;
        hasLight = false;
        lightsrc.enabled = false;
        initLight.SetActive(false);
        if (!mapNflash)
        {
            Quaternion quar = new Quaternion();
            quar.eulerAngles = new Vector3(0, 0, 0);
            Instantiate(Resources.Load("flashlight"), new Vector3(7.5f, 27.37f, -59.47f), quar);
            Instantiate(Resources.Load("morning_map"));
        }
        if (!finish4)
        {
            din_l1.enabled = false;
            din_l2.enabled = false;
        }
        saveCheckpoint();
        dining_key.SetActive(true);
        quiz_setup1.SetActive(true);
        quiz_setup2.SetActive(true);
        quiz_setup4.SetActive(true);
        quiz_setup5.SetActive(true);
        changeDoorStatus(true, "", 2);
        if (!mapNflash)
            StartCoroutine(enterStory7());
    }

    IEnumerator enterStory7()
    {
        yield return new WaitForSecondsRealtime(5f);
        enterStory(7);
    }

    public void Unlock2Ways()
    {
        unsetObjective(5);
        unsetObjective(6);
        changeDoorStatus(true, "", 1);
        changeDoorStatus(true, "", 7);
        mapNflash = true;
        hasLight = true;
        lightsrc.enabled = true;
        saveCheckpoint();
    }

    public void UnlockDining()
    {
        unsetObjective(11);
        keyDin = true;
        dining_key.SetActive(false);
        saveCheckpoint();
        changeDoorStatus(true, "", 9);
    }

    public void SolveQuiz1()
    {
        if (finish1) return;
        cubeblock.SetActive(false);
        sound_player.PlaySFX("quiz1");
    }

    public void FinishQuiz1()
    {
        if (finish1)
            return;
        unsetObjective(7);
        unsetObjective(8);
        finish1 = true;
        saveCheckpoint();
        changeDoorStatus(true, "", 0);
    }

    public void SolveQuiz2()
    {
        if (finish2)
            return;
        sound_player.PlaySFX("quiz2");
        greenkey.transform.tag = "Pickup";
        Rigidbody rigid = greenkey.GetComponent<Rigidbody>();
        rigid.useGravity = true;
    }

    public void FinishQuiz2()
    {
        if (finish2)
            return;
        unsetObjective(9);
        finish2 = true;
        saveCheckpoint();
        changeDoorStatus(true, "", 3);
        changeDoorStatus(true, "", 2);
        if (finish5)
            UnlockKeyI();
    }

    public void SolveQuiz4()
    {
        if (finish4) return;
        sound_player.PlaySFX("quiz4");
        purplekey.transform.tag = "Pickup";
        Rigidbody rigid = purplekey.GetComponent<Rigidbody>();
        rigid.useGravity = true;
    }

    public void FinishQuiz4()
    {
        if (finish4) return;
        unsetObjective(12);
        finish4 = true;
        saveCheckpoint();
        changeDoorStatus(true, "", 8);
    }

    public void SolveQuiz5()
    {
        if (finish5) return;
        sound_player.PlaySFX("quiz5");
        sphereblock.SetActive(false);
        bluekey.GetComponent<Rigidbody>().useGravity = true;
    }

    public void FinishQuiz5()
    {
        if (finish5) return;
        unsetObjective(13);
        finish5 = true;
        saveCheckpoint();
        changeDoorStatus(true, "", 10);
        changeDoorStatus(true, "", 11);
        if (finish2)
            UnlockKeyI();
    }

    public void SolveFQuiz()
    {
        if (finishf)
            return;
        unsetObjective(14);
        finishf = true;
        sound_player.PlaySFX("fquiz");
        enterStory(22);
    }

    public void DinLightsOn()
    {
        din_l1.enabled = true;
        din_l2.enabled = true;
        sound_player.PlaySFX("lightshut");
    }

    public void UnlockKeyI()
    {
        lever.tag = "Flow";
    }

    public void takeFlashlight()
    {
        flholder.SetActive(true);
        if (keyiholder.tag == "Untagged" && keyiiholder.tag == "Untagged")
        {
            liftDoor.downDoorInstant(GameObject.Find("Morning_Great"));
        }
        hasLight = true;
        setUIHint("Press F to turn on/off flashlight");
    }

    public void placeFlashlight()
    {
        flholder.SetActive(false);
        hasLight = false;
        lightsrc.enabled = false;
        flholder.SetActive(false);
        Quaternion quar = new Quaternion();
        quar.eulerAngles = new Vector3(0, 0, 0);
        Instantiate(Resources.Load("flashlight"), new Vector3(7.5f, 27.37f, -59.47f), quar);
        if (keyiholder.tag == "Untagged" && keyiiholder.tag == "Untagged")
        {
            UnlockTheGreatDrawingRoom();
        }
    }

    public void UnlockTheGreatDrawingRoom()
    {
        fquiz_setup.SetActive(true);
        liftDoor.upDoorInstant(GameObject.Find("Morning_Great"));
        enterStory(21);
    }

    public void FinishGame()
    {
        if (win)
            return;
        win = true;
        sound_player.StopBG();
        sound_player.PlaySFX("explode");
        freeze(true);
        cameffect.SimpleFadeOut();
        StartCoroutine(FinishGameEffect());
    }

    IEnumerator FinishGameEffect()
    {
        initLight.SetActive(true);
        transform.position = new Vector3(16f, 28f, -67f);
        Quaternion tmp = new Quaternion();
        tmp.eulerAngles = new Vector3(0, -25, 0);
        transform.rotation = tmp;
        yield return new WaitForSecondsRealtime(2);
        cameffect.SimpleFadeIn();
        freeze(false);
        enterStory(24);
        yield return new WaitForSecondsRealtime(5);
        LoadWinScene();
    }

    public void LoadWinScene()
    {
        SceneManager.LoadScene(2);
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
            texttxt.enabled = true;
            texttxt.text = message;
        }
        else
        {
            UIText.enabled = false;
            texttxt.enabled = false;
        }
    }

    public void setUIHint(string hint)
    {
        StartCoroutine(ShowHint(hint));
    }

    public void setObjective(int id)
    {
        sound_player.PlaySFX("objective");
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
        vision.obj.SetActive(false);
        rotatingObj = vision.obj;
        physics.radius = 0.1f;
        freeze(true);
        Cursor.visible = true;
        cur_mode = (int)Mode.viewMode;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cam.cullingMask = (int)RenderLayer.viewMode;

        var prefab = Resources.Load(name);
        rotate_Obj = (GameObject)Instantiate(prefab,cam.transform.position + cam.transform.forward*distance , cam.transform.rotation);
        objName.text = objname;
    }
    public void exitView(bool debug = false)
    {
        if (rotatingObj != null)
            rotatingObj.SetActive(true);
        rotatingObj = null;
        physics.radius = 1;
        freeze(false);
        Cursor.visible = false;
        cur_mode = (int)Mode.freeMode;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cam.cullingMask = (int)RenderLayer.freeMode;

        Destroy(rotate_Obj);
        rotate_Obj = null;
    }

    public void enterStory(int storyNum)
    {
        //Debug.Log("enter story num " + storyNum);
        if (story.runStory(storyLine, storyNum))
        {
            freeze(true);
            cur_mode = (int)Mode.storyMode;
            storyPanel.enabled = true;
            storyLine.enabled = true;
        }
    }
    public void exitStory()
    {
        storyPanel.enabled = false;
        storyLine.enabled = false;
        freeze(false);
        cur_mode = (int)Mode.freeMode;
    }

    // while (cur_mode == (int)Mode.storyMode) {};  -   wait

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
        Cursor.visible = true;
        cur_mode = (int)Mode.pauseMode;
        freeze(true);
        cam.cullingMask = (int)RenderLayer.pauseMode;
    }
    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        Debug.Log("Back to Main Menu");
        SceneManager.LoadScene(0);
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
        //Debug.Log(transform.rotation.w);
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
        sound_player.StopBG();
        sound_player.PlaySFX("thunder");
        yield return new WaitForSecondsRealtime(6);
        sound_player.PlaySFX("strike");
        cameffect.FadeOut();
        yield return new WaitForSecondsRealtime(1);
        sound_player.PlaySFX("pitch");
        initRestart();
        yield return new WaitForSecondsRealtime(3);
        cameffect.FadeIn();
        sound_player.PlayBG();
    }

    IEnumerator ShowHint(string hint)
    {
        hinttxt.enabled = true;
        hinttxt.text = hint;
        UIhint.enabled = true;
        yield return new WaitForSeconds(2);
        UIhint.enabled = false;
        hinttxt.enabled = false;
    }

    IEnumerator ShowObjective(string objective)
    {
        timedObjective.GetComponentInChildren<UnityEngine.UI.Text>().text = objective;
        timedObjective.enabled = true;
        timedtxt.enabled = true;
        timedtxt.text = objective;
        yield return new WaitForSeconds(3);
        timedObjective.enabled = false;
        timedtxt.enabled = false;
    }
}
