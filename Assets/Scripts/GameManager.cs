using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Mode: int
{
    freeMode = 0,
    viewMode = 1,
    storyMode = 2
}

public class GameManager : MonoBehaviour
{
    [SerializeField] bool freeze = false;
    public bool hasLight = true;
    int cur_mode;

    PlayerMovement movement;
    LookWithMouse mouseMovement;
    Light lightsrc;
    RaycastObj vision;
    Camera cam;
    [SerializeField] UnityEngine.UI.Image background;
    [SerializeField] UnityEngine.UI.Text UIText;
    [SerializeField] UnityEngine.UI.Text modelText;
    [SerializeField] UnityEngine.UI.Image storyPanel;
    UnityEngine.UI.Text storyLine;
    GameObject rotate_Obj;
    LayerMask mask;
    StoryManager story;

    // Start is called before the first frame update
    void Start()
    {
        movement = (PlayerMovement)gameObject.GetComponent(typeof(PlayerMovement));
        mouseMovement = (LookWithMouse)gameObject.GetComponentInChildren<Camera>().GetComponent(typeof(LookWithMouse));
        lightsrc = (Light)gameObject.GetComponentInChildren<Camera>().GetComponent(typeof(Light));
        vision = (RaycastObj)gameObject.GetComponent(typeof(RaycastObj));
        cam = (Camera)gameObject.GetComponentInChildren(typeof(Camera));
        story = (StoryManager)gameObject.GetComponent(typeof(StoryManager));
        storyLine = storyPanel.GetComponentInChildren<UnityEngine.UI.Text>();
        storyPanel.enabled = false;
        storyLine.enabled = false;
        rotate_Obj = null;
        mask = cam.cullingMask;
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

    public void viewObject(string name)
    {
        freeze = true;
        Cursor.visible = true;
        cur_mode = (int)Mode.viewMode;
        Cursor.lockState = CursorLockMode.None;
        cam.cullingMask = ~mask;

        var prefab = Resources.Load(name);
        rotate_Obj = (GameObject)Instantiate(prefab,cam.transform.position + cam.transform.forward*3 , cam.transform.rotation);
    }

    public void finishStory()
    {
        storyPanel.enabled = false;
        storyLine.enabled = false;
        freeze = false;
        cur_mode = (int)Mode.freeMode;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && cur_mode == (int)Mode.freeMode)
        {
            vision.handleInteration();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && cur_mode == (int)Mode.viewMode)
        {
            freeze = false;
            Cursor.visible = false;
            cur_mode = (int)Mode.freeMode;
            Cursor.lockState = CursorLockMode.Locked;
            cam.cullingMask = mask;

            Destroy(rotate_Obj);
            rotate_Obj = null;
        }
        else if (Input.GetKeyDown(KeyCode.F1) && cur_mode == (int)Mode.freeMode)
        {
            freeze = true;
            cur_mode = (int)Mode.storyMode;
            storyPanel.enabled = true;
            storyLine.enabled = true;
            story.runStory(storyLine, 0);
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
