using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Order : MonoBehaviour
{
    [SerializeField] private Image desiredItem1Image;
    [SerializeField] private Image desiredItem2Image;
    [SerializeField] private Button deliverButton;

    private string desiredItem1;
    private string desiredItem2;

    private Inventory inventory;

    void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        PickDesiredItems();
        CheckPlayerInventory();
    }

    void PickDesiredItems()
    {
        Items items = new Items();
        int selection = Random.Range(0, items.ItemList.Count);
        desiredItem1 = items.ItemList[selection].Name;
        desiredItem1Image.sprite = items.ItemList.FirstOrDefault(item => item.Name == desiredItem1).Sprite;

        if (Random.Range(1, 10) <= 3)
        {
            selection = Random.Range(0, items.ItemList.Count);
            desiredItem2 = items.ItemList[selection].Name;
            desiredItem2Image.sprite = items.ItemList.FirstOrDefault(item => item.Name == desiredItem2).Sprite;
        }
        else
        {
            desiredItem2 = "";
            Color tempColour = desiredItem2Image.color;
            tempColour.a = 0;
            desiredItem2Image.color = tempColour;
        }
    }

    public void CheckPlayerInventory()
    {
        deliverButton.gameObject.SetActive(false);

        if (inventory.CheckForItem(desiredItem1))
            if (desiredItem1 == desiredItem2)
            {
                if (inventory.CheckForItemDuplicate(desiredItem2))
                    deliverButton.gameObject.SetActive(true);
            }
            else if(desiredItem2 == "" || inventory.CheckForItem(desiredItem2))
                deliverButton.gameObject.SetActive(true);
    }

    public void Satisfy()
    {;
        StartCoroutine(SatisfyCoroutine());
    }

    IEnumerator SatisfyCoroutine()
    {
        yield return new WaitUntil(() => inventory.RemoveItem(desiredItem1));
        if (desiredItem2 != "")
            yield return new WaitUntil(() => inventory.RemoveItem(desiredItem2));

        Debug.Log(inventory.CheckForItem(desiredItem1));

        OrderScreen orderScreen = GetComponentInParent<OrderScreen>();
        orderScreen.OnOrderCompleted();

        Destroy(gameObject);
    }
}
