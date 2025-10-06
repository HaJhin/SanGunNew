using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ItemType {Heal,Shield}

[System.Serializable]
public class Item
{
    public string itemName;
    public int price;
    public string description;
    public ItemType type;

}
