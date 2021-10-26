using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class GarbageCollection : MonoBehaviour
{
    [HideInInspector] public ItemSlot collectionSlot;
    [HideInInspector] private Items itemData;
    public TextMeshProUGUI itemCountUI;

    //This is set by the GarbageContainer when the player walks into the trigger (seemed like the easy way out)
    [HideInInspector] public string containedItem;
    [HideInInspector] public int itemCount;
    [HideInInspector] public GarbageContainer currentContainer;

    void Awake()
    {
        collectionSlot = GetComponentInChildren<ItemSlot>();
        itemData = new Items();
    }

    public void RespawnItem()
    {
        if (itemCount > 1)
        {
            collectionSlot.Item = Instantiate(collectionSlot.itemPrefab, collectionSlot.transform).GetComponent<Item>();
            collectionSlot.Item.InitializeItem(containedItem);
            itemCount--;
            currentContainer.itemCount--;
        }
        else if (itemCount == 1)
        {
            itemCount--;
            currentContainer.itemCount--;
        }

        itemCountUI.text = "x" + itemCount;
    }

    public void SpawnItem()
    {
        if (itemCount > 1)
        {
            collectionSlot.Item = Instantiate(collectionSlot.itemPrefab, collectionSlot.transform).GetComponent<Item>();
            collectionSlot.Item.InitializeItem(containedItem);
        }

        itemCountUI.text = "x" + itemCount;
    }
}
