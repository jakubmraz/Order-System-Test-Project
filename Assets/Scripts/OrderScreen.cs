using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderScreen : MonoBehaviour
{
    [SerializeField] private List<RectTransform> orderHolders;

    public Order orderPrefab;

    public Order[] CreateOrderObjects()
    {
        Order[] ordersCreated = new Order[9];
        int i = 0;

        foreach (var orderHolder in orderHolders)
        {
            Order newOrder = Instantiate(orderPrefab, orderHolder);
            ordersCreated[i] = newOrder;
            i++;
        }

        return ordersCreated;
    }

    public void CloseOrderScreen()
    {
        SavingLoading.Instance.SaveOrderData();
        gameObject.SetActive(false);
    }
}
