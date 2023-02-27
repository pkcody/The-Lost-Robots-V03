using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerCalc : MonoBehaviour
{
    public InventoryObject towerInv;

    public TextMeshProUGUI text;
    private List<GameObject> itemTowerList = new List<GameObject>();

    public List<TextMeshProUGUI> craftingTypeTexts;

    public List<Transform> CraftSlots;
    public GameObject CraftedSlot;

    // trying recipes
    public List<GameObject> CraftMe;

    

    private void Start()
    {
        towerInv.Container.Clear();
        //get robot UI
        craftingTypeTexts = GetComponent<CharacterMovement>().craftingTypeTexts;
        //craftingtyetext

        //Fill inv
        //body parts
        for (int i = 0; i < 3; i++)
        {
            // image slots
            CraftSlots.Add(gameObject.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(3).GetChild(i));
            
        }

        //add recipes
        //CraftMe.Add()

    }

    public void RecipeHolder()
    {
        
        //foreach (GameObject obj in CraftSlots)
        //{
        //    Item item = obj.GetComponent<Item>();
        //    if (craftingTypeTexts[0])
        //    {
        //        CraftSlots[0].GetComponent<Image>().sprite = item.;
        //    }
        //}
        
    }


}
