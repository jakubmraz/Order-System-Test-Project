using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ItemSlot> ItemSlots;

    public ItemSlot FindFirstEmptySlot()
    {
        foreach (var itemSlot in ItemSlots)
        {
            if (!itemSlot.Item)
                return itemSlot;
        }

        return null;
    }

    public bool CheckForItem(string itemName)
    {
        foreach (var itemSlot in ItemSlots)
        {
            if(itemSlot.Item)
                if (itemSlot.Item.itemData.Name == itemName)
                    return true;
        }

        return false;
    }

    public void RemoveItem(string itemName)
    {
        foreach (var itemSlot in ItemSlots)
        {
            if (itemSlot.Item)
                if (itemSlot.Item.itemData.Name == itemName)
                {
                    Destroy(itemSlot.Item.gameObject);
                    break;
                }
        }
    }
}
