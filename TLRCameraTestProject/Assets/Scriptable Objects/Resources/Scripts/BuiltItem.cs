using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Object", menuName = "Inventory System/Items/BuiltItem")]
public class BuiltItem : ItemObject
{
    public float itemsNeeded;

    public void Awake()
    {
        type = ItemType.BuiltItem;
    }
}
