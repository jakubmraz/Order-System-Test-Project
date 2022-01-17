using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string InventoryData;
    public int CollectionData;
    public string ContainerData;
    public string TimeData;
    public string OrderData;

    public SaveData(string inventoryData, int collectionData, string containerData, string timeData, string orderData)
    {
        InventoryData = inventoryData;
        CollectionData = collectionData;
        ContainerData = containerData;
        TimeData = timeData;
        OrderData = orderData;
    }

    //Compatibility
    public SaveData(string inventoryData, int collectionData, string containerData, string timeData)
    {
        InventoryData = inventoryData;
        CollectionData = collectionData;
        ContainerData = containerData;
        TimeData = timeData;
        OrderData = "";
    }
}
