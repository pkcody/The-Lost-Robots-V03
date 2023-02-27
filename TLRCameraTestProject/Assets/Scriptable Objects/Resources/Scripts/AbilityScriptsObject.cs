using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Object", menuName = "Inventory System/Items/AbilityScripts")]
public class AbilityScriptsObject : ItemObject
{
    public float amountBonus;
    public float forWhat;

    public void Awake()
    {
        type = ItemType.AbilityScripts;
    }
}
