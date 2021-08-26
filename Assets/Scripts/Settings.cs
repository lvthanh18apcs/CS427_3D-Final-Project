using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Settings
{
    public float sound;
    public float sfx;
    public float mouseSensitivity;

    public Settings(float sound, float sfx, float mouseSensitivity)
    {
        this.sound = sound;
        this.sfx = sfx;
        this.mouseSensitivity = mouseSensitivity;
    }

    public Settings(float[] data)
    {
        this.sound = data[0];
        this.sfx = data[1];
        this.mouseSensitivity = data[2];
    }

    public float[] toArray()
    {
        float[] data = new float[3];
        data[0] = sound;
        data[1] = sfx;
        data[2] = mouseSensitivity;
        return data;
    }
}
