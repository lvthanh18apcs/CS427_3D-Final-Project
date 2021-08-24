using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool freeze = false;
    bool plight = false;
    bool hasLight = true;
    bool viewMode = false;
    PlayerMovement movement;
    LookWithMouse mouseMovement;
    Light flashLight;
    RaycastObj vision;
    Camera cam;
    public UnityEngine.UI.Image background;
    public UnityEngine.UI.Text UIText;
    public UnityEngine.UI.Text modelText;
    GameObject rotate_Obj;
    LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        movement = (PlayerMovement)gameObject.GetComponent(typeof(PlayerMovement));
        mouseMovement = (LookWithMouse)gameObject.GetComponentInChildren<Camera>().GetComponent(typeof(LookWithMouse));
        flashLight = (Light)gameObject.GetComponentInChildren<Camera>().GetComponent(typeof(Light));
        vision = (RaycastObj)gameObject.GetComponent(typeof(RaycastObj));
        cam = (Camera)gameObject.GetComponentInChildren(typeof(Camera));
        rotate_Obj = null;
        mask = cam.cullingMask;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void viewObject(string name)
    {
        freeze = true;
        Cursor.visible = true;
        viewMode = true;
        Cursor.lockState = CursorLockMode.None;
        cam.cullingMask = ~mask;

        var prefab = Resources.Load(name);
        rotate_Obj = (GameObject)Instantiate(prefab,cam.transform.position + cam.transform.forward*3 , cam.transform.rotation);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !viewMode)
        {
            vision.handleInteration();
        }
        else if (Input.GetKeyDown(KeyCode.F1) && viewMode)
        {
            freeze = false;
            Cursor.visible = false;
            viewMode = false;
            Cursor.lockState = CursorLockMode.Locked;
            cam.cullingMask = mask;

            Destroy(rotate_Obj);
            rotate_Obj = null;
        }
        if (Input.GetKeyDown(KeyCode.F) && hasLight)
        {
            if (plight)
                plight = false;
            else
                plight = true;
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

        if (plight)
            flashLight.enabled = true;
        else
            flashLight.enabled = false;
    }
}
