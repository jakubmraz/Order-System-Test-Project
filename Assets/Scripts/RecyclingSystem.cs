using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RecyclingSystem : MonoBehaviour
{
    [SerializeField] private ItemSlot entrySlot;
    [SerializeField] private ItemSlot topLeftSlot;
    [SerializeField] private ItemSlot topCentreSlot;
    [SerializeField] private ItemSlot topRightSlot;
    [SerializeField] private ItemSlot midLeftSlot;
    [SerializeField] private ItemSlot midCentreSlot;
    [SerializeField] private ItemSlot midRightSlot;
    [SerializeField] private ItemSlot botLeftSlot;
    [SerializeField] private ItemSlot botCentreSlot;
    [SerializeField] private ItemSlot botRightSlot;

    [SerializeField] private Button recycleButton;

    private List<ItemSlot> resultSlots;

    //How many items will be missing from the original recipe
    private int recyclingPenalty = 2;

    void Awake()
    {
        recycleButton.enabled = false;
        resultSlots = new List<ItemSlot>()
        {
            topLeftSlot, topCentreSlot, topRightSlot, midLeftSlot, midCentreSlot, midRightSlot, botLeftSlot, botCentreSlot, botRightSlot
        };
    }

    public void Recycle()
    {
        if (entrySlot.Item == null)
            return;

        Item itemToBeRecycled = entrySlot.Item;
        string recipe = ItemDataAccessor.Instance.GetItemList().FirstOrDefault(item => itemToBeRecycled.itemData.Name == item.Name).Recipe;

        string[] splitString = recipe.Split(';');
        int i = 0;
        int itemsPenalised = 0;

        foreach (var itemSlot in resultSlots)
        {
            if (splitString[i] != "0")
            {
                if (itemsPenalised < recyclingPenalty)
                {
                    if (Random.Range(0, 2) == 1)
                    {
                        itemsPenalised++;
                        i++;
                        continue;
                    }
                } 
                itemSlot.Item = Instantiate(itemSlot.itemPrefab, itemSlot.transform).GetComponent<Item>();
                itemSlot.Item.InitializeItem(splitString[i]);
            }

            i++;
        }

        entrySlot.Item.RemoveOneItem();
        if (entrySlot.Item.count == 0)
        {
            Destroy(entrySlot.Item.gameObject);
            entrySlot.Item = null;
        }

        DisableRecycleButton();

        //if(PlayerPrefs.HasKey("Recycled")){
        //    if(PlayerPrefs.GetInt("Recycled") == 1) return;           
        //}
        //PlayerPrefs.SetInt("Recycled", 1);
        //PlayerPrefs.Save();
    }

    public bool CheckIfResultSlotsEmpty()
    {
        foreach (var slot in resultSlots)
        {
            if (slot.Item != null)
                return false;
        }

        return true;
    }

    public bool CheckIfEntrySlotFilled()
    {
        if (entrySlot.Item == null) return false;
        else return true;
    }

    public void EnableRecycleButton()
    {
        recycleButton.enabled = true;
    }

    public void DisableRecycleButton()
    {
        recycleButton.enabled = false;
    }
}
