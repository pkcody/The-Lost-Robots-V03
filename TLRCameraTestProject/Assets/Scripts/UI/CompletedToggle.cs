using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CompletedToggle : MonoBehaviour
{
    public Image toggleImg;
    public TextMeshProUGUI scratchThis;

    public Sprite xDone;

    public void TaskCompletedToggleOff()
    {
        toggleImg.sprite = xDone;
        scratchThis.text = "<s>" + scratchThis.text + "</s>";
    }
}
