using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lurking : MonoBehaviour
{
    //x : -5 25
    //y: : 3 13 23
    Vector3 nextpos;
    float target_y;
    [SerializeField] float rate_x, rate_y;
    void Start()
    {
        rate_x = 0.1f; rate_y = 0.05f;
        target_y = Random.Range(13f, 23f);
    }

    // Update is called once per frame
    void Update()
    {
        nextpos = transform.position + new Vector3(rate_x, rate_y);
        transform.position = nextpos;
        if (rate_y > 0 && nextpos.y >= target_y)
        {
            target_y = Random.Range(3f, 13f);
            rate_y *= -1;
        }
        else if (rate_y < 0 && nextpos.y <= target_y)
        {
            target_y = Random.Range(13f, 23f);
            rate_y *= -1;
        }
        if ((rate_x > 0 && nextpos.x >= 25) || (rate_x<0 && nextpos.x <= -5))
            rate_x *= -1;
    }
}
