using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    [SerializeField] private ItemSlot topLeftSlot;
    [SerializeField] private ItemSlot topCentreSlot;
    [SerializeField] private ItemSlot topRightSlot;
    [SerializeField] private ItemSlot midLeftSlot;
    [SerializeField] private ItemSlot midCentreSlot;
    [SerializeField] private ItemSlot midRightSlot;
    [SerializeField] private ItemSlot botLeftSlot;
    [SerializeField] private ItemSlot botCentreSlot;
    [SerializeField] private ItemSlot botRightSlot;

    [SerializeField] public ItemSlot resultSlot;

    [HideInInspector] public List<ItemSlot> craftingSlots;

    public bool CraftingActive;

    void Awake()
    {
        CraftingActive = false;
        craftingSlots = new List<ItemSlot>()
        {
            topLeftSlot, topCentreSlot, topRightSlot, midLeftSlot, midCentreSlot, midRightSlot, botLeftSlot, botCentreSlot, botRightSlot
        };
    }

    public void Craft()
    {
        if(CraftingActive)
        {
            if (resultSlot.Item)
            {
                Destroy(resultSlot.Item.gameObject);
                Debug.Log("Hi");
            }
            string craftingString = GetCraftingString();
            ShowCraftableItem(craftingString);
        }
        
        
    }

    private string GetCraftingString()
    {
        string result = "";
        foreach (var craftingSlot in craftingSlots)
        {
            if (craftingSlot.Item)
                result += craftingSlot.Item.itemData.Name + ";"; 
            else
                result += "0;";

        }
        Debug.Log(result);
        return result;
    }

    private void ShowCraftableItem(string craftingString)
    {
        foreach (var item in ItemDataAccessor.Instance.GetItemList())
        {
            if (item.Recipe == craftingString)
            {
                Debug.Log("Activate.");
                Item newItem = Instantiate(resultSlot.itemPrefab, resultSlot.transform).GetComponent<Item>();
                newItem.InitializeItem(item.Name);
                resultSlot.Item = newItem;
            }
        }
    }

    public void CraftNewItem()
    {
        foreach (var slot in craftingSlots)
        {
            if (slot.Item)
                slot.Item.RemoveOneItem();
                //Destroy(slot.Item.gameObject);
        }
    }
}
