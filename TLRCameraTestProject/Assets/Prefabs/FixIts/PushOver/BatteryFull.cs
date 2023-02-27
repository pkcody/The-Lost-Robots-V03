using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryFull : MonoBehaviour
{
    private List<Transform> batteries = new List<Transform>();
    public GameObject fullBattery;

    public void AskFull()
    {
        foreach (var item in gameObject.GetComponentsInChildren<Transform>())
        {
            if (item.name.Contains("Cell"))
            {
                if (!batteries.Contains(item))
                {
                    batteries.Add(item);
                }
            }
        }

        if (batteries.Count == 4)
        {

            for (int i = 0; i < batteries.Count; i++)
            {
                batteries[i].gameObject.SetActive(false);
            }
            fullBattery.SetActive(true);
            gameObject.tag = "HoldItem";
            print("yay");
        }
    }
}
