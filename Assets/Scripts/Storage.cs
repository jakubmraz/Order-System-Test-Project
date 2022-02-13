using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public static Storage Instance { get; private set; }

    private List<StorageItemCard> itemCards;
    public Dictionary<string, int> itemValues;
    public Dictionary<string, int> itemCapacities;
    [SerializeField] private RectTransform storageGridLayout;
    [SerializeField] private StorageItemCard itemCardPrefab;

    public int BaseStorageSpace = 50;

    void Start()
    {
        Instance = this;

        itemCards = new List<StorageItemCard>();
        itemValues = new Dictionary<string, int>();
        itemCapacities = new Dictionary<string, int>();

        if(SavingLoading.Instance.LoadStorageCapacity(out Dictionary<string, int> capacities))
        {
            itemCapacities = capacities;
        }
        else
        {
            foreach (var item in ItemDataAccessor.Instance.GetBaseItemList())
            {
                itemCapacities.Add(item.Name, BaseStorageSpace);
            }
        }

        Dictionary<string, int> loadedValues = SavingLoading.Instance.LoadStorageData();

        if (loadedValues != null)
        {
            foreach (var item in ItemDataAccessor.Instance.GetBaseItemList())
            {
                StorageItemCard itemCard = Instantiate(itemCardPrefab, storageGridLayout).GetComponent<StorageItemCard>();

                if (loadedValues.TryGetValue(item.Name, out int value))
                {
                    itemValues.Add(item.Name, value);
                }
                else
                {
                    value = 0;
                    itemValues.Add(item.Name, value);
                }

                itemCards.Add(itemCard);
                itemCard.UpdateItemCard(item, value, GetItemCapacity(item.Name));
            }
        }
        else
        {
            foreach (var item in ItemDataAccessor.Instance.GetBaseItemList())
            {
                StorageItemCard itemCard = Instantiate(itemCardPrefab, storageGridLayout).GetComponent<StorageItemCard>();
                itemValues.Add(item.Name, 10);
                itemCards.Add(itemCard);
                itemCard.UpdateItemCard(item, 10, GetItemCapacity(item.Name));
            }
        }
    }

    public void OpenResourceCollection(string itemName)
    {
        itemValues.TryGetValue(itemName, out int itemCount);

        try
        {
            GarbageCollection.Instance.containedItem = itemName;
            GarbageCollection.Instance.itemCount = itemCount;
            UI.Instance.ShowCollectionScreen();
        }
        catch
        {
            UI.Instance.ShowCollectionScreen();
            GarbageCollection.Instance.containedItem = itemName;
            GarbageCollection.Instance.itemCount = itemCount;
            UI.Instance.CloseCollectionScreen();
            UI.Instance.ShowCollectionScreen();
        }
    }

    public void UpdateCardCounts()
    {
        foreach (var card in itemCards)
        {
            int count = itemValues[card.GetItem()];
            card.UpdateItemCount(count);
            int cap = itemCapacities[card.GetItem()];
            card.UpdateItemCapacity(cap);
            card.UpdateButtons();
        }
        SavingLoading.Instance.SaveStorageData(itemValues);
    }

    public void FillItem(string itemName, int amount)
    {
        itemValues[itemName] += amount;
        UpdateCardCounts();
    }

    public int GetItemCount(string itemName)
    {
        return itemValues[itemName];
    }

    public int GetItemCapacity(string itemName)
    {
        return itemCapacities[itemName];
    }

    public void ChangeItemCapacity(string itemName, int value)
    {
        itemCapacities[itemName] = value;
        SavingLoading.Instance.SaveStorageCapacity(itemCapacities);
        UpdateCardCounts();
    }

    public void IncreaseItemCapacity(string itemName, int value)
    {
        itemCapacities[itemName] += value;
        SavingLoading.Instance.SaveStorageCapacity(itemCapacities);
        UpdateCardCounts();
    }
}
