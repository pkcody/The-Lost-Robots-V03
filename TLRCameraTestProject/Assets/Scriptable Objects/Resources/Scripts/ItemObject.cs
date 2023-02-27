using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Resources,
    BodyParts,
    BuiltItem,
    AbilityScripts,
    Default
}
public class ItemObject : ScriptableObject
{
    public GameObject prefab;
    public ItemType type;
    public Sprite UIimage;
    [TextArea(15, 20)]
    public string description;
}
