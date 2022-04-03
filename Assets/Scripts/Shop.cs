using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    public static Shop Instance {get; private set;}

    [SerializeField] private List<ShopItemSO> shopItems;

    private List<ShopItemCard> itemCards;
    private List<RectTransform> cardContainers;

    private Dictionary<ItemData, int> itemStock;

    private const int ResourceStockSize = 20;
    private const int ItemStockSize = 5;

    //UI
    [SerializeField] private ShopItemCard itemCardPrefab;
    [SerializeField] private RectTransform layoutGroupPrefab;
    [SerializeField] private RectTransform emptyObjectPrefab;
    [SerializeField] private RectTransform shopGridLayout;
    [SerializeField] private TextMeshProUGUI EnergyTabButton;
    [SerializeField] private TextMeshProUGUI ItemTabButton;
    [SerializeField] private TextMeshProUGUI ResourceTabButton;
    [SerializeField] private TextMeshProUGUI StorageTabButton;
    [SerializeField] private TextMeshProUGUI ContainerTabButton;

    public void Start()
    {
        Instance = this;
        itemCards = new List<ShopItemCard>();
        cardContainers = new List<RectTransform>();
        itemStock = new Dictionary<ItemData, int>();

        foreach(ShopItemSO item in shopItems){
            ShopItemCard itemCard = Instantiate(itemCardPrefab, shopGridLayout).GetComponent<ShopItemCard>();
            itemCards.Add(itemCard);
            if(item.Kind == ShopItemSO.ItemKind.Item || item.Kind == ShopItemSO.ItemKind.Resource) itemStock.Add(item.Item, 0);
            itemCard.InitializeUI(item);
        }

        if(LoadShopStock(out Dictionary<ItemData, int> loadedStock) && !IsNewDay())
        {
            itemStock = loadedStock;
        }
        else
        {
            foreach(ItemData item in itemStock.Keys.ToList())
            {
                if(item.Final)
                    itemStock[item] = ItemStockSize;
                
                else
                    itemStock[item] = ResourceStockSize;
            }
        }
    }

    public void Update(){
        UpdateShopCards();
    }

    //Prequisites for item appearing in the shop
    public bool CheckIfItemAppears(ShopItemSO shopItem){
        if (shopItem.Item.Level > PlayerValues.Instance.PlayerLevel) return false;

        return true;
    }

    //Prequisites for buy button being clickable
    public bool CheckIfCanBuyItem(ShopItemSO shopItem)
    {
        if (!PlayerValues.Instance.CheckMoney(shopItem.Price)) return false;
        if (shopItem.Kind != ShopItemSO.ItemKind.Energy)
            if (shopItem.Item.Level > PlayerValues.Instance.PlayerLevel) return false;
        
        if(shopItem.Kind == ShopItemSO.ItemKind.Resource || shopItem.Kind == ShopItemSO.ItemKind.Item)
            if(GetStock(shopItem.Item) == 0) return false;   
                     
        return true;
    }

    public void ActivateItem(ShopItemSO shopItem)
    {
        PlayerValues.Instance.PlayerMoney -= shopItem.Price;

        switch(shopItem.Kind)
        {
            case ShopItemSO.ItemKind.Energy:
                PlayerValues.Instance.AddEnergy(shopItem.AmountAdded);
                break;
            case ShopItemSO.ItemKind.Resource:
                for(int i = 0; i < shopItem.AmountAdded; i++){
                    Inventory.Instance.AddItem(shopItem.Item.Name, false);
                }                
                break;
            case ShopItemSO.ItemKind.Item:
                for(int i = 0; i < shopItem.AmountAdded; i++){
                    Inventory.Instance.AddItem(shopItem.Item.Name, false);
                }
                break;
            case ShopItemSO.ItemKind.StorageUpgrade:
                Storage.Instance.IncreaseItemCapacity(shopItem.Item.Name, shopItem.AmountAdded);
                break;
            case ShopItemSO.ItemKind.ContainerUpgrade:
                ContainerSystem.Instance.IncreaseContainerCapacity(shopItem.Item.Name, shopItem.AmountAdded);
                break;
        }

        foreach(ShopItemCard card in itemCards)
        {
            card.UpdateUI();
        }

        if(GetStock(shopItem.Item) != -1)
        {
            itemStock[shopItem.Item]--;
        }

        SaveShopStock();
    }

    public List<ShopItemSO> GetShopItems(ShopItemSO.ItemKind kind){
        return (from item in shopItems where item.Kind == kind select item).ToList();
    }

    public int GetStock(ItemData item)
    {
        return itemStock[item];
    }

    private void SaveShopStock()
    {
        string storageString = "";

        foreach (var pair in itemStock)
        {
            storageString += $"{pair.Key.Name}#{pair.Value};";
        }
        PlayerPrefs.SetString("ShopStock", storageString);
        PlayerPrefs.SetString("StockDate", DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    private bool LoadShopStock(out Dictionary<ItemData, int> stock)
    {
        stock = new Dictionary<ItemData, int>();

        if (!PlayerPrefs.HasKey("ShopStock")) return false;
        
        string stockString = PlayerPrefs.GetString("ShopStock");
        string[] pairs = stockString.Split(';');
        foreach (var pair in pairs)
        {
            if (pair.Contains("#"))
            {
                string[] values = pair.Split('#');
                stock.Add(ItemDataAccessor.Instance.GetItemData(values[0]), Convert.ToInt32(values[1]));
            }
        }

        return true;        
    }

    private bool IsNewDay()
    {
        if(!PlayerPrefs.HasKey("StockDate")) return true;
        
        DateTime lastStockUpdate = DateTime.Parse(PlayerPrefs.GetString("StockDate"));
        if((lastStockUpdate.Day < DateTime.Now.Day && lastStockUpdate.Month == DateTime.Now.Month)
            || (lastStockUpdate.Month < DateTime.Now.Month))
            return true;
        
        return false;
    } 

    #region UI
    public void UpdateShopCards()
    {
        foreach(ShopItemCard card in itemCards)
        {
            card.UpdateUI();
        }
    }

    public void ShowTab(string kind)
    {
        switch(kind)
        {
            default:
                SwitchTabButtonColour("Energy");
                FillShopItems(ShopItemSO.ItemKind.Energy);
                break;
            case "Energy":
                SwitchTabButtonColour("Energy");
                FillShopItems(ShopItemSO.ItemKind.Energy);
                break;
            case "Resources":
                SwitchTabButtonColour("Resources");
                FillShopItems(ShopItemSO.ItemKind.Resource);
                break;
            case "Items":
                SwitchTabButtonColour("Items");
                FillShopItems(ShopItemSO.ItemKind.Item);
                break;
            case "Storage":
                SwitchTabButtonColour("Storage");
                FillShopItems(ShopItemSO.ItemKind.StorageUpgrade);
                break;
            case "Container":
                SwitchTabButtonColour("Container");
                FillShopItems(ShopItemSO.ItemKind.ContainerUpgrade);
                break;
        }
    }

    private void FillShopItems(ShopItemSO.ItemKind kind)
    {
        foreach(ShopItemCard card in itemCards)
        {
            Destroy(card.gameObject);
        }
        foreach(RectTransform container in cardContainers)
        {
            Destroy(container.gameObject);
        }

        itemCards = new List<ShopItemCard>();
        cardContainers = new List<RectTransform>();

        IEnumerable<ShopItemSO> items = from data in shopItems
                                            where data.Kind == kind
                                            orderby data.Price
                                            select data;
        List<ShopItemSO> itemsList = items.ToList<ShopItemSO>();

        RectTransform lastLayout = new RectTransform();

        for(int i = 0; i < itemsList.Count(); i++)
        {
            if(i % 2 == 0 || itemsList[i].Big)
            {
                lastLayout = Instantiate(layoutGroupPrefab, shopGridLayout).GetComponent<RectTransform>();
                cardContainers.Add(lastLayout);
                ShopItemCard itemCard = Instantiate(itemCardPrefab, lastLayout).GetComponent<ShopItemCard>();
                itemCard.InitializeUI(itemsList[i]);
                itemCards.Add(itemCard);
            }
            else if (!itemsList[i].Big)
            {
                ShopItemCard itemCard = Instantiate(itemCardPrefab, lastLayout).GetComponent<ShopItemCard>();
                itemCard.InitializeUI(itemsList[i]);
                itemCards.Add(itemCard);
            }

            if(i != itemsList.Count() - 1)
                if(itemsList[i+1].Big)
                    Instantiate(emptyObjectPrefab, lastLayout);

            if(i == itemsList.Count() - 1 && i % 2 == 0 && !itemsList[i].Big)
                Instantiate(emptyObjectPrefab, lastLayout);
            
        }
    }

    private void SwitchTabButtonColour(string tab)
    {
        switch(tab)
        {
            default:
                EnergyTabButton.color = Color.yellow;
                ResourceTabButton.color = Color.white;
                ItemTabButton.color = Color.white;
                StorageTabButton.color = Color.white;
                ContainerTabButton.color = Color.white;
                break;
            case "Energy":
                EnergyTabButton.color = Color.yellow;
                ResourceTabButton.color = Color.white;
                ItemTabButton.color = Color.white;
                StorageTabButton.color = Color.white;
                ContainerTabButton.color = Color.white;
                break;
            case "Resources":
                EnergyTabButton.color = Color.white;
                ResourceTabButton.color = Color.yellow;
                ItemTabButton.color = Color.white;
                StorageTabButton.color = Color.white;
                ContainerTabButton.color = Color.white;
                break;
            case "Items":
                EnergyTabButton.color = Color.white;
                ResourceTabButton.color = Color.white;
                ItemTabButton.color = Color.yellow;
                StorageTabButton.color = Color.white;
                ContainerTabButton.color = Color.white;
                break;
            case "Storage":
                EnergyTabButton.color = Color.white;
                ResourceTabButton.color = Color.white;
                ItemTabButton.color = Color.white;
                StorageTabButton.color = Color.yellow;
                ContainerTabButton.color = Color.white;
                break;
            case "Container":
                EnergyTabButton.color = Color.white;
                ResourceTabButton.color = Color.white;
                ItemTabButton.color = Color.white;
                StorageTabButton.color = Color.white;
                ContainerTabButton.color = Color.yellow;
                break;
        }
    }
    #endregion
}
