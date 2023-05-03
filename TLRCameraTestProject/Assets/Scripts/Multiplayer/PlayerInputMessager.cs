using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputMessager : MonoBehaviour
{
    public static PlayerInputMessager instance;

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

    private void OnPlayerJoined(PlayerInput player)
    {
        if(SceneManager.GetActiveScene().name == "Join")
        {
            print("death");
            FindObjectOfType<PlayerSpawning>().players[PlayerInputManager.instance.playerCount - 1] = player.gameObject;
            FindObjectOfType<PlayerSpawning>().SetInitialPlayerValues();
            FindObjectOfType<PauseHunter>().PauseHunt();
            player.GetComponent<JoinFunctionality>().OnJoin();
        }
        else
        {
            Destroy(player.transform.root.gameObject);
        }
        
    }

    private void OnPlayerLeft(PlayerInput player)
    {

        print("moredeath");
    }

}
