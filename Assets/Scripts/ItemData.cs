using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemData:ScriptableObject
{
    public string Name;
    public Sprite Sprite;
    public string Recipe;
    public string Description;
    public int Value;
    public bool Final;
    public int Level = 1;
    //To explain, the 3x3 grid is first split into rows
    //000
    //000       -> 000 000 000 -> 000000000
    //000
    //0 stands for empty, an item is written, hence the recipe for a bike would be
    //Steel00
    //SteelSteelSteel   -> Steel00SteelSteelSteelRubber0Rubber
    //Rubber0Rubber
}

[CreateAssetMenu(fileName = "ItemList", menuName = "ScriptableObjects/ItemList", order = 1)]
public class Items:ScriptableObject
{
    public List<ItemData> ItemList = new List<ItemData>();

    public List<ItemData> BaseItemList = new List<ItemData>();
}
