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
    bool fadein = false, fadeout = false, simplefadeout = false, simplefadein = false;
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

    public void SimpleFadeOut()
    {
        simplefadeout = true;
    }

    public void SimpleFadeIn()
    {
        simplefadein = true;
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
        else if (simplefadeout)
        {
            filter.color = Color.white;
            simplefadeout = false;
        }
        else if (simplefadein)
        {
            color = filter.color;
            if (color.a > 0)
            {
                color.a -= 0.1f;
                filter.color = color;
            }
            else
                simplefadein = false;
        }
    }
}
