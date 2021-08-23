using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool freeze = false;
    bool plight = false;
    bool hasLight = true;
    PlayerMovement movement;
    LookWithMouse mouseMovement;
    Light flashLight;
    RaycastObj vision;
    public Camera cam;
    GameObject rotate_Obj;

    // Start is called before the first frame update
    void Start()
    {
        movement = (PlayerMovement)gameObject.GetComponent(typeof(PlayerMovement));
        mouseMovement = (LookWithMouse)gameObject.GetComponentInChildren<Camera>().GetComponent(typeof(LookWithMouse));
        flashLight = (Light)gameObject.GetComponentInChildren<Camera>().GetComponent(typeof(Light));
        vision = (RaycastObj)gameObject.GetComponent(typeof(RaycastObj));
        cam = (Camera)gameObject.GetComponentInChildren(typeof(Camera));
        rotate_Obj = null;
    }

    public void viewObject(string name)
    {
        var canvas = (Canvas)gameObject.GetComponentInChildren(typeof(Canvas));
        if (canvas == null)
        {
            Debug.Log("no");
            return;
        }
        freeze = true;
        var prefab = Resources.Load(name + ".prefab");
        Vector3 pos = new Vector3(5, 0, 0) + cam.transform.position;
        rotate_Obj = (GameObject)Instantiate(prefab, pos, Quaternion.identity);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            vision.handleInteration();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && rotate_Obj != null)
        {
            var canvas = (Canvas)gameObject.GetComponentInChildren(typeof(Canvas));
            canvas.enabled = false;
            freeze = false;
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
