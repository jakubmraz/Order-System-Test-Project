using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopLayout : MonoBehaviour
{
    public void GetAllShopItems(out List<ShopItemSO> shopItems, out List<ShopItemCard> shopItemCards)
    {
        shopItemCards = GetComponentsInChildren<ShopItemCard>().ToList();

        shopItems = new List<ShopItemSO>();
        foreach(ShopItemCard card in shopItemCards)
        {
            shopItems.Add(card.ShopItem);
        }
    }
}
