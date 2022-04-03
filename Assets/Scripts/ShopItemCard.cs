using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemCard : MonoBehaviour
{
    [HideInInspector] public ShopItemSO ShopItem;

    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemPrice;
    [SerializeField] private Image cardBackground;
    [SerializeField] private Button cardAsButton;
    [SerializeField] private TextMeshProUGUI itemStock;

    public void BuyItem()
    {
        Shop.Instance.ActivateItem(ShopItem);
    }

    public void InitializeUI(ShopItemSO shopItem)
    {
        ShopItem = shopItem;
        itemImage.sprite = ShopItem.Sprite;
        itemName.text = ShopItem.Name;
        itemPrice.text = ShopItem.Price.ToString();
        UpdateUI();
    }

    public void UpdateUI()
    {
        if(ShopItem == null) return;
        
        if(Shop.Instance.CheckIfCanBuyItem(ShopItem))
        {
            cardBackground.color = GetColor(ShopItem.Kind);
            cardAsButton.enabled = true;
            itemPrice.color = new Color(255, 255, 255, 255);
        }
        else
        {
            cardBackground.color = new Color32(133, 133, 133, 255);
            cardAsButton.enabled = false;
            itemPrice.color = new Color(255, 0, 0, 255);
        }
        
        
        if(ShopItem.Kind != ShopItemSO.ItemKind.Item && ShopItem.Kind != ShopItemSO.ItemKind.Resource)
        {
            itemStock.text = "";
        }
        else 
        {
            int stock = Shop.Instance.GetStock(ShopItem.Item);
            if (stock == 0)
            {
                itemStock.text = "Restock tomorrow";
                itemStock.color = Color.red;
            }            
            else
            {
                itemStock.text = $"In stock: {stock}";
                itemStock.color = Color.black;
            }
        }
        
    }

    private Color32 GetColor(ShopItemSO.ItemKind kind)
    {
        switch(kind)
        {
            case ShopItemSO.ItemKind.Energy:
                return new Color32(252, 255, 172, 255);
            case ShopItemSO.ItemKind.Resource:
                return new Color32(172, 215, 255, 255);
            case ShopItemSO.ItemKind.Item:
                return new Color32(172, 255, 176, 255);
            case ShopItemSO.ItemKind.StorageUpgrade:
                return new Color32(255, 177, 172, 255);
            case ShopItemSO.ItemKind.ContainerUpgrade:
                return new Color32(255, 172, 240, 255);
            default:
                return new Color32(252, 255, 172, 255);
        }
    } 
}
