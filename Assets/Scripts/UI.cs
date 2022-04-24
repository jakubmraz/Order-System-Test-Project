using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class UI : MonoBehaviour
{
    public static UI Instance { get; private set; }

    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform craftingGrid;
    [SerializeField] private Button inventoryButton;

    [SerializeField] private RectTransform nakedInventory;
    [SerializeField] private CraftingSystem craftingSystem;
    [SerializeField] private RectTransform garbageCollectionScreen;
    [SerializeField] private GarbageCollection garbageCollection;
    [SerializeField] private RectTransform recyclingSystem;
    [SerializeField] private RectTransform storageScreen;
    [SerializeField] private RectTransform storeScreen;
    [SerializeField] private RectTransform missionScreen;
    [SerializeField] private ItemDepository depositoryScreen;

    [SerializeField] private RectTransform winPanel;
    [SerializeField] private RectTransform lossPanel;

    [SerializeField] private OrderScreen orderScreen;

    public List<ItemSlot> InventoryScreenItemSlots;
    public List<ItemSlot> CraftingScreenItemSlots;
    public List<ItemSlot> CollectionScreenItemSlots;
    public List<ItemSlot> RecyclingScreenItemSlots;
    public List<ItemSlot> DespositoryItemSlots;

    private SavingLoading savingLoading;
    private RealTimeEffects realTimeEffects;
    private Inventory inventory;

    void Awake()
    {
        Instance = this;

        savingLoading = GetComponent<SavingLoading>();
        realTimeEffects = FindObjectOfType<RealTimeEffects>();
        inventory = GetComponentInParent<Inventory>();
        inventory.AssignNewInventorySlots(InventoryScreenItemSlots);
        inventory.LoadSavedInventory();
    }

    public void ShowCollectionScreen()
    {
        inventoryButton.gameObject.SetActive(false);
        background.gameObject.SetActive(true);
        garbageCollectionScreen.gameObject.SetActive(true);
        garbageCollection.gameObject.SetActive(true);
        inventory.AssignNewInventorySlots(CollectionScreenItemSlots);
        garbageCollection.SpawnItem();
    }

    public void ShowCraftingScreen()
    {
        inventoryButton.gameObject.SetActive(false);
        background.gameObject.SetActive(true);
        craftingGrid.gameObject.SetActive(true);
        inventory.AssignNewInventorySlots(CraftingScreenItemSlots);
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
        //savingLoading.SaveContainerData(realTimeEffects.GarbageContainers);

        CloseAll();
    }

    public void OpenInventory()
    {
        inventory.AssignNewInventorySlots(InventoryScreenItemSlots);
        inventoryButton.gameObject.SetActive(false);
        background.gameObject.SetActive(true);
        nakedInventory.gameObject.SetActive(true);
    }

    private void CloseAll()
    {
        background.gameObject.SetActive(false);
        nakedInventory.gameObject.SetActive(false);
        craftingGrid.gameObject.SetActive(false);
        inventoryButton.gameObject.SetActive(true);
        garbageCollection.gameObject.SetActive(false);
        garbageCollectionScreen.gameObject.SetActive(false);
        recyclingSystem.gameObject.SetActive(false);
        storageScreen.gameObject.SetActive(false);
        storeScreen.gameObject.SetActive(false);
        missionScreen.gameObject.SetActive(false);
        depositoryScreen.OnClosed();
        depositoryScreen.gameObject.SetActive(false);        
    }

    public void ShowMissionScreen()
    {
        inventoryButton.gameObject.SetActive(false);
        background.gameObject.SetActive(true);
        missionScreen.gameObject.SetActive(true);
        MissionSystem.Instance.OnUIOpened();
    }

    public void ShowShopScreen()
    {
        inventoryButton.gameObject.SetActive(false);
        background.gameObject.SetActive(true);
        storeScreen.gameObject.SetActive(true);
        //Shop.Instance.ShowTab("Energy");
        Shop.Instance.UpdateShopCards();
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
        OrderSystem.Instance.CheckPlayerInventory();
    }

    public void ShowRecyclingScreen()
    {
        inventoryButton.gameObject.SetActive(false);
        background.gameObject.SetActive(true);
        recyclingSystem.gameObject.SetActive(true);
        inventory.AssignNewInventorySlots(RecyclingScreenItemSlots);
    }

    public void ShowStorageScreen()
    {
        inventoryButton.gameObject.SetActive(false);
        background.gameObject.SetActive(true);
        storageScreen.gameObject.SetActive(true);
        inventory.AssignNewInventorySlots(CollectionScreenItemSlots);
        Storage.Instance.UpdateCardCounts();
    }

    public void CloseCollectionScreen()
    {
        garbageCollection.collectionSlot.KillTheChildren();
        savingLoading.SaveInventoryData();
        garbageCollection.gameObject.SetActive(false);
        garbageCollectionScreen.gameObject.SetActive(false);
    }

    public void ShowDepositoryScreen()
    {
        inventoryButton.gameObject.SetActive(false);
        background.gameObject.SetActive(true);
        depositoryScreen.gameObject.SetActive(true);
        inventory.AssignNewInventorySlots(DespositoryItemSlots);
        depositoryScreen.OnOpened();
    }
}
