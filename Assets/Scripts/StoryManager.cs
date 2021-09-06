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
        /*
        test.Add("This is a test story line\n" +
            "The 2nd line\n" +
            "The 3rd line\n" +
            "The 4th line\n" +
            "The 5th line The 5th line The 5th line The 5th line The 5th line The 5th line The 5th line The 5th line The 5th lineThe 5th line The 5th line\n");
        test.Add("Just a second part");
        */

        //first dialog - story begin


        //at billiard room
        test.Add("Hmmm, it's 6pm already...\n" +
            "I should come home now or my mom would not let me in.\n");
        test.Add("Oh wait,\n" +
            "The Mjolnir arrives today. May I have a look at it...\n" +
            "It's may not take time, and I can be at home before dinner.\n");
        test.Add("It is said that the tree Mjolnir will glow in the dark,\n" +
            "maybe turning off all the lights would make it easier to find.\n");

        // OBJECTIVE 0: Find a way to cut down electricity
        //at the door to Porcelian
        test.Add("There is light from Porcelian room, I should take a look...\n");
        test.Add("Oh the Mjolnir is right there. But the door is locked.\n" +
            "There must be a key around here...\n");

        // OBJECTIVE 1: Find the quiz, solve it to enter the Porcelian room.
        //after solve objective 1 and unlock the door 
        test.Add("Huh, that's too easy for a big fan of Shinichi...\n");

        //in front of the hammer
        test.Add("What a bulky hammer!!!\n" +
            "Let see whether I'm a chosen one =)))\n");

        //spawn at the morning room
        test.Add("OUCH!!\n" +
            "What happened? Why am I in this room?\n");
        test.Add("Anyway, I must go home, it's too late now...\n");
        test.Add("Hold on, why are all the doors locked??\n" +
            "This museum is so weird...\n" +
            "Alright, let's find the key quickly and come home.\n");
        test.Add("Maybe there's something to unlock these doors,\n" +
            "maybe it's a rock or a book...\n");

        // OBJECTIVE 2: pick up the runestone to unlock doors to Armoury & Upper Vestibule
        // go to the Armoury
        test.Add("Hmm, let's see if I can find anything in this room...\n");

        // OBJECTIVE 3: Explore the Armoury
        // OBJECTIVE 4: Find way to go to Porcelian 
        // in the Armoury, at the door to Billiard
        test.Add("Oh, is the door to this room locked too?\n" +
            "maybe the key is in the Smoking room...\n");

        // in the Porcelian
        test.Add("Another stone here.\n" +
             "Let's pick it up, it may be helpful later.\n");
        // OBJECTIVE 5: Try activating the Shield Force
        // after picking up the runestone
        test.Add("Huh, it's quite challenge already.\n" +
            "I have to move faster or I will stay all night in this mysifying museum...\n");
        test.Add("Let's walk downstair to see if I can find anything helpful.\n");

        // OBJECTIVE 6:  Explore the Dining
        // OBJECTIVE 7: Find the stone in the Serving
        // in the dining room
        test.Add("Aww, the Serving room is also locked...\n" +
            "Let's find the key.\n");
        test.Add("But this room looks pretty empty,\n" +
            "maybe there is some hint in the Small Drawing room...\n");

        // after find out the 2nd runestone
        test.Add("Well, I've been through most of this museum and still can't find a way out...\n");
        test.Add("Wait, there's still a big room I haven't entered yet.\n" +
            "Let's try it out...\n");

        //at the door to Great Drawing
        test.Add("Hm, it's locked...\n" +
            "Maybe the things I picked up earlier will help now.\n");

        // OBJECTIVE 8: Combine the objects
        // in the Great Drawing
        test.Add("OK, this is the last room, the key must be here.\n");

        // OBJECTIVE 9: Find the decipher password, somewhere in the bunch of papers on the floor.
        //after find out the password
        test.Add("Great, finally found the key to get out of this place.\n" +
            "Quickly get out of here.\n");

        // OBJECTIVE 10: walk downstair and leave the museum
        //after leaving the museum
        test.Add("What a terrifying experience, I will not enter this place anymore...\n");
        test.Add("Oops, IT'S 10PM.\n" +
            "My mother will let me sleep in the garden if I don't come back ASAP and explain everything...\n");

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
                        manager.exitStory();
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
