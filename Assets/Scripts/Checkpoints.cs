using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Checkpoints
{
    public bool finish0 = false, finish1 = false, finish2 = false, finish4 = false, finish5 = false, keyI = false, keyII = false, keyDin = false, mapNflash = false;
    public float posx,posy,posz, rotx,roty,rotz;

    public Checkpoints(bool f0, bool f1, bool f2, bool f4, bool f5, bool keyI, bool keyII, bool keyDin, bool mapNflash, Vector3 pos, Vector3 rot)
    {
        finish0 = f0;
        finish1 = f1;
        finish2 = f2;
        finish4 = f4;
        finish5 = f5;
        this.keyI = keyI;
        this.keyII = keyII;
        this.keyDin = keyDin;
        this.mapNflash = mapNflash;
        posx = pos.x; posy = pos.y;posz = pos.z;
        rotx = rot.x;roty = rot.y;rotz = rot.z;
    }
}
