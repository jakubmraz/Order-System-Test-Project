using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string InventoryData;
    public string OrderData;

    public SaveData(string inventoryData, string orderData)
    {
        InventoryData = inventoryData;
        OrderData = orderData;
    }
}
