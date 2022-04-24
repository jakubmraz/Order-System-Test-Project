using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDepository : MonoBehaviour
{
    [SerializeField] private List<ItemSlot> depositorySlots;
    [SerializeField] private ItemSlot trashSlot;
    [SerializeField] private Item itemPrefab;

    private const int ItemRemovalPrice = 30;

    public void OnOpened()
    {
        FillDepository();
    }

    public void OnClosed()
    {
        PlayerPrefs.SetString("Depository", GetDepositoryString());
        PlayerPrefs.SetString("DepositoryNumbers", GetDepositoryNumbersString());
        PlayerPrefs.Save();

        SavingLoading.Instance.SaveInventoryData();
        if(trashSlot.Item != null)
        {
            Inventory.Instance.AddItem(trashSlot.Item.itemData.Name, trashSlot.Item.IsBroken);
            Destroy(trashSlot.Item.gameObject);
            trashSlot.Item = null;
        }
    }

    public void DestroyItem()
    {
        if(trashSlot.Item == null) return;
        
        Destroy(trashSlot.Item.gameObject);
        trashSlot.Item = null;

        //I don't think this should get disabled if they don't have the money, it will only charge money if they do have it
        if(PlayerValues.Instance.CheckMoney(ItemRemovalPrice))
            PlayerValues.Instance.PlayerMoney -= ItemRemovalPrice;
    }

    private void FillDepository()
    {
        if(!LoadSavedDepository(out string loadedItems, out string loadedNumbers)) return;

        string[] splitString = loadedItems.Split(';');

        bool isBroken = false;
        for (int i = 0; i < depositorySlots.Count; i++)
        {
            if (splitString[i] != "0" && splitString[i] != "")
            {
                if (splitString[i].Contains("!"))
                {
                    splitString[i] = splitString[i].Replace("!", "");
                    isBroken = true;
                }
                depositorySlots[i].Item = Instantiate(itemPrefab, depositorySlots[i].transform).GetComponent<Item>();
                depositorySlots[i].Item.InitializeItem(splitString[i]);
                if(isBroken)
                    depositorySlots[i].Item.BreakItem();
            }
            isBroken = false;
        }

        splitString = loadedNumbers.Split(';');

        for (int i = 0; i < depositorySlots.Count; i++)
        {
            if (splitString[i] != "0" && splitString[i] != "")
            {
                depositorySlots[i].Item.count = Convert.ToInt32(splitString[i]);
                depositorySlots[i].Item.UpdateCountText();
            }
        }

    }

    private bool LoadSavedDepository(out string loadedItems, out string loadedNumbers)
    {
        loadedItems = "";
        loadedNumbers = "";

        if(!PlayerPrefs.HasKey("Depository")) return false;
        if(!PlayerPrefs.HasKey("DepositoryNumbers")) return false;

        loadedItems = PlayerPrefs.GetString("Depository");
        loadedNumbers = PlayerPrefs.GetString("DepositoryNumbers");

        return true;
    }

    private string GetDepositoryString()
    {
        string inventoryString = "";

        foreach (var itemSlot in depositorySlots)
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

    private string GetDepositoryNumbersString()
    {
        string inventoryString = "";

        foreach (var itemSlot in depositorySlots)
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
}
