using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSystem : MonoBehaviour
{
    public static OrderSystem Instance { get; private set; }

    //Minutes
    public const int OrderCooldown = 3;
    public const int OrderCooldownOnSkip = 15;

    [SerializeField] private OrderScreen orderScreen;
    private Order[] orders;

    void Start()
    {
        Instance = this;
        orders = orderScreen.CreateOrderObjects();

        string loadedOrders = SavingLoading.Instance.LoadOrderData();
        if (loadedOrders != "")
        {
            FillOrdersWithLoadedData(loadedOrders);
        }
        else
        {
            foreach (Order order in orders)
            {
                order.PickDesiredItems();
            }
        }

        CheckPlayerInventory();
    }

    public void CheckPlayerInventory()
    {
        foreach (Order order in orders)
        {
            if (order.deliverButton != null)
            {
                order.deliverButton.gameObject.SetActive(false);

                if (Inventory.Instance.CheckForNonBrokenItem(order.desiredItem1))
                    if (order.desiredItem1 == order.desiredItem2)
                    {
                        if (Inventory.Instance.CheckForItemDuplicate(order.desiredItem2))
                            order.deliverButton.gameObject.SetActive(true);
                    }
                    else if (order.desiredItem2 == "" || Inventory.Instance.CheckForNonBrokenItem(order.desiredItem2))
                        order.deliverButton.gameObject.SetActive(true);
            }
        }

        SavingLoading.Instance.SaveOrderData();
    }

    public string GetOrderString()
    {
        string orderString = "";

        foreach (Order order in orders)
        {
            order.GetOrderData(out string order1, out string order2, out bool completed, out bool skipped, out DateTime timeCompleted);
            orderString += order1 + "#" + order2 + "#" + completed + "#" + timeCompleted + "#" + skipped + ";";
        }

        return orderString;
    }

    public void FillOrdersWithLoadedData(string data)
    {
        Debug.Log(data);
        string[] splitString = data.Split(';');

        int i = 0;
        foreach (Order order in orders)
        {
            string[] splitData = splitString[i].Split('#');
            Debug.Log(splitData[0]);
            try
            {
                order.FillLoadedData(splitData[0], splitData[1], bool.Parse(splitData[2]), DateTime.Parse(splitData[3]),
                    bool.Parse(splitData[4]));
            }
            catch
            {
                order.FillLoadedData(splitData[0], splitData[1], bool.Parse(splitData[2]), DateTime.Parse(splitData[3]), false);
            }
            
            i++;
        }
    }
}
