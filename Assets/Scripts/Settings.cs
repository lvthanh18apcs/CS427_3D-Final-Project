using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Settings
{
    public int sound;
    public int sfx;
    public int mouseSensitivity;

    public Settings(int sound, int sfx, int mouseSensitivity)
    {
        this.sound = sound;
        this.sfx = sfx;
        this.mouseSensitivity = mouseSensitivity;
    }

    public Settings(int[] data)
    {
        this.sound = data[0];
        this.sfx = data[1];
        this.mouseSensitivity = data[2];
    }

    public int[] toArray()
    {
        int[] data = new int[3];
        data[0] = sound;
        data[1] = sfx;
        data[2] = mouseSensitivity;
        return data;
    }
}
