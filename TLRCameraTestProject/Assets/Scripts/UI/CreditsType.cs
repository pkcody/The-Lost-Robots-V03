using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CreditsType : MonoBehaviour
{
    public TextMeshProUGUI text;
    [SerializeField] private float typingSpeed = 0.04f;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name.Contains("Quit"))
        {
            string response = @"                            Credits

    Paige Cody                        Programmer
    Ashton McDonald         Animator
    Arianna Tabatabaei      Modeller and UI


                             Voice Actors

    Anesah Price                   Mother Ship";

            StartCoroutine(DisplayLine(response));
        }
        else if (SceneManager.GetActiveScene().name.Contains("Game"))
        {
            string response = @"The journey of your robots end here. The boxes of nuts and bolts never made it back to Mothership and never learned why. Just why . . . ";

            StartCoroutine(DisplayLine(response));
        }


    //    string response = @"                            Credits

    //Paige Cody                        Programmer
    //Ashton McDonald         Animator
    //Arianna Tabatabaei      Modeller and UI


    //                         Voice Actors

    //Anesah Price                   Mother Ship";

        //StartCoroutine(DisplayLine(response));
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
        yield return new WaitForSeconds(5f);
        //text.gameObject.SetActive(false);
    }
}
