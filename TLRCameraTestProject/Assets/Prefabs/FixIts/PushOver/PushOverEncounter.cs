using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PushOverEncounter : MonoBehaviour
{
    public int numPlayersColliding = 0;
    public GameObject batteryOnMe;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            numPlayersColliding++;
            TryTipping();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            numPlayersColliding--;
        }
    }

    public void TryTipping()
    {
        print(PlayerInputManager.instance.playerCount);
        if(numPlayersColliding == PlayerInputManager.instance.playerCount)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            
            
            Invoke("DropBattery", .5f);
            Invoke("FreezeRb", 3f);
        }
    }

    public void DropBattery()
    {
        batteryOnMe.GetComponent<Rigidbody>().useGravity = true;
        //Invoke("MakeBatteryPickup", 2.5f);
    }
    
    public void MakeBatteryPickup()
    {
        Destroy(batteryOnMe.GetComponent<Rigidbody>());
        //batteryOnMe.GetComponent<BoxCollider>().isTrigger = true;
    }
    public void FreezeRb()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
}
