using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnowledgeScriptAbility : MonoBehaviour
{

    //Show how to pick up
    // play speaking parts 
    //wait and then have another player run into the tower
    // comment on tower
    // then allow player to interact with tower
    // someone comment on the sound and how they should try to clear it up

    public Transform towerMe;

    void Start()
    {
        foreach (var can in towerMe.GetComponentsInChildren<Canvas>())
        {
            can.gameObject.SetActive(false);

        }
    }


    public void TurnOnAll()
    {

        foreach (var can in towerMe.GetComponentsInChildren<Canvas>(true))
        {
            print(can.name);
            can.gameObject.SetActive(true);
            

        }
        FindObjectOfType<PlayerSpawning>().tutorialON = false;
        FindObjectOfType<PlayerSpawning>().ChangePlayerInput();

        FindObjectOfType<TowerSoundEffect>().tutorialON = false;


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TowerCraftingEncounter")
        {
            TurnOnAll();
            foreach (var tsp in FindObjectsOfType<TutorialScriptPickUp>())
            {
                tsp.tutorialON = false;

            }
            transform.root.GetComponent<RobotMessaging>().TowerRobotSpeak("Can you hear that static? I think it's trying to say something. Maybe if we make more towers we could clear it up.");

            Destroy(gameObject);
        }
    }
}
