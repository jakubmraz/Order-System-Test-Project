using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour
{
    public Item itemPrefab;

    public string debugItem;
    public Item Item;

    public bool IsCraftingSlot;
    public bool IsResultSlot;
    public bool IsCollectionSlot;

    void Awake()
    {
        if (debugItem != string.Empty)
        {
            Item = Instantiate(itemPrefab, transform).GetComponent<Item>();
            Item.InitializeItem(debugItem);
        }
    }

    public void KillTheChildren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
