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

    //particles
    public GameObject locationIcon;

    private void Start()
    {
        h20Slider.gameObject.SetActive(false);
        locationIcon.SetActive(false);

    }
    public void TryMotherShipEnd()
    {
        if(fullH20 && window && battery && inhaler)
        {
            // where end audio goes
            canBoardShip = true;
            MotherShipStory.instance.MSTalk("Outro_AllMissCounted");
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
                if (robBoardedNum == PlayerInputManager.instance.playerCount)
                {
                    MotherShipStory.instance.MSTalk("Outro_Thankyou");
                    Invoke("ToCreditScene", 4.5f);
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
        }
    }
}
