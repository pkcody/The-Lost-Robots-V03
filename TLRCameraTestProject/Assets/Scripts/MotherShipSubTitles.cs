using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MotherShipSubTitles : MonoBehaviour
{
    public static MotherShipSubTitles instance;

    public TextMeshProUGUI text;
    public GameObject chatBoxPar;

    [SerializeField] private float typingSpeed = 0.04f;
    public string response;

    Coroutine coroutine = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }



    private IEnumerator DisplayLine(string line)
    {
        text.text = " ";
        text.gameObject.SetActive(true);
        chatBoxPar.gameObject.SetActive(true);
        foreach (char letter in line.ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(5f);
        text.gameObject.SetActive(false);
        chatBoxPar.gameObject.SetActive(false);

    }

    public void TryMSSpeak(string s)
    {
        //if (coroutine != null)
        //{
        //    StopCoroutine(coroutine);

        //}
        if(chatBoxPar.activeSelf == false)
        {
            coroutine = StartCoroutine(DisplayLine(s));

        }

    }

    public void TutorialSpeak(GameObject go)
    {
        if (go.name.Contains("Walk_Platform"))
        {
            response = "Move your left joystick around to drive your robot forward. Due the limitations of wireless connection you may only be able to move so far away from each other.";
            TryMSSpeak(response);
        }
        else if (go.name.Contains("Collect_Platform"))
        {
            response = "Tap 'X' on your controller inorder to interact with objects. These are possible resources you may collect from a planet you can visit. Practice breaking and collecting the material by walking over it.";
            TryMSSpeak(response);
        }
        else if (go.name.Contains("Craft_Platform"))
        {
            response = "Roll up to the crafting platform and one robot at a time can 'interact' (X) with the crafting table. I shall have you practice crafting a 'Weak Wire'. First use your left and right arrow keys to scroll through your crafting object. Then when you find 'Weak Wire' press up on the arrow key to craft." +
                " To understand more about your inventory go to the next platform.";
            TryMSSpeak(response);
        }
        else if (go.name.Contains("Inv_Platform"))
        {
            response = "Press 'Y' for your inventory. To scroll use the bumpers. The top three slots are for robot upgrades. The next two rows are inventory space. The two paper scripts on the side are programming upgrades. First try dropping your " +
                "Weak Wire by pressing the 'Down Arrow' then pick it up again. Now try 'using' the item by hitting the 'Up Arrow'. Consuming wires heals you and friends.";
            TryMSSpeak(response);
        }
        else if (go.name.Contains("Fix_Platform"))
        {
            response = "Now let's practice repairing the ship. First 'Interact' with the broken window pieces. Then follow your guidance system via the arrows to the window frame. If you need to drop and item you are holding just press your Left Trigger.";
            TryMSSpeak(response);
        }
        else if(go.name.Contains("GoToButtons_Platform"))
        {
            response = "Each platform has helpful tips and tricks for you to use when you are out in the wild. Once you are ready head to the right side of the training room where the buttons are and do the last step to move to the front of this Mothership.";
            TryMSSpeak(response);
        }
        else if(go.name.Contains("ReadyGo_Platform"))
        {
            response = "Sometimes object will need you to interact with them in a different way like these buttons. To exit to the front of the Mothership interact with these buttons by pressing Right Trigger to Ready Up!";
            TryMSSpeak(response);
        }



    }


    public void JoinSubT()
    {
        response = "Welcome, How many of you will be exploring a new world today?";
        TryMSSpeak(response);

    }
    public void MainMenuSubT(int i)
    {
        if (i == 1)
        {
            response = "Wonderful, now, let’s begin the process to find a world to explore.";
            TryMSSpeak(response);
        }
        else if (i == 2)
        {
            response = "Go ahead and draw out the planet you wish to visit today.";
            TryMSSpeak(response);
        }

    }

    public void GameSubT(int i)
    {
        if (i == 1)
        {
            response = "Oh no there's been a malfunction. Robots hold on tight; this won't be a calculated landing.";
            TryMSSpeak(response);
        }
        else if (i == 2)
        {
            response = "*** ***** static from the towers ******* ********* ";
            TryMSSpeak(response);
        }
        else if (i == 3)
        {
            response = "This is *****. My lost robots, I need a *****, a full ****** of ****, " +
                "and a plated *******. If you can her us, please bring the items to help!";
            TryMSSpeak(response);
        }
        else if (i == 4)
        {
            response = "This is Mothership. My lost robots, I need a Window, a full canister of H2O, " +
                "and a plated quadruple battery. If you can her us, please bring the items to help!";
            TryMSSpeak(response);
        }
        else if (i == 5)
        {
            response = "All the missing parts have been accounted for. Please hurry and get in so we can leave. ";
            TryMSSpeak(response);
        }
        else if (i == 6)
        {
            response = "Thank you my little robots we couldn’t have continued our journey without you.";
            TryMSSpeak(response);
        }
    }

    public void EndSubT()
    {
        response = "Robots do a quick reset then we can look to explore the next planet.";
        TryMSSpeak(response);
    }
}
