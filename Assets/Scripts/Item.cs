using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData itemData;
    private Image itemImage;

    public void InitializeItem(string item)
    {
        Items items = new Items();
        itemData = items.GetItemData(item);
        itemImage = GetComponent<Image>();
        itemImage.sprite = itemData.Sprite;
    }
}
