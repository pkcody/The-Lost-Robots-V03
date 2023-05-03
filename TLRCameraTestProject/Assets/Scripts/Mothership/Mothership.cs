using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Mothership : MonoBehaviour
{
    public Slider h20Slider;

    public int itemsRecieved = 0;

    public bool fullH20 = false;
    public bool window = false;
    public bool battery = false;
    public bool inhaler = false;

    public bool tower1 = true;
    public bool tower2 = false;
    public bool tower3 = false;

    public bool canBoardShip = false;
    public int robBoardedNum = 0;

    //Backlog toggles
    public GameObject toggleTower2;
    public GameObject toggleTower3;
    public GameObject toggleMSLoc;
    public GameObject toggleAllParts;
    public GameObject toggleWindow;
    public GameObject toggleH2O;
    public GameObject toggleBattery;


    //particles
    public GameObject smokeParticle;
    public GameObject EndMSObj;
    public GameObject locationIcon;

    public GameObject storyMovies;

    public GameObject whiteScreen;

    //Camera
    public GameObject LeavePlanetCamera;
    public GameObject MainCamera;
    public int playcount = 0;

    //Bad ending
    public bool AllDead = false;
    public GameObject AllDeadEnding;
    public int stillAlive;

    private void Start()
    {
        h20Slider.gameObject.SetActive(false);
        locationIcon.SetActive(false);

        storyMovies = GameObject.Find("StoryMovies");
        playcount = PlayerInputManager.instance.playerCount;

        foreach (var anim in GetComponentsInChildren<Animator>(true))
        {
            if (anim.gameObject.name.Contains(playcount.ToString()))
            {
                EndMSObj = anim.gameObject;
                EndMSObj.SetActive(true);
                break;
            }
        }
        
    }
    public void TryMotherShipEnd()
    {
        if(fullH20 && window && battery && inhaler)
        {
            // where end audio goes
            canBoardShip = true;

            //toggles
            //toggleAllParts.GetComponent<Toggle>().isOn = false;

            MotherShipStory.instance.MSTalk("Outro_AllMissCounted");
            MotherShipSubTitles.instance.GameSubT(5);

            //foreach (var rm in FindObjectsOfType<RobotMessaging>())
            //{
            //    rm.MotherRobotSpeak("Yayyyy we did it!!!!!");
            //}
            //white
            whiteScreen.GetComponent<Animation>().Play("FadeWhite");
            //move
            foreach (var players in FindObjectsOfType<CharacterMovement>())
            {
                players.gameObject.transform.position = new Vector3(1000, -100, 1000);

            }
            //turn on new
            foreach (var robos in EndMSObj.GetComponentsInChildren<Transform>(true))
            {
                if (robos.gameObject.name.Contains("RobotWalkEnd"))
                {
                    robos.gameObject.SetActive(true);
                }
            }
            //change camera
            LeavePlanetCamera.SetActive(true);
            LeavePlanetCamera.tag = "MainCamera";
            MainCamera.tag = "Untagged";
            //unwhite fade
            whiteScreen.GetComponent<Animation>().Play("UnFadeWhite");

            //play animation
            EndMSObj.GetComponent<Animator>().enabled = true;

            //straight to credit
            Invoke("ToCreditScene", 8.95f);

        }
        else 
        {
            MotherShipStory.instance.MSTalk("Outro_" + itemsRecieved);
        }
    }

    

    public void ToCreditScene()
    {
        ScenesManager.instance.ChangeToScene("Quit");
    }

    public void CheckH20()
    {
        if(h20Slider.value == h20Slider.maxValue)
        {
            fullH20 = true;
            toggleH2O.GetComponent<Toggle>().isOn = false;

            h20Slider.gameObject.SetActive(false);
            TryMotherShipEnd();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(tower1 && tower2 && tower3)
        {
            if (other.name == "Window_obj")
            {
                window = true;
                toggleWindow.GetComponent<Toggle>().isOn = false;

                other.transform.root.GetComponent<CharHoldItem>().currentHold = null;
                other.transform.root.GetComponent<CharacterMovement>().inRangeHold = false;

                Destroy(other.gameObject);
                foreach (var chi in FindObjectsOfType<CharHoldItem>())
                {
                    print("Removing window from players");
                    chi.windowInRange = false;
                }

                //audio and commentary
                MotherShipStory.instance.MSTalk("Outro_theWindow");
                itemsRecieved++;

                TryMotherShipEnd();
                
            }
            else if (other.name == "QuadBattery")
            {
                battery = true;
                toggleBattery.GetComponent<Toggle>().isOn = false;

                other.transform.root.GetComponent<CharHoldItem>().currentHold = null;
                other.transform.root.GetComponent<CharacterMovement>().inRangeHold = false;


                Destroy(other.gameObject);
                foreach (var chi in FindObjectsOfType<CharHoldItem>())
                {
                    print("Removing battery from players");
                    chi.batteryInRange = false;
                }

                //audio and commentary
                MotherShipStory.instance.MSTalk("Outro_battery");
                itemsRecieved++;

                TryMotherShipEnd();
            }
            else if (other.name == "InhalerReceiver")
            {
                inhaler = true;
                toggleH2O.GetComponent<Toggle>().isOn = false;

                other.transform.root.GetComponent<CharHoldItem>().currentHold = null;
                other.transform.root.GetComponent<CharacterMovement>().inRangeHold = false;

                print("Removing inhaler from players");
                Destroy(other.gameObject);

                //audio and commentary
                MotherShipStory.instance.MSTalk("Outro_canister");
                itemsRecieved++;

                TryMotherShipEnd();
            }
        }
        
        
    }

    IEnumerator PlayVideo()
    {
        if (AllDead == true)
        {
            yield return new WaitForSeconds(1f);

            AllDeadEnding.SetActive(true);

            yield return new WaitForSeconds(10f);

        }

        ToCreditScene();

    }

    public void TestPlayersAlive()
    {
        stillAlive = playcount;
        foreach (var item in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (item.name.Contains("Player"))
            {
                if (item.GetComponent<CharacterMovement>())
                {
                    if (!item.GetComponent<CharacterMovement>().isAlive)
                    {
                        stillAlive--;

                        if (stillAlive == 0)
                        {
                            AllDead = true;
                            StartCoroutine(PlayVideo());
                        }

                    }
                }
            }


        }
    }

    private void Update()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);

        if (tower1 && tower2 && tower3)
        {
            locationIcon.SetActive(true);
            toggleMSLoc.GetComponent<Toggle>().isOn = false;

        }
    }

    public void Tower2Toggle()
    {
        toggleTower2.GetComponent<Toggle>().isOn = false;
        toggleTower3.SetActive(true);
    }
    public void Tower3Toggle()
    {
        toggleTower3.GetComponent<Toggle>().isOn = false;
        toggleMSLoc.SetActive(true);
        toggleAllParts.SetActive(true);
        toggleBattery.SetActive(true);
        toggleH2O.SetActive(true);
        toggleWindow.SetActive(true);
    }
}
