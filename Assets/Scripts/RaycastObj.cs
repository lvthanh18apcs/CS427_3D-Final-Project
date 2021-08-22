using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastObj : MonoBehaviour
{
    public static string selectedObj;
    public string internalObject;
    public RaycastHit theObj;
    public GameObject obj;
    void Update()
    {
        if(Physics.Raycast(transform.position,transform.TransformDirection(Vector3.forward),out theObj))
        {
            selectedObj = theObj.transform.gameObject.name;
            internalObject = theObj.transform.gameObject.name;
            obj = theObj.transform.gameObject;
        }
    }
}
