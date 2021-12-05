using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class UI : MonoBehaviour
{
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform craftingGrid;
    [SerializeField] private Button inventoryButton;

    [SerializeField] private CraftingSystem craftingSystem;
    [SerializeField] private GarbageCollection garbageCollection;
    [SerializeField] private RectTransform recyclingSystem;

    [SerializeField] private RectTransform winPanel;
    [SerializeField] private RectTransform lossPanel;

    [SerializeField] private OrderScreen orderScreen;

    private SavingLoading savingLoading;
    private RealTimeEffects realTimeEffects;

    void Awake()
    {
        savingLoading = GetComponent<SavingLoading>();
        realTimeEffects = FindObjectOfType<RealTimeEffects>();
    }

    public void ShowCollectionScreen()
    {
        inventoryButton.gameObject.SetActive(false);
        background.gameObject.SetActive(true);
        garbageCollection.gameObject.SetActive(true);
        garbageCollection.SpawnItem();
    }

    public void ShowCraftingScreen()
    {
        inventoryButton.gameObject.SetActive(false);
        background.gameObject.SetActive(true);
        craftingGrid.gameObject.SetActive(true);
        craftingSystem.CraftingActive = true;
    }

    public void CloseCraftingScreen()
    {
        craftingSystem.CraftingActive = false;
        Inventory inventory = GetComponent<Inventory>();
        foreach (var slot in craftingSystem.craftingSlots)
        {
            if (slot.Item)
            {
                ItemSlot emptySlot = inventory.FindFirstEmptySlot();
                slot.Item.transform.SetParent(emptySlot.transform);
                emptySlot.Item = slot.Item;
                emptySlot.Item.transform.localPosition = new Vector3(0, 0, 0);
                DragDrop2 dragDrop2 = emptySlot.Item.GetComponent<DragDrop2>();
                dragDrop2.currentSlot = emptySlot;
                slot.Item = null;
            }
            //put all items back into inventory
        }

        if (craftingSystem.resultSlot.Item)
        {
            Destroy(craftingSystem.resultSlot.Item.gameObject);
            craftingSystem.resultSlot.Item = null;
        }

        if (garbageCollection.collectionSlot)
        {
            garbageCollection.collectionSlot.KillTheChildren();
        }

        savingLoading.SaveInventoryData();
        savingLoading.SaveContainerData(realTimeEffects.GarbageContainers);

        CloseAll();
    }

    public void OpenInventory()
    {
        inventoryButton.gameObject.SetActive(false);
        background.gameObject.SetActive(true);
    }

    private void CloseAll()
    {
        background.gameObject.SetActive(false);
        craftingGrid.gameObject.SetActive(false);
        inventoryButton.gameObject.SetActive(true);
        garbageCollection.gameObject.SetActive(false);
        recyclingSystem.gameObject.SetActive(false);
    }

    public void ShowVictoryScreen()
    {
        winPanel.gameObject.SetActive(true);
    }

    public void ShowLossScreen()
    {
        lossPanel.gameObject.SetActive(true);
    }

    public void ShowOrderScreen()
    {
        orderScreen.gameObject.SetActive(true);
        orderScreen.OnOrderCompleted();
    }

    public void ShowRecyclingScreen()
    {
        inventoryButton.gameObject.SetActive(false);
        background.gameObject.SetActive(true);
        recyclingSystem.gameObject.SetActive(true);
    }
}
