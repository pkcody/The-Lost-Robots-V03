using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BacklogOnOff : MonoBehaviour
{
    public Toggle backlogPanel;

    public void ToggleBacklog()
    {
        backlogPanel.isOn = !backlogPanel.isOn;
    }
}
