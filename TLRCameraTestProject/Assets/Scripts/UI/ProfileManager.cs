using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    public GameObject[] playerProfiles;

    private void Start()
    {
        for(int i = 0; i < 4; i++)
        {
            if(PlayerSpawning.instance.players[i] != null)
            {
                playerProfiles[i].SetActive(true);
            }
            else
            {
                playerProfiles[i].SetActive(false);
            }
        }
    }
}
