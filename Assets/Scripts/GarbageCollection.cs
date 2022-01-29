using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class GarbageCollection : MonoBehaviour
{
    public static GarbageCollection Instance { get; private set; }

    [HideInInspector] public ItemSlot collectionSlot;
    public TextMeshProUGUI itemCountUI;

    //This is set by the GarbageContainer when the player walks into the trigger (seemed like the easy way out)
    [HideInInspector] public string containedItem;
    [HideInInspector] public int itemCount;
    //[HideInInspector] public GarbageContainer currentContainer;

    void Awake()
    {
        Instance = this;

        collectionSlot = GetComponentInChildren<ItemSlot>();
    }

    public void RespawnItem()
    {
        if (itemCount > 1)
        {
            collectionSlot.Item = Instantiate(collectionSlot.itemPrefab, collectionSlot.transform).GetComponent<Item>();
            collectionSlot.Item.InitializeItem(containedItem);
            itemCount--;
            Storage.Instance.itemValues[containedItem]--;
        }
        else if (itemCount == 1)
        {
            itemCount--;
            Storage.Instance.itemValues[containedItem]--;
        }

        itemCountUI.text = "x" + itemCount;
        Storage.Instance.UpdateCardCounts();
        SavingLoading.Instance.SaveStorageData(Storage.Instance.itemValues);
    }

    public void SpawnItem()
    {
        if (itemCount >= 1)
        {
            collectionSlot.Item = Instantiate(collectionSlot.itemPrefab, collectionSlot.transform).GetComponent<Item>();
            collectionSlot.Item.InitializeItem(containedItem);
        }

        itemCountUI.text = "x" + itemCount;
        Storage.Instance.UpdateCardCounts();
    }
}
