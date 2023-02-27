using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.name.Contains("Tip"))
        {
            if (other.transform.parent.gameObject.TryGetComponent<CloudInhaler>(out CloudInhaler cloudInhaler))
            {
                cloudInhaler.canInhale = true;
                cloudInhaler.currInhalingClouds.Add(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Tip"))
        {
            if (other.transform.parent.gameObject.TryGetComponent<CloudInhaler>(out CloudInhaler cloudInhaler))
            {
                cloudInhaler.canInhale = false;
                cloudInhaler.currInhalingClouds.Remove(this);
            }
        }
    }
}
