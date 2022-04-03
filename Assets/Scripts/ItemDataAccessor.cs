using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDataAccessor : MonoBehaviour
{
    public static ItemDataAccessor Instance { get; private set; }
    [SerializeField] private Items items;

    void Awake()
    {
        Instance = this;
    }

    public ItemData GetItemData(string itemName)
    {
        List<ItemData> allItems = items.ItemList.Concat(items.BaseItemList).ToList();
        return allItems.FirstOrDefault(item => item.Name == itemName);
    }

    public List<ItemData> GetItemList()
    {
        return items.ItemList;
    }

    public List<ItemData> GetBaseItemList()
    {
        return items.BaseItemList;
    }

    public List<ItemData> GetFullItemList()
    {
        return items.ItemList.Concat(items.BaseItemList).ToList();
    }
}
