using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorForSmall : MonoBehaviour
{
    [SerializeField] GameObject s1, s2, s3, s4, s5, s6, skey;
    [SerializeField] int changePerSec = 1;
    int rate, count;
    Renderer render;
    // Start is called before the first frame update
    void Start()
    {
        render = s1.GetComponent<Renderer>();
        render.material.SetColor("Color_9df8f5bad3a74598a492b216c2fd8909", Color.red);
        render = s2.GetComponent<Renderer>();
        render.material.SetColor("Color_9df8f5bad3a74598a492b216c2fd8909", Color.green);
        render = s3.GetComponent<Renderer>();
        render.material.SetColor("Color_9df8f5bad3a74598a492b216c2fd8909", Color.blue);
        render = s4.GetComponent<Renderer>();
        render.material.SetColor("Color_9df8f5bad3a74598a492b216c2fd8909", new Color(1,1,0));
        render = s5.GetComponent<Renderer>();
        render.material.SetColor("Color_9df8f5bad3a74598a492b216c2fd8909", new Color(1,0,1));
        render = s6.GetComponent<Renderer>();
        render.material.SetColor("Color_9df8f5bad3a74598a492b216c2fd8909", new Color(0,1,1));

        render = skey.GetComponent<Renderer>();
        rate = 50 / changePerSec;
        count = 0;
    }

    private void FixedUpdate()
    {
        ++count;
        if (count >= rate)
        {
            float red = Random.Range(0f, 1f);
            float green = Random.Range(0f, 1f);
            float blue = Random.Range(0f, 1f);
            count = 0;
            render.material.SetColor("Color_bd29e1a6dad54c0ab3af6ea7505bc4cb", new Color(red,green,blue));
        }
    }
}
