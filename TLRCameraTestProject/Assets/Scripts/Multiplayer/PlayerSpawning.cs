using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Cinemachine;

//Spawns the player in different locations
public class PlayerSpawning : MonoBehaviour
{
    public static PlayerSpawning instance;

    public GameObject[] players = new GameObject[4];
    public Transform[] hoverInfo = new Transform[4];
    public Material[] mats = new Material[4];
    public Material[] personalityMats = new Material[4];
    public InventoryObject[] inv = new InventoryObject[4];

    public Transform[] JoinSpawnPos = new Transform[4];
    public Transform[] MenuSpawnPos = new Transform[4];
    public Transform[] GameSpawnPos = new Transform[4];
    public Transform[] TutorialSpawnPos = new Transform[4];

    public bool lockedIn = false;

    public CinemachineTargetGroup targetbrain;

    //Ready UP
    public bool readyUpCheck;
    public GameObject[] allReady = new GameObject[4];

    //tutorial
    public bool tutorialON = true;
    public bool BIGTutorialON = true;

    //Camera Boundries
    public GameObject tutorialBoundary;
    public GameObject gameBoundary;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        
     
    }

    public void ChangePlayerInput()
    {
        foreach(GameObject go in players)
        {
            if (go != null)
            {
                int Index = System.Array.IndexOf(players, go);

                // Different spawns for each scene
                if (SceneManager.GetActiveScene().name == "Join")
                {
                    go.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
                    print("calling");
                    go.GetComponent<PlayerInput>().defaultActionMap = "UI";
                }
                else if (SceneManager.GetActiveScene().name == "MainMenu")
                {
                    go.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
                    print("calling");
                    go.GetComponent<PlayerInput>().defaultActionMap = "UI";
                }
                else if (BIGTutorialON && SceneManager.GetActiveScene().name == "Game") //
                {
                    tutorialBoundary = GameObject.Find("TutorialPlayerBoundries");

                    go.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
                    go.GetComponent<PlayerInput>().defaultActionMap = "Player";
                    go.GetComponent<PlayerPainting>().enabled = false;

                    foreach (var cc in FindObjectsOfType<CinemachineConfiner>())
                    {
                        cc.m_BoundingVolume = tutorialBoundary.GetComponent<Collider>();
                    }

                    FindObjectOfType<BiomeTracker>().Default();

                }
                else if (tutorialON && SceneManager.GetActiveScene().name == "Game") //
                {
                    gameBoundary = GameObject.Find("PlayerBoundries");

                    go.GetComponent<PlayerInput>().SwitchCurrentActionMap("TutorialMap");
                    go.GetComponent<PlayerInput>().defaultActionMap = "TutorialMap";
                    go.GetComponent<PlayerPainting>().enabled = false;

                    //biome checker on
                    FindObjectOfType<BiomeTracker>().PlayThisAudio();


                    foreach (var cc in FindObjectsOfType<CinemachineConfiner>())
                    {
                        cc.m_BoundingVolume = gameBoundary.GetComponent<Collider>();
                    }
                }
                else if (!tutorialON && SceneManager.GetActiveScene().name == "Game")
                {
                    go.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
                    go.GetComponent<PlayerInput>().defaultActionMap = "Player";
                    go.GetComponent<PlayerPainting>().enabled = false;
                }
                else if (SceneManager.GetActiveScene().name == "Credits")
                {
                    go.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
                    print("calling");
                    go.GetComponent<PlayerInput>().defaultActionMap = "UI";
                }
            }
        }
    }

    public void StartingPositions()
    {
        foreach (GameObject go in players)
        {
            if (go != null)
            {
                int Index = System.Array.IndexOf(players, go);



                if (SceneManager.GetActiveScene().name == "Game")
                {

                    targetbrain = FindObjectOfType<CinemachineTargetGroup>();
                    

                    if (BIGTutorialON)
                    {
                        go.transform.position = TutorialSpawnPos[Index].position;

                    }
                    else
                    {
                        go.transform.position = GameSpawnPos[Index].position;

                    }
                    go.GetComponent<CharacterMovement>().enabled = true;
                    go.GetComponent<CharacterMovement>().cinemachineTargetGroup = targetbrain;
                   // if (go.GetComponent<CharacterMovement>().tutorialSetBegin)
                   // {
                        go.GetComponent<CharacterMovement>().BeginGame();
                       // go.GetComponent<CharacterMovement>().tutorialSetBegin = false;
                    //}

                    //cam
                    targetbrain.AddMember(go.transform, 1f, 5f);

                    //inv
                    print(inv[Index]);
                    go.GetComponent<DisplayingInventory>().enabled = true;
                    go.GetComponent<DisplayingInventory>().inventoryObj = inv[Index];

                }
            }
        }
    }

    public void StartGameAfterTutorial()
    {
        BIGTutorialON = false;

        //show time
        MotherShipStory.instance.GameActuallyStarting();
        GameObject storyMovies = GameObject.Find("StoryMovies");

        storyMovies.transform.GetChild(0).gameObject.SetActive(true);
        storyMovies.transform.GetChild(1).gameObject.SetActive(true);

        StartCoroutine(DisableVideo0());
        StartCoroutine(DisableVideo1());

        ChangePlayerInput();
        StartingPositions();

        FindObjectOfType<BiomeTracker>().PlayThisAudio();

    }

    IEnumerator DisableVideo0()
    {
        GameObject storyMovies = GameObject.Find("StoryMovies");

        yield return new WaitForSeconds(16f);
        storyMovies.transform.GetChild(0).gameObject.SetActive(false);
    }
    IEnumerator DisableVideo1()
    {
        GameObject storyMovies = GameObject.Find("StoryMovies");

        yield return new WaitForSeconds(18f);
        storyMovies.transform.GetChild(1).gameObject.SetActive(false);

        //audio for biome checker turn on
        FindObjectOfType<BiomeTracker>().PlayThisAudio();

    }

    public void SetInitialPlayerValues()
    {
        foreach(GameObject go in players)
        {
            if (go != null)
            {
                int Index = System.Array.IndexOf(players, go);
                go.transform.position = JoinSpawnPos[Index].position;

                readyUpCheck = false;

                go.transform.eulerAngles = Vector3.zero;
                print("hi");

                if(Index == 0)
                {
                    MotherShipStory.instance.MSTalk("Intro_" + PlayerInputManager.instance.playerCount);
                }


                go.GetComponent<JoinFunctionality>().personalityMats = personalityMats;
                

                foreach (var mr in go.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    //print(mr.gameObject.name);
                    if (mr.material.name.Contains("ReplaceMe"))
                    {
                        mr.material = mats[Index];

                    }
                }
                
            }

        }
        FindObjectOfType<PauseHunter>().PauseHunt();
    }

}
