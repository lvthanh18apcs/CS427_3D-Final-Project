using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource audio;
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        audio = gameObject.GetComponent<AudioSource>();
        Settings set = SaveManager.loadSettings();
        audio.volume = set.sound / 100;
    }

    public void onBack()
    {
        SceneManager.LoadScene(0);
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
