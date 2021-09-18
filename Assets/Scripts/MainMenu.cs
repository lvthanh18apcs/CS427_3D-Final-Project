using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject loading, menu, setting, confirm;
    [SerializeField] Slider slider, soundSlider, sfxSlider, mouseSenseSlider;
    [SerializeField] Text percentage;
    [SerializeField] Button loadbtn; 
    [SerializeField] bool finish0, finish1, finish2, finish4, finish5, keyI, keyII, keyDin, mapNflash;
    [SerializeField] Vector3 PlayerPos, PlayerRot;
    AudioSource audio_src;
    void Start()
    {
        audio_src = gameObject.GetComponent<AudioSource>();
        Settings set = SaveManager.loadSettings();
        audio_src.volume = set.sound / 100;
        Checkpoints cp = SaveManager.loadCheckpoints();
        PlayerPos = new Vector3(cp.posx, cp.posy, cp.posz);
        PlayerRot = new Vector3(cp.rotx, cp.roty, cp.rotz);
        finish0 = cp.finish0;
        finish1 = cp.finish1;
        finish2 = cp.finish2;
        finish4 = cp.finish4;
        finish5 = cp.finish5;
        keyI = cp.keyI;
        keyII = cp.keyII;
        keyDin = cp.keyDin;
        mapNflash = cp.mapNflash;
        if (cp.finish0 == false)
            loadbtn.interactable = false;
        else
            loadbtn.interactable = true;
        setting.SetActive(false);
    }

    public void LoadLevel (int sceneIndex)
    {
        StartCoroutine(LoadASynchronously(sceneIndex));
    }

    public void OnSettingsOpen()
    {
        Settings set = SaveManager.loadSettings();
        soundSlider.value = set.sound;
        audio_src.volume = set.sound / 100;
        sfxSlider.value = set.sfx;
        mouseSenseSlider.value = set.mouseSensitivity;
    }

    public void OnSettingsClose()
    {
        Settings set = new Settings(soundSlider.value, sfxSlider.value, mouseSenseSlider.value);
        SaveManager.saveSettings(set);
    }

    public void OnSoundChanged(float val)
    {
        audio_src.volume = val/100;
    }

    public void UpdateCheckpoint()
    {
        //loading screen here
        Checkpoints checkpoints = SaveManager.loadCheckpoints();
        PlayerPos = new Vector3(checkpoints.posx,checkpoints.posy,checkpoints.posz);
        finish0 = checkpoints.finish0;
        finish1 = checkpoints.finish1;
        finish2 = checkpoints.finish2;
        finish4 = checkpoints.finish4;
        finish5 = checkpoints.finish5;
        keyI = checkpoints.keyI;
        keyII = checkpoints.keyII;
        keyDin = checkpoints.keyDin;
        mapNflash = checkpoints.mapNflash;
    }

    public void OnNewGame()
    {
        if (loadbtn.interactable)
        {
            confirm.SetActive(true);
        }
        else
        {
            ProceedNewGame();
        }
    }

    public void ProceedNewGame()
    {
        string path = Application.persistentDataPath + "/obj.txt";
        if (File.Exists(path))
            File.Delete(path);
        Vector3 basepos = new Vector3(44, 28, -12);
        Checkpoints checkpoints = new Checkpoints(false, false, false, false, false, false, false, false, false, basepos, new Vector3(0, 150, 0));
        SaveManager.saveCheckpoints(checkpoints);
        LoadLevel(1);
    }

    public void OnLoadGame()
    {
        Checkpoints cp = SaveManager.loadCheckpoints();
        Vector3 basepos = new Vector3(cp.posx, cp.posy, cp.posz);
        Vector3 baserot = new Vector3(cp.rotx, cp.roty, cp.rotz);
        //UpdateCheckpoint();
        //Vector3 basepos = new Vector3(16f, 28f, -67f);
        //Vector3 baserot = new Vector3(0, 0, 0);
        //Checkpoints checkpoints = new Checkpoints(finish0, finish1, finish2, finish4, finish5, keyI, keyII, keyDin, mapNflash, basepos,baserot);
        //SaveManager.saveCheckpoints(checkpoints);
        LoadLevel(1);
    }

    public void OnQuit()
    {
        Debug.Log("Quit game");
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }

    void Update()
    {
        if (finish0 == false)
            loadbtn.interactable = false;
        else
            loadbtn.interactable = true;
    }

    IEnumerator LoadASynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        loading.SetActive(true);
        menu.SetActive(false);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progress = Mathf.Round(progress);
            slider.value = progress;
            percentage.text = progress * 100f + "%";
            Debug.Log(progress);
            yield return null;
        }
    }
}
