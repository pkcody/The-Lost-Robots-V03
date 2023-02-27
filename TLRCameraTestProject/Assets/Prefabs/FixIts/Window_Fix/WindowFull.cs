using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowFull : MonoBehaviour
{
    private List<Transform> glass = new List<Transform>();
    public GameObject fullGlass;
    
    public void AskFull()
    {
        foreach (var item in gameObject.GetComponentsInChildren<Transform>())
        {
            if (item.name.Contains("broken"))
            {
                if (!glass.Contains(item))
                {
                    glass.Add(item);
                }
            }
        }

        if (glass.Count == 5)
        {
            
            for (int i = 0; i < glass.Count; i++)
            {
                glass[i].gameObject.SetActive(false);
            }
            fullGlass.SetActive(true);
            gameObject.tag = "HoldItem";
            print("yay");
        }
    }
}
