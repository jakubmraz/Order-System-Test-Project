using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ItemSlot> ItemSlots;
    public Item ItemPrefab;
    private SavingLoading savingLoading;

    void Awake()
    {
        savingLoading = GetComponent<SavingLoading>();
        LoadSavedInventory();
    }

    public ItemSlot FindFirstEmptySlot()
    {
        foreach (var itemSlot in ItemSlots)
        {
            if (!itemSlot.Item)
                return itemSlot;
        }

        return null;
    }

    public void LoadSavedInventory()
    {
        string inventoryString = savingLoading.LoadInventoryData();
        FillInventory(inventoryString);
    }

    private void FillInventory(string inventoryString)
    {
        string[] splitString = inventoryString.Split(';');
        int i = 0;

        foreach (var itemSlot in ItemSlots)
        {
            if (splitString[i] != "0")
            {
                itemSlot.Item = Instantiate(ItemPrefab, itemSlot.transform).GetComponent<Item>();
                itemSlot.Item.InitializeItem(splitString[i]);
            }

            i++;
        }
    }

    internal string GetInventoryString()
    {
        string inventoryString = "";

        foreach (var itemSlot in ItemSlots)
        {
            if (itemSlot.Item)
                inventoryString += itemSlot.Item.itemData.Name;
            else
                inventoryString += 0;

            inventoryString += ";";
        }

        return inventoryString;
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
