using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StorageItemCard : MonoBehaviour
{
    public ItemData item;

    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private Button returnButton;
    [SerializeField] private Button takeButton;
    [SerializeField] private Slider itemCountSlider;

    private int numberCount;

    public void UpdateButtons()
    {
        if (Inventory.Instance.FindFirstEmptySlot() == null || numberCount == 0) takeButton.enabled = false;
        else takeButton.enabled = true;

        if (Inventory.Instance.CheckForItem(item.Name) && numberCount < Storage.Instance.MaxStorageSpace ) returnButton.enabled = true;
        else returnButton.enabled = false;
    }

    public void UpdateItemCard(ItemData itemData, int itemCount)
    {
        item = itemData;

        numberCount = itemCount;

        this.itemName.text = itemData.Name;
        this.itemImage.sprite = itemData.Sprite;
        this.itemCount.text = itemCount + "/" + Storage.Instance.MaxStorageSpace;
        this.itemCountSlider.value = itemCount;
        this.itemCountSlider.maxValue = Storage.Instance.MaxStorageSpace;
    }

    public void UpdateItemCount(int itemCount)
    {
        numberCount = itemCount;

        this.itemCount.text = itemCount + "/" + Storage.Instance.MaxStorageSpace;
        this.itemCountSlider.value = itemCount;
        this.itemCountSlider.maxValue = Storage.Instance.MaxStorageSpace;
    }

    public void OpenCollection()
    {
        Storage.Instance.OpenResourceCollection(item.Name);
    }

    public string GetItem()
    {
        return item.Name;
    }

    public void TakeItem()
    {
        ItemSlot slot = Inventory.Instance.FindFirstEmptySlot();
        if (slot)
        {
            slot.Item = Instantiate(slot.itemPrefab, slot.transform).GetComponent<Item>();
            slot.Item.InitializeItem(item.Name);
        }

        Storage.Instance.itemValues[item.Name]--;
        Storage.Instance.UpdateCardCounts();
    }

    public void ReturnItem()
    {
        if(Inventory.Instance.CheckForItem(item.Name))
        {
            Inventory.Instance.RemoveItem(item.Name);
            Storage.Instance.itemValues[item.Name]++;
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
