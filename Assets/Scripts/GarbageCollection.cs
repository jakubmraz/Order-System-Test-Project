using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GarbageCollection : MonoBehaviour
{
    public ItemSlot collectionSlot;
    private Items itemData;

    //This is set by the GarbageContainer when the player walks into the trigger (seemed like the easy way out)
    public string containedItem;

    void Awake()
    {
        collectionSlot = GetComponentInChildren<ItemSlot>();
        itemData = new Items();
    }

    public void RespawnItem()
    {
        collectionSlot.Item = Instantiate(collectionSlot.itemPrefab, collectionSlot.transform).GetComponent<Item>();
        collectionSlot.Item.InitializeItem(containedItem);
    }
}
