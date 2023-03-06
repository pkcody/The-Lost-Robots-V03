using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonPush : MonoBehaviour
{
    public bool pushable = false;
    public int num = 1;

    private void Start()
    {
        if(num > PlayerInputManager.instance.playerCount)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    public void TryButtonPush(int playerIndex)
    {
        print("pushing");
        if (playerIndex == num)
        {

            pushable = true;
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z);
            CheckIfAllPlayersReady();

        }
    }

    // function to start game

    public void CheckIfAllPlayersReady()
    {
        int count = 0;
        foreach (var bp in FindObjectsOfType<ButtonPush>())
        {
            if (bp.pushable)
            {
                count++;
                if (count == PlayerInputManager.instance.playerCount)
                {
                    PlayerSpawning.instance.StartGameAfterTutorial();
                }
            }
        }
    }
}
