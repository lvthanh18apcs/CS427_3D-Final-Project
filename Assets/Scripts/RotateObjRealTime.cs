using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjRealTime : MonoBehaviour
{
    bool isRotating = false;
    [SerializeField] float goal_angle;
    [SerializeField] float watch_angle;
    [SerializeField] float rotate_dis;
    float angle;
    Light lightsrc;
    AudioSource player;
    public bool revealed = false;
    void Start()
    {
        goal_angle = transform.eulerAngles.z;
        angle = transform.eulerAngles.z;
        lightsrc = gameObject.GetComponentInChildren<Light>();
        player = gameObject.GetComponent<AudioSource>();
        lightsrc.enabled = false;
    }

    public void RotateSelf(float z_angle)
    {
        goal_angle = angle + z_angle;
        rotate_dis = (goal_angle - angle) / 100;
        transform.tag = "Untagged";
        isRotating = true;
    }

    void FixedUpdate()
    {
        if (isRotating)
        {
            Vector3 newrot = transform.eulerAngles;
            angle += rotate_dis;

            //if (goal_angle > 0 && newrot.z < 0)
            //    newrot.z += 360;
            //else if (goal_angle < 0 && newrot.z > 0)
            //    newrot.z -= 360;
            //newrot.z %= 360;
            if ( (Mathf.Round(angle) >= Mathf.Round(goal_angle) && rotate_dis > 0) || ( Mathf.Round(angle) <= Mathf.Round(goal_angle) && rotate_dis<0) )
            {
                angle = Mathf.Round(goal_angle);
                isRotating = false;
                transform.tag = "RRotate";
            }
            watch_angle = angle;
            newrot.z = angle;
            transform.eulerAngles = newrot;
            if (Mathf.Round(goal_angle) % 360 == 0 || Mathf.Round(goal_angle) == 360)
                revealed = true;
            if (Mathf.Round(angle) % 360 == 0 && Mathf.Round(goal_angle) %360==0)
            {
                lightsrc.enabled = true;
                player.Play();
            }
            else
                lightsrc.enabled = false;
        }
    }
}
