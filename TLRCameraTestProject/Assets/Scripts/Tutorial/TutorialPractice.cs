using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPractice : MonoBehaviour
{
    public GameObject walk_tut;
    public GameObject break_tut;
    public GameObject craft_tut;
    public GameObject inv_tut;
    public GameObject repair_tut;

    private void OnTriggerEnter(Collider other)
    {
        if(other == walk_tut.GetComponent<Collider>())
        {

        }
    }


    public void Tutorial_walk()
    {

    }
}
