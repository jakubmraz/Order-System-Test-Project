using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class ItemData
{
    public string Name;
    public Sprite Sprite;
    public string Recipe;
    //To explain, the 3x3 grid is first split into rows
    //000
    //000       -> 000 000 000 -> 000000000
    //000
    //0 stands for empty, an item is written, hence the recipe for a bike would be
    //Steel00
    //SteelSteelSteel   -> Steel00SteelSteelSteelRubber0Rubber
    //Rubber0Rubber

    public ItemData(string name, string spriteName, [CanBeNull] string recipe)
    {
        Name = name;
        Sprite = Resources.Load<Sprite>("Sprites/" + spriteName);
        Recipe = recipe ?? "000000000";

    }
}

public class Items
{
    public List<ItemData> ItemList = new List<ItemData>()
    {
        new ItemData("Bicycle", "bike", "Steel;0;0;Steel;Steel;Steel;Rubber;0;Rubber;"),
        new ItemData("Bag", "bag", "Plastic;0;Plastic;Plastic;Plastic;Plastic;Plastic;Plastic;Plastic;"),
        new ItemData("Watering Can", "wateringCan", "0;0;Plastic;Plastic;Plastic;0;Plastic;Plastic;0;")
    };

    public List<ItemData> BaseItemList = new List<ItemData>()
    {
        new ItemData("Steel", "steel", null),
        new ItemData("Rubber", "rubber", null),
        new ItemData("Plastic", "plastic", null)
    };

    public ItemData GetItemData(string itemName)
    {
        List<ItemData> allItems = ItemList.Concat(BaseItemList).ToList();
        return allItems.FirstOrDefault(item => item.Name == itemName);
    }
}
