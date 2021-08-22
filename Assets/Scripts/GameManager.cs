using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool freeze = false;
    public bool hasLight = false;
    PlayerMovement movement;
    LookWithMouse mouseMovement;
    Light flashLight;

    // Start is called before the first frame update
    void Start()
    {
        movement = (PlayerMovement)gameObject.GetComponent(typeof(PlayerMovement));
        mouseMovement = (LookWithMouse)gameObject.GetComponentInChildren<Camera>().GetComponent(typeof(LookWithMouse));
        flashLight = (Light)gameObject.GetComponentInChildren<Camera>().GetComponent(typeof(Light));
    }

    // Update is called once per frame
    void Update()
    {
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

        if (hasLight)
            flashLight.enabled = true;
        else
            flashLight.enabled = false;
    }
}
