using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemCountTMP;

    public ItemData itemData;
    public bool IsBroken = false;
    public int count;

    public void InitializeItem(string item)
    {
        itemData = ItemDataAccessor.Instance.GetItemData(item);
        itemImage = GetComponent<Image>();
        itemImage.sprite = itemData.Sprite;
        count = 1;
        itemCountTMP.text = "" + count;
    }

    public void BreakItem()
    {
        IsBroken = true;
        itemImage.color = Color.red;
    }

    public void UpdateCountText()
    {
        itemCountTMP.text = "" + count;
    }

    public void RemoveOneItem()
    {
        count--;
        UpdateCountText();
    }
}
