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
    public GameObject liftOffAnim;
    public GameObject locationIcon;

    public GameObject storyMovies;

    //Camera
    public GameObject LeavePlanetCamera;
    public GameObject MainCamera;
    public int playcount = 0;

    private void Start()
    {
        h20Slider.gameObject.SetActive(false);
        locationIcon.SetActive(false);

        storyMovies = GameObject.Find("StoryMovies");
        playcount = PlayerInputManager.instance.playerCount;
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

            foreach (var rm in FindObjectsOfType<RobotMessaging>())
            {
                rm.MotherRobotSpeak("Yayyyy we did it!!!!!");
            }

            
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
        if (other.name.Contains("Player"))
        {
            if (canBoardShip)
            {
                Destroy(other.gameObject);
                robBoardedNum++;

                if (robBoardedNum == playcount)
                {
                    MotherShipStory.instance.MSTalk("Outro_Thankyou");
                    MotherShipSubTitles.instance.GameSubT(6);
                    //play videos
                    //mothership animation and change camera
                    LeavePlanetCamera.SetActive(true);
                    LeavePlanetCamera.tag = "MainCamera";
                    MainCamera.tag = "Untagged";
                    print("try to leave please");
                    StartCoroutine(PlayVideo());

                    //Invoke("ToCreditScene", 8f);
                }
            }
        }
        
    }

    IEnumerator PlayVideo()
    {
        smokeParticle.SetActive(true);
        liftOffAnim.GetComponent<Animation>().Play("LiftOff");

        yield return new WaitForSeconds(6f);

        storyMovies.transform.GetChild(2).gameObject.SetActive(true);


        yield return new WaitForSeconds(6f);
        storyMovies.transform.GetChild(3).gameObject.SetActive(true);
        storyMovies.transform.GetChild(2).gameObject.SetActive(false);

        yield return new WaitForSeconds(3f);
        //storyMovies.transform.GetChild(3).gameObject.SetActive(false);
        ToCreditScene();

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
