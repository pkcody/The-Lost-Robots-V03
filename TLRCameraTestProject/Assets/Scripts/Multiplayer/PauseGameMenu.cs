using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseGameMenu : MonoBehaviour
{
    public bool isPaused;
    public PlayerInput playerInput;
    public GameObject pauseGameMenu;
    public GameObject settingsMenu;
    public GameObject mainMenu;
    public GameObject paintingMenu;
    public GameObject TopButtonOnSettings;

    private GameObject pauseFirstBtn;
    private GameObject settingsFirstBtn;
    private GameObject mainFirstBtn;
    private GameObject paintFirstBtn;



    public void TogglePause(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {


            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                bool backToMain = !mainMenu.activeSelf;
                settingsMenu.SetActive(!backToMain);
                
                mainMenu.SetActive(backToMain);

                GetFirstBtns();

                EventSystem.current.SetSelectedGameObject(backToMain ? mainFirstBtn : settingsFirstBtn);
            }
            

            if(pauseGameMenu != null)
            {
                print("Toggle Pause");
                bool newPauseMenuState = !pauseGameMenu.activeSelf;
                print(newPauseMenuState);
                pauseGameMenu.SetActive(newPauseMenuState);
                //Time.timeScale = newPauseMenuState ? 0 : 1;


                if (newPauseMenuState)
                {
                    Debug.Log("Game Paused");
                    if (SceneManager.GetActiveScene().name == "Game")
                    {
                        FindObjectOfType<TowerProgress>().PlayTowerSoundsPause();
                    }
                    foreach (GameObject go in PlayerSpawning.instance.players)
                    {
                        if(go != null)
                        {
                            go.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
                            go.GetComponent<PlayerInput>().defaultActionMap = "UI";
                        }

                    }
                    GetFirstBtns();

                    EventSystem.current.SetSelectedGameObject(pauseFirstBtn);
                    pauseFirstBtn.GetComponent<Button>().Select();
                    InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;

                }
                else
                {
                    Debug.Log("Game Resumed");
                    foreach (GameObject go in PlayerSpawning.instance.players)
                    {
                        if (go != null)
                        {
                            FindObjectOfType<MotherShipStory>()._as.Stop();

                            if (go.GetComponent<TutorialScriptPickUp>().tutorialON)
                            {
                                go.GetComponent<PlayerInput>().SwitchCurrentActionMap("TutorialMap");
                                go.GetComponent<PlayerInput>().defaultActionMap = "TutorialMap";
                            }
                            else
                            {
                                go.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
                                go.GetComponent<PlayerInput>().defaultActionMap = "Player";
                            }
                            
                        }

                    }
                    InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
                }

            }
            
        }

    }

    public void ReturnFromMenuToPrevious(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if(SceneManager.GetActiveScene().name == "MainMenu")
            {
                print("Back to Main Menu");
                settingsMenu.SetActive(false);
                if(paintingMenu == null)
                {
                    foreach (RectTransform go in Resources.FindObjectsOfTypeAll(typeof(RectTransform)) as RectTransform[])
                    {
                        print("look");
                        if (go.name == "PaintingMenu")
                        {
                            print("find");
                            paintingMenu = go.gameObject;
                        }
                    }
                }
                paintingMenu.SetActive(false);
                mainMenu.SetActive(true);
                
                GetFirstBtns();

                EventSystem.current.SetSelectedGameObject(mainFirstBtn);

            }
            if (settingsMenu != null && pauseGameMenu != null)
            {
                print("Back to Pause Menu");
                
                if (SceneManager.GetActiveScene().name == "MainMenu")
                {
                    pauseGameMenu.SetActive(true);
                }

                settingsMenu.SetActive(false);
                TopButtonOnSettings.GetComponent<Button>().Select();


            }
            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
        }


    }

    private void GetFirstBtns()
    {
        if(pauseGameMenu != null)
        {
            foreach (Button go in Resources.FindObjectsOfTypeAll(typeof(Button)) as Button[])
            {
                if (go.name == "Continue")
                {
                    pauseFirstBtn = go.gameObject;
                }
            }
        }
        if(settingsMenu != null)
        {
            foreach (Button go in Resources.FindObjectsOfTypeAll(typeof(Button)) as Button[])
            {
                if (go.name == "ControlsButton")
                {
                    settingsFirstBtn = go.gameObject;
                }
            }
        }
        if(mainMenu != null)
        {
            mainFirstBtn = mainMenu.transform.GetChild(1).gameObject;
        }
    }


}
