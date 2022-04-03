using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "ScriptableObjects/ShopItem", order = 1)]
public class ShopItemSO : ScriptableObject
{
    public enum ItemKind
    {
        Energy,
        Resource,
        Item,
        StorageUpgrade,
        ContainerUpgrade
    }
    
    public ItemKind Kind;
    public string Name;
    public Sprite Sprite;
    public int Price;

    public int AmountAdded; //Fill every time
    public ItemData Item; //Leave empty if kind is energy

    public bool Big;
}
