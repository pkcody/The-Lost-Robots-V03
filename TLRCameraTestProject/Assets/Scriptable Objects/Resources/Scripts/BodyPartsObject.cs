using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Object", menuName = "Inventory System/Items/BodyParts")]
public class BodyPartsObject : ItemObject
{
    public float defence;
    public float hitExtra;
    public float whereOn;
    public void Awake()
    {
        type = ItemType.BodyParts;
    }
}
