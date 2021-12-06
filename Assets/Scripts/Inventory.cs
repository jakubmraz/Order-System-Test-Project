using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class Inventory : MonoBehaviour
{
    public Item ItemPrefab;
    private SavingLoading savingLoading;

    private List<ItemSlot> itemSlots;

    void Awake()
    {
        savingLoading = GetComponent<SavingLoading>();
        //Called from UI on startup now
        //LoadSavedInventory();
    }

    public void AssignNewInventorySlots(List<ItemSlot> itemSlots)
    {
        this.itemSlots = itemSlots;
        LoadSavedInventory();
    }

    public void FlushInventory()
    {
        foreach (var slot in itemSlots)
        {
            if (slot.Item)
            {
                Destroy(slot.Item.gameObject);
                slot.Item = null;
            }
        }
    }

    public ItemSlot FindFirstEmptySlot()
    {
        foreach (var itemSlot in itemSlots)
        {
            if (!itemSlot.Item)
                return itemSlot;
        }

        return null;
    }

    public void LoadSavedInventory()
    {
        FlushInventory();
        string inventoryString = savingLoading.LoadInventoryData();
        FillInventory(inventoryString);
    }

    private void FillInventory(string inventoryString)
    {
        string[] splitString = inventoryString.Split(';');
        int i = 0;

        bool isBroken = false;
        foreach (var itemSlot in itemSlots)
        {
            if (splitString[i] != "0")
            {
                if (splitString[i].Contains("!"))
                {
                    splitString[i] = splitString[i].Replace("!", "");
                    isBroken = true;
                }
                itemSlot.Item = Instantiate(ItemPrefab, itemSlot.transform).GetComponent<Item>();
                itemSlot.Item.InitializeItem(splitString[i]);
                if(isBroken)
                    itemSlot.Item.BreakItem();
            }

            i++;
            isBroken = false;
        }
    }

    internal string GetInventoryString()
    {
        string inventoryString = "";

        foreach (var itemSlot in itemSlots)
        {
            if (itemSlot.Item)
            {
                if (itemSlot.Item.IsBroken)
                    inventoryString += "!";
                inventoryString += itemSlot.Item.itemData.Name;
            }
                
            else
                inventoryString += 0;

            inventoryString += ";";
        }

        return inventoryString;
    }

    public bool CheckForItem(string itemName)
    {
        foreach (var itemSlot in itemSlots)
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

        foreach (var itemSlot in itemSlots)
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
        foreach (var itemSlot in itemSlots)
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

    public void AddRandomBrokenItem()
    {
        Items items = new Items();
        int randomIndex = Random.Range(0, items.ItemList.Count - 1);
        string randomItem = items.ItemList[randomIndex].Name;

        ItemSlot emptySlot = FindFirstEmptySlot();
        emptySlot.Item = Instantiate(ItemPrefab, emptySlot.transform).GetComponent<Item>();
        emptySlot.Item.InitializeItem(randomItem);
        emptySlot.Item.BreakItem();
    }
}
