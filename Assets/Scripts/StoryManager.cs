using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    GameManager manager;
    UnityEngine.UI.Text uiText;

    string story_buffer;
    int storyNum, dialog, index;
    List<int> stack = new List<int>();
    List<List<string>> story = new List<List<string>>();
    public List<bool> used = new List<bool>();
    void Awake()
    {
        manager = (GameManager)gameObject.GetComponent(typeof(GameManager));
        /*
        test.Add("This is a test story line\n" +
            "The 2nd line\n" +
            "The 3rd line\n" +
            "The 4th line\n" +
            "The 5th line The 5th line The 5th line The 5th line The 5th line The 5th line The 5th line The 5th line The 5th lineThe 5th line The 5th line\n");
        test.Add("Just a second part");
        */

        //first dialog - story begin

        //OBJECTIVE 0: GO HOME
        //at billiard room - id 0
        // OBJECTIVE 0: FIND THE PORCELAIN ROOM
        List<string> text = new List<string>();
        text.Add("Hmmm, it's 6pm already..." +
            " I should come home now or my mom would not let me in.\n");
        text.Add("Oh wait...\n" +
            "The Mjolnir arrives today. May I have a look at it...\n" +
            "It may not take much time, I can be at home before dinner.\n");
        text.Add("It is said that the Mjolnir is located inside the Porcelain room, which is the last room of this side");
        // story = new List<List<string>>();
        story.Add(text);

        //OBJECTIVE 0.1: FIND THE ELECTRICITY PANEL
        //at the door to Porcelian - id 1
        List<string> text0 = new List<string>();
        text0.Add("The Porcelain room! But the door is locked..\n");
        text0.Add("I have no idea how to open the door..\n"+
            "Electronic doors.. Hmm....\n" +
            "Maybe I can cut the electricity to see what happens.");
        story.Add(text0);

        // look at DanceCode hint - id 2
        List<string> text01 = new List<string>();
        text01.Add("Why is the Dancing Man code lying here??");
        story.Add(text01);

        // look at LetterBlock - id 3
        text01 = new List<string>();
        text01.Add("This looks interesting. Must be some kind of mechanism to unlock that force field");
        story.Add(text01);

        // pull lever down - id 4
        text01 = new List<string>();
        text01.Add("Good thing I brought my flashlight.");
        story.Add(text01);

        // after solve objective 1 and unlock the door - id 5 - temporarily not used
        List<string> text1 = new List<string>();
        text1.Add("Huh, that's too easy for a big fan of Shinichi...\n");
        story.Add(text1);

        // OBJECTIVE 1: FIND A WAY TO SHUT DOWN THE FORCE FIELD
        //in front of the hammer - id 6
        List<string> text_hammer = new List<string>();
        text_hammer.Add("What a bulky hammer!!!\n" +
            "Let see whether I'm a chosen one =)))\n");
        story.Add(text_hammer);

        //spawn at the morning room - id 7
        List<string> text_morning = new List<string>();
        text_morning.Add("OUCH!!\n" +
            "What happened? Why am I in this room?\n" +
            "My head hurts so much.. ugh\n");
        text_morning.Add("Anyway, I must go home, it seems late now...\n");
        story.Add(text_morning);

        //OBJECTIVE 2.1: FIND A FLASHLIGHT
        //OBJECTIVE 2.2: FIND THE MUSEUM MAP
        //look at either door in morning before having mapNflash - id 8
        text_morning = new List<string>();
        text_morning.Add("Hold on, why are all the doors locked??\n" +
            "This museum is so weird...\n" +
            "Alright, let's find the key quickly and come home.\n");
        text_morning.Add("Maybe there's something to unlock these doors,\n" +
            "maybe it's a rock or a book...\n");
        text_morning.Add("It's so dark. I need to find my flashlights.");
        text_morning.Add("A map should be helpful to find my ways around here");
        story.Add(text_morning);

        //Armoury_Morning && quiz1 = false
        //OBJECTIVE 3.1: EXPLORE THE ARMOURY ROOM - ID 9
        List<string> text2 = new List<string>();
        text2.Add("Hmm, let's see if I can find anything in this room...\n");
        story.Add(text2);

        //quiz1 = false
        //OBJECTIVE 3.2: FIND A WAY TO THE SMOKING ROOM - ID 10
        List<string> tmp = new List<string>();
        tmp.Add("This room is locked. It has the HEART symbol..");
        story.Add(tmp);

        //quiz2 = false
        // OBJECTIVE 3.3: FIND A WAY TO THE BILLIARD ROOM - ID 11
        List<string> text34 = new List<string>();
        text34.Add("Oh, is the door to this room locked too?\n" +
            "But it has the CLUB symbol\n" +
            "I could try finding a key with a CLUB symbol\n");
        story.Add(text34);

        // OBJECTIVE 3.4: TRY ACTIVATING THE SHIELD FORCE
        //in Billiard, try the door before lever up - id 12
        tmp = new List<string>();
        tmp.Add("I don\'t want to get zapped again\n" +
            "I need to block The  Mjolnir first. The electricity panel could help with that.");
        story.Add(tmp);

        // in the Porcelian - id 13
        List<string> text_porcelian = new List<string>();
        text_porcelian.Add("That the 1st key to unlock the Great Drawing Room.\n" +
             "Let's pick it up\n");
        story.Add(text_porcelian);

        // KEY I TO GREAT - id 14
        List<string> text_I = new List<string>();
        text_I.Add("Huh, it's quite challenge already.\n" +
            "I have to move faster or I will stay all night in this mystifying museum...\n");
        story.Add(text_I);

        //keyDin = false
        // OBJECTIVE 4.1:  FIND THE KEY TO THE DINING ROOM - id 15
        tmp = new List<string>();
        tmp.Add("This room is obviously locked, the key might lie around, somewhere..\n" +
            "If I\'m lucky, I can find it.");
        story.Add(tmp);

        //quiz4 = false
        // OBJECTIVE 4.2: FIND THE KEY TO THE SMALL DRAWING ROOM- ID 16
        tmp = new List<string>();
        tmp.Add("Is this the Diamond symbol???\n" +
            "This room looks interesting!");
        story.Add(tmp);
        
        // OBJECTIVE 4.3: FIND THE KEY TO THE SERVING ROOM - ID 17
        //quiz5 = false
        List<string> text67 = new List<string>();
        text67.Add("Aww, the Serving room is also locked...\n" +
            "Let's find the key that has the SPADE symbol.");
        story.Add(text67);

        // KEY II TO GREAT - id 18
        tmp = new List<string>();
        tmp.Add("This must be the second key to the Great Drawing Room.\n");
        story.Add(tmp);

        // IF 2 KEYS ARE ACCQUIRED - 19
        List<string> text_II = new List<string>();
        text_II.Add("The Great Drawing is the only room that I have not entered\n"+
            "It should have what I\'m looking for..\n" +
            "The passcode at the door downstairs.\n" +
            "Let\'s put an end to this trouble.");
        story.Add(text_II);

        //mechanism not trigger
        //at the door to Great Drawing - id 20
        List<string> text_great = new List<string>();
        text_great.Add("Hm, it's locked...\n" +
            "This room is very mysterious.\n" +
            "There should be a way to open the door. Hint might be close.");
        story.Add(text_great);

        //OBJECTIVE 5: FIND THE PASSCODE - id 21
        tmp = new List<string>();
        tmp.Add("Aha. Nothing can stop me now. Bring on the best puzzle!");
        story.Add(tmp);

        //OBJECTIVE 5.1: GO HOME - ID 22
        tmp = new List<string>();
        tmp.Add("Okay.. So the floor says something like..\n" +
            "YO..\n" +
            "UW..\n" +
            "IN..\n" +
            "That should be enough to get out of here.");
        tmp.Add("Better hurry up and unlock the door downstairs.\n" +
            "No point staying at this creepy museum.\n" +
            "Time for my shift has long passed.");
        story.Add(tmp);

        //win - id 23
        List<string> text10 = new List<string>();
        text10.Add("What a terrifying experience, better not to be so curious...\n");
        text10.Add("Oops, IT'S 10PM.\n" +
            "My mother will let me sleep in the garden if I don't come back ASAP and explain everything...\n");
        story.Add(text10);

        //after win - id 24
        text_morning = new List<string>();
        text_morning.Add("OUCH!!\n" +
            "What happened? Why am I in this room?\n" +
            "My head hurts so much.. ugh\n");
        text_morning.Add("JK Congrats. Game will end soon. Bye!\n");
        story.Add(text_morning);

        used = new List<bool>();
        for (int i = 0; i < story.Count; ++i)
            used.Add(false);
    }

    public bool runStory(UnityEngine.UI.Text text, int num)
    {
        if (used[num])
        {
            if (stack.Count == 0)
            {
                manager.exitStory();
                return false;
            }
            else
                return true;
        }
        uiText = text;
        if (stack.Count == 0)
        {
            dialog = 0; index = -1;
        }
        stack.Add(num);
        used[num] = true;
        return true;
    }

    public void checkObjective(int id)
    {
        if (id == 0)
        {
            manager.setUIHint("Press Esc to see objectives");
            manager.setObjective(0);
            manager.setObjective(1);
        }
        else if (id == 1)
            manager.setObjective(2);
        else if (id == 6)
            manager.setObjective(4);
        else if (id == 8)
        {
            manager.setObjective(5);
            manager.setObjective(6);
        }
        else if (id == 9 && !manager.finish1)
            manager.setObjective(7);
        else if (id == 10)
            manager.setObjective(8);
        else if (id == 11)
            manager.setObjective(9);
        else if (id == 12)
            manager.setObjective(10);
        else if (id == 15)
            manager.setObjective(11);
        else if (id == 16)
            manager.setObjective(12);
        else if (id == 17)
            manager.setObjective(13);
        else if (id == 21)
            manager.setObjective(14);
        else if (id == 22)
            manager.setObjective(0);
    }

    private void Update()
    {
        if (stack.Count!=0)
        {
            storyNum = stack[0];
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
                        checkObjective(stack[0]);
                        stack.RemoveAt(0);
                        if (stack.Count == 0)
                            manager.exitStory();
                        else
                        {
                            storyNum = stack[0]; dialog = 0; index = -1;
                        }
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
        if (stack.Count!=0)
        {
            storyNum = stack[0];
            index++;
            if (index >= story[storyNum][dialog].Length)
                return;
            story_buffer += story[storyNum][dialog][index];
        }
    }
}
