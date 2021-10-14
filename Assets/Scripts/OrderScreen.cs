using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderScreen : MonoBehaviour
{
    [SerializeField] private List<RectTransform> orderHolders;

    public Order orderPrefab;

    void Awake()
    {
        FillInOrders();
    }

    void FillInOrders()
    {
        foreach (var orderHolder in orderHolders)
        {
            Order newOrder = Instantiate(orderPrefab, orderHolder);
        }
    }

    public void OnOrderCompleted()
    {
        Order order;

        foreach (var orderHolder in orderHolders)
        {
            order = orderHolder.GetComponentInChildren<Order>();
            if(order)
                order.CheckPlayerInventory();
        }
    }

    public void CloseOrderScreen()
    {
        gameObject.SetActive(false);
    }
}
