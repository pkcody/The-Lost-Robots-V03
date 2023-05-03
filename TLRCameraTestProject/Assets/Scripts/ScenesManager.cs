using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager instance;

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
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void StartGameScene()
    {
        if (PlayerSpawning.instance.players[0] != null)
        {
            SceneManager.LoadScene("Game");
        }
    }
    public void ContinueToGameScene()
    {
        if (PlayerSpawning.instance.players[0] != null)
        {
            FindObjectOfType<PlayerSpawning>().BIGTutorialON = false;

            SceneManager.LoadScene("Game");
        }
    }

    public void GoMainMenuScene()
    {
        foreach (var players in FindObjectsOfType<JoinFunctionality>())
        {
            if (!players.isReady)
            {
                return;
            }
        }
        
        if (PlayerSpawning.instance.players[1] != null)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void ChangeToScene(string scene)
    {
        SceneManager.LoadScene(scene);

    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayerSpawning.instance.ChangePlayerInput();
        PlayerSpawning.instance.StartingPositions();
    }
}
