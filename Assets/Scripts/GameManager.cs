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
    [SerializeField] bool freeze = false;
    public bool hasLight = true;
    int cur_mode;

    PlayerMovement movement;
    LookWithMouse mouseMovement;
    RaycastObj vision;
    StoryManager story;
    public LiftDoor liftDoor;


    Light lightsrc;
    [SerializeField] Camera cam;
    [SerializeField] UnityEngine.UI.Text UIText;
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
    
    // Start is called before the first frame update
    void Start()
    {
        movement = (PlayerMovement)gameObject.GetComponent(typeof(PlayerMovement));
        mouseMovement = (LookWithMouse)gameObject.GetComponentInChildren<Camera>().GetComponent(typeof(LookWithMouse));
        vision = (RaycastObj)gameObject.GetComponent(typeof(RaycastObj));
        story = (StoryManager)gameObject.GetComponent(typeof(StoryManager));
        lightsrc = (Light)gameObject.GetComponentInChildren(typeof(Light));
        liftDoor = (LiftDoor)gameObject.GetComponent(typeof(LiftDoor));
        storyPanel.enabled = false;
        storyLine.enabled = false;
        rotate_Obj = null;
        Cursor.lockState = CursorLockMode.Locked;
        cur_mode = (int)Mode.freeMode;
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

    //max distance is 5
    public void enterView(string name, int distance = 3, string objname = "")
    {
        freeze = true;
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
        freeze = false;
        Cursor.visible = false;
        cur_mode = (int)Mode.freeMode;
        Cursor.lockState = CursorLockMode.Locked;
        cam.cullingMask = (int)RenderLayer.freeMode;

        Destroy(rotate_Obj);
        rotate_Obj = null;
    }

    public void enterStory(int storyNum)
    {
        freeze = true;
        cur_mode = (int)Mode.storyMode;
        storyPanel.enabled = true;
        storyLine.enabled = true;
        story.runStory(storyLine, storyNum);
    }
    public void exitStory()
    {
        storyPanel.enabled = false;
        storyLine.enabled = false;
        freeze = false;
        cur_mode = (int)Mode.freeMode;
    }

    public void enterMap()
    {
        float offset_x = -72f, offset_z = -120;
        float offset_convert_x = -960, offset_convert_y = -540;
        freeze = true;
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
        freeze = false;
        cur_mode = (int)Mode.freeMode;
        cam.cullingMask = (int)RenderLayer.freeMode;
    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        cur_mode = (int)Mode.pauseMode;
        freeze = true;
        cam.cullingMask = (int)RenderLayer.pauseMode;
    }
    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cur_mode = (int)Mode.freeMode;
        freeze = false;
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

    // Update is called once per frame
    void Update()
    {
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

        if (freeze)
        {
            movement.enabled = false;
            mouseMovement.enabled = false;
        }
        else
        {
            movement.enabled = true;
            mouseMovement.enabled = true;
        }
    }
}
