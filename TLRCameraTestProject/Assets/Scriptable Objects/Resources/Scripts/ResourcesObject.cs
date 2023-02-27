using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Object", menuName = "Inventory System/Items/Resources")]
public class ResourcesObject : ItemObject
{
    public float amount;

    public void Awake()
    {
        type = ItemType.Resources;
    }
}
