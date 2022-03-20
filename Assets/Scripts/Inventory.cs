using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    //Make sure to update the UI script in the editor accordingly
    public int NumberOfInventorySlots;
    public const int MaxStackCount = 30;

    public Item ItemPrefab;
    private SavingLoading savingLoading;

    private List<ItemSlot> itemSlots;

    void Awake()
    {
        Instance = this;
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

    public ItemSlot FindFirstNonFullSlotWithItem(string item, bool broken)
    {
        foreach (var itemSlot in itemSlots)
        {
            if (itemSlot.Item != null && itemSlot.Item.itemData.Name == item && itemSlot.Item.IsBroken == broken && itemSlot.Item.count < MaxStackCount)
                return itemSlot;
        }

        return null;
    }

    public void LoadSavedInventory()
    {
        FlushInventory();
        string inventoryString = savingLoading.LoadInventoryData();
        string numberString = savingLoading.LoadInventoryNumberData();
        FillInventory(inventoryString);
        FillInventoryNumbers(numberString);
    }

    private void FillInventory(string inventoryString)
    {
        string[] splitString = inventoryString.Split(';');

        //-1 because the code counts the "" after the last ; as another member of the array
        int slotLoadDifference = Math.Abs(splitString.Length - 1 - NumberOfInventorySlots);

        if (slotLoadDifference > 0)
        {
            for (int i = 0; i < slotLoadDifference; i++)
            {
                inventoryString += "0;";
            }

            splitString = inventoryString.Split(';');
        }

        bool isBroken = false;
        for (int i = 0; i < NumberOfInventorySlots; i++)
        {
            if (splitString[i] != "0" && splitString[i] != "")
            {
                if (splitString[i].Contains("!"))
                {
                    splitString[i] = splitString[i].Replace("!", "");
                    isBroken = true;
                }
                itemSlots[i].Item = Instantiate(ItemPrefab, itemSlots[i].transform).GetComponent<Item>();
                itemSlots[i].Item.InitializeItem(splitString[i]);
                if(isBroken)
                    itemSlots[i].Item.BreakItem();
            }
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

    public string GetInventoryNumbersString()
    {
        string inventoryString = "";

        foreach (var itemSlot in itemSlots)
        {
            if (itemSlot.Item)
            {
                inventoryString += itemSlot.Item.count + "";
            }
            else
            {
                inventoryString += 0;
            }

            inventoryString += ";";
        }

        return inventoryString;
    }

    private void FillInventoryNumbers(string numberString)
    {
        if (numberString == "") return;

        string[] splitString = numberString.Split(';');

        //-1 because the code counts the "" after the last ; as another member of the array
        int slotLoadDifference = Math.Abs(splitString.Length - 1 - NumberOfInventorySlots);

        if (slotLoadDifference > 0)
        {
            for (int i = 0; i < slotLoadDifference; i++)
            {
                numberString += "0;";
            }

            splitString = numberString.Split(';');
        }

        for (int i = 0; i < NumberOfInventorySlots; i++)
        {
            if (splitString[i] != "0" && splitString[i] != "")
            {
                itemSlots[i].Item.count = Convert.ToInt32(splitString[i]);
                itemSlots[i].Item.UpdateCountText();
            }
        }
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

    public bool CheckForNonBrokenItem(string itemName)
    {

        foreach (var itemSlot in itemSlots)
        {
            if (itemSlot.Item)
                if (itemSlot.Item.itemData.Name == itemName && !itemSlot.Item.IsBroken)
                    return true;
        }

        return false;
    }

    //Does not check for broken items since the only usage of this method is in the order system, create a new one without the broken check if needed
    public bool CheckForItemDuplicate(string itemName)
    {
        int itemCount = 0;

        foreach (var itemSlot in itemSlots)
        {
            if (itemSlot.Item)
                if (itemSlot.Item.itemData.Name == itemName && !itemSlot.Item.IsBroken)
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
                    //Destroy(itemSlot.Item.gameObject);
                    //itemSlot.Item = null;
                    itemSlot.Item.RemoveOneItem();
                    if(itemSlot.Item.count == 0)
                    {
                        Destroy(itemSlot.Item.gameObject);
                        itemSlot.Item = null;
                        savingLoading.SaveInventoryData();
                    }
                    return true;
                }
        }

        return true;
    }

    public void AddRandomBrokenItem()
    {
        int randomIndex = Random.Range(0, ItemDataAccessor.Instance.GetItemList().Count - 1);
        string randomItem = ItemDataAccessor.Instance.GetItemList()[randomIndex].Name;

        //ItemSlot emptySlot = FindFirstEmptySlot();
        //emptySlot.Item = Instantiate(ItemPrefab, emptySlot.transform).GetComponent<Item>();
        //emptySlot.Item.InitializeItem(randomItem);
        //emptySlot.Item.BreakItem();
        AddItem(randomItem, true);
        savingLoading.SaveInventoryData();
    }

    public void AddItem(string itemName, bool broken)
    {
        ItemSlot nonFullSlot = FindFirstNonFullSlotWithItem(itemName, broken);
        if (nonFullSlot != null)
        {
            nonFullSlot.Item.count++;
            nonFullSlot.Item.UpdateCountText();
            return;
        }

        ItemSlot emptySlot = FindFirstEmptySlot();
        if (emptySlot)
        {
            emptySlot.Item = Instantiate(emptySlot.itemPrefab, emptySlot.transform).GetComponent<Item>();
            emptySlot.Item.InitializeItem(itemName);
            if (broken) emptySlot.Item.BreakItem();
        }

        savingLoading.SaveInventoryData();
    }
}
