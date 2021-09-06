using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraEffect : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] UnityEngine.UI.Image filter;
    [SerializeField] Volume volume;
    DepthOfField depth;
    Color color;
    bool fadein = false, fadeout = false;
    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGet<DepthOfField>(out depth);
    }

    public void FadeOut()
    {
        fadeout = true;
    }

    public void FadeIn()
    {
        fadein = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (fadeout)
        {
            color = filter.color;
            if (color.a < 1)
            {
                color.a += 0.05f;
                filter.color = color;
            }
            else
            {
                fadeout = false;
                depth.focalLength.value = 20f;
            }
        }
        else if (fadein)
        {
            color = filter.color;
            if (color.a > 0)
            {
                color.a -= 0.02f;
                filter.color = color;
            }
            if (color.a <= 0 && depth.focalLength.value > 1)
                depth.focalLength.value -= 0.1f;
            if (depth.focalLength.value == 1)
                fadein = false;
        }
    }
}
