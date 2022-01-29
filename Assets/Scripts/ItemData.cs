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
    //To explain, the 3x3 grid is first split into rows
    //000
    //000       -> 000 000 000 -> 000000000
    //000
    //0 stands for empty, an item is written, hence the recipe for a bike would be
    //Steel00
    //SteelSteelSteel   -> Steel00SteelSteelSteelRubber0Rubber
    //Rubber0Rubber

    ////For compatibility
    //public ItemData(string name, string spriteName, [CanBeNull] string recipe)
    //{
    //    Name = name;
    //    Sprite = Resources.Load<Sprite>("Sprites/" + spriteName);
    //    Recipe = recipe ?? "000000000";
    //    Description = "no description";
    //    Value = 0;
    //}

    //public ItemData(string name, string spriteName, [CanBeNull] string recipe, string description, int value)
    //{
    //    Name = name;
    //    Sprite = Resources.Load<Sprite>("Sprites/" + spriteName);
    //    Recipe = recipe ?? "000000000";
    //    Description = description;
    //    Value = value;
    //}
}

[CreateAssetMenu(fileName = "ItemList", menuName = "ScriptableObjects/ItemList", order = 1)]
public class Items:ScriptableObject
{
    public List<ItemData> ItemList = new List<ItemData>()
    {
        //new ItemData("Bicycle", "bike", "Steel;0;0;Steel;Steel;Steel;Rubber;0;Rubber;", "A bicycle, used for getting around fast.", 200),
        //new ItemData("Bag", "bag", "Plastic;0;Plastic;Plastic;Plastic;Plastic;Plastic;Plastic;Plastic;", "A plastic bag, used for carrying things around. Produces lots of plastic waste.", 5),
        //new ItemData("Watering Can", "wateringCan", "0;0;Plastic;Plastic;Plastic;0;Plastic;Plastic;0;", "A plastic watering can, used to water plants. Made out of sturdy HDPE plastic.", 60)
    };

    public List<ItemData> BaseItemList = new List<ItemData>()
    {
        //new ItemData("Steel", "steel", null, "A piece of steel, used to create objects. Is also a useful weapon in the ghetto.", 10),
        //new ItemData("Rubber", "rubber", null, "A rubber tire, good for driving on and executing people in the favellas. Can also be used as a swing for kids.", 10),
        //new ItemData("Plastic", "plastic", null, "A piece of used plastic. Every time you toss one on the ground, a turtle dies.", 1)
    };
}
