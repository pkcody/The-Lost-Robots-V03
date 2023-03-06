using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleTutorial : MonoBehaviour
{
    public GameObject On;
    public GameObject Off;

    public bool tutorialOn = true;

    private bool canToggle = true;

    IEnumerator delayToggle()
    {
        canToggle = false;
        yield return new WaitForSeconds(0.25f);
        canToggle = true;

    }
    public void Toggle()
    {
        if (canToggle)
        {
            tutorialOn = !tutorialOn;
            On.SetActive(!On.activeSelf);
            Off.SetActive(!Off.activeSelf);
            StartCoroutine(delayToggle());
        }
        
    }
}
