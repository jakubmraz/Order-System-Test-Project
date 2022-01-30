using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StorageItemCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private Button returnButton;
    [SerializeField] private Button takeButton;

    public void UpdateButtons()
    {
        if (Inventory.Instance.FindFirstEmptySlot() == null || itemCount.text == "0") takeButton.enabled = false;
        else takeButton.enabled = true;

        if (Inventory.Instance.CheckForItem(itemName.text)) returnButton.enabled = true;
        else returnButton.enabled = false;
    }

    public void UpdateItemCard(string itemName, Sprite itemImage, int itemCount)
    {
        this.itemName.text = itemName;
        this.itemImage.sprite = itemImage;
        this.itemCount.text = itemCount.ToString();
    }

    public void UpdateItemCount(int itemCount)
    {
        this.itemCount.text = itemCount.ToString();
    }

    public void OpenCollection()
    {
        Storage.Instance.OpenResourceCollection(itemName.text);
    }

    public string GetItem()
    {
        return itemName.text;
    }

    public void TakeItem()
    {
        ItemSlot slot = Inventory.Instance.FindFirstEmptySlot();
        if (slot)
        {
            slot.Item = Instantiate(slot.itemPrefab, slot.transform).GetComponent<Item>();
            slot.Item.InitializeItem(itemName.text);
        }

        Storage.Instance.itemValues[itemName.text]--;
        Storage.Instance.UpdateCardCounts();
    }

    public void ReturnItem()
    {
        if(Inventory.Instance.CheckForItem(itemName.text))
        {
            Inventory.Instance.RemoveItem(itemName.text);
            Storage.Instance.itemValues[itemName.text]++;
        }
        Storage.Instance.UpdateCardCounts();
    }

    //private Color GetRandomPastelColor()
    //{
    //    int i = Random.Range(1, 12);

    //    switch (i)
    //    {
    //        case 1:
    //            return new Color(172, 234, 255);
    //        case 2:
    //            return new Color(172, 255, 252);
    //        case 3:
    //            return new Color(172, 255, 175);
    //        case 4:
    //            return new Color(220, 255, 172);
    //        case 5:
    //            return new Color(252, 255, 172);
    //        case 6:
    //            return new Color(255, 226, 172);
    //        case 7:
    //            return new Color(255, 175, 172);
    //        case 8:
    //            return new Color(255, 172, 231);
    //        case 9:
    //            return new Color(225, 172, 255);
    //        case 10:
    //            return new Color(172, 173, 255);
    //        case 11:
    //            return new Color(172, 201, 255);
    //        case 12:
    //            return new Color(255, 172, 194);
    //        default:
    //            return new Color(172, 234, 255);
    //    }
    //}
}
