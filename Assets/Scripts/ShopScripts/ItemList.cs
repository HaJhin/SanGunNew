using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDB" , menuName = "ItemDB/ItemList")]
public class ItemList : ScriptableObject
{
    public Item[] itemList;
}
