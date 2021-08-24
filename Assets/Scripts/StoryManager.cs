using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    GameManager manager;
    UnityEngine.UI.Text uiText;

    string story_buffer;
    int storyNum, dialog, index;
    bool write;
    List<List<string>> story;
    void Start()
    {
        manager = (GameManager)gameObject.GetComponent(typeof(GameManager));
        List<string> test = new List<string>();
        test.Add("This is a test story line\n" +
            "The 2nd line\n" +
            "The 3rd line\n" +
            "The 4th line\n" +
            "The 5th line The 5th line The 5th line The 5th line The 5th line The 5th line The 5th line The 5th line The 5th lineThe 5th line The 5th line\n");
        test.Add("Just a second part");

        story = new List<List<string>>();
        story.Add(test);
        write = false;
    }

    public void runStory(UnityEngine.UI.Text text, int num)
    {
        uiText = text;
        storyNum = num; dialog = 0; index = -1;
        write = true;
    }
    private void Update()
    {
        if (write)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //end of first dialog
                if (index >= story[storyNum][dialog].Length)
                {
                    dialog++; index = -1;
                    story_buffer = "";
                    uiText.text = "";

                    //end of story
                    if (dialog >= story[storyNum].Count)
                    {
                        write = false;
                        manager.finishStory();
                        return;
                    }
                }
                else
                {
                    //user want to skip text animation
                    index = story[storyNum][dialog].Length;
                    story_buffer = story[storyNum][dialog];
                }
            }
            uiText.text = story_buffer;
        }
    }

    private void FixedUpdate()
    {
        if (write)
        {
            index++;
            if (index >= story[storyNum][dialog].Length)
                return;
            story_buffer += story[storyNum][dialog][index];
        }
    }
}
