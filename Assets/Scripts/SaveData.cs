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

    public SaveData(string inventoryData, int collectionData, string containerData, string timeData)
    {
        InventoryData = inventoryData;
        CollectionData = collectionData;
        ContainerData = containerData;
        TimeData = timeData;
    }
}
