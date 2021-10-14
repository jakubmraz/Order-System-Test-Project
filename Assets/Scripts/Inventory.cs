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

    public bool CheckForItemDuplicate(string itemName)
    {
        int itemCount = 0;

        foreach (var itemSlot in ItemSlots)
        {
            if (itemSlot.Item)
                if (itemSlot.Item.itemData.Name == itemName)
                    itemCount++;
        }

        if (itemCount >= 2)
            return true;
        return false;
    }

    public bool RemoveItem(string itemName)
    {
        foreach (var itemSlot in ItemSlots)
        {
            if (itemSlot.Item)
                if (itemSlot.Item.itemData.Name == itemName)
                {
                    Destroy(itemSlot.Item.gameObject);
                    return true;
                }
        }

        return true;
    }
}
