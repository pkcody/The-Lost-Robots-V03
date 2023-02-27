using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreditsType : MonoBehaviour
{
    public TextMeshProUGUI text;
    [SerializeField] private float typingSpeed = 0.04f;
    // Start is called before the first frame update
    void Start()
    {
        string response = @"                            Credits

    Paige Cody                     Programmer
    Ashton McDonald         Animator
    Arianna Tabatabaei      Modeller


                             Voice Actors

    Allison Evdokimo            Robots
    Anesah Price                   Mother Ship";

        StartCoroutine(DisplayLine(response));
    }



    private IEnumerator DisplayLine(string line)
    {
        yield return new WaitForSeconds(typingSpeed);

        text.text = " ";
        text.gameObject.SetActive(true);
        foreach (char letter in line.ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(2f);
        //text.gameObject.SetActive(false);
    }
}
