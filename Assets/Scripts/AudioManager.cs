using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public Sound[] sfx;

    //public static AudioManager instance;
    AudioSource sound_player;
    bool playbg = true;
    private void Awake()
    {
        //if (instance == null)
        //    instance = this;
        //else
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        //DontDestroyOnLoad(gameObject);

        Settings set = SaveManager.loadSettings();

        sound_player = gameObject.AddComponent<AudioSource>();
        sound_player.volume = set.sound / 100;

        foreach (Sound s in sfx)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume * (set.sfx / 100);
            s.source.loop = s.loop;
        }
    }

    public void updateSoundVolume(float val)
    {
        sound_player.volume = val;
    }

    public void updateSFXVolume(float val)
    {
        for (int i = 0; i < sfx.Length; ++i)
        {
            sfx[i].source.volume = sfx[i].volume * (val / 100);
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = null;
        for (int i = 0; i < sfx.Length; ++i)
            if (sfx[i].name == name)
            {
                s = sfx[i];
                break;
            }
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
        }
        else
            s.source.Play();
    }

    public void PauseSFX(string name)
    {
        Sound s = null;
        for (int i = 0; i < sfx.Length; ++i)
            if (sfx[i].name == name)
            {
                s = sfx[i];
                break;
            }
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
        }
        s.source.Pause();
    }

    public void ResumeSFX(string name)
    {
        Sound s = null;
        for (int i = 0; i < sfx.Length; ++i)
            if (sfx[i].name == name)
            {
                s = sfx[i];
                break;
            }
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
        }
        s.source.UnPause();
    }

    public void PauseBG()
    {
        sound_player.Pause();
    }

    public void ResumeBG()
    {
        sound_player.UnPause();
    }

    public void StopBG()
    {
        sound_player.Stop();
        playbg = false;
    }

    public void NextBG()
    {
        sound_player.Stop();
    }

    public void PlayBG()
    {
        playbg = true;
    }

    private void Update()
    {
        if (sound_player != null && !sound_player.isPlaying && playbg)
        {
            int r = Random.Range(0, 15);
            sound_player.clip = sounds[r].clip;
            sound_player.Play();
        }
    }
}
