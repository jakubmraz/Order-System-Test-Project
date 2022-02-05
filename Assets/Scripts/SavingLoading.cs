using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SavingLoading : MonoBehaviour
{
    public static SavingLoading Instance { get; private set; }

    private string inventoryString = "0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;";
    private Inventory inventory;

    void Awake()
    {
        Instance = this;

        inventory = GetComponent<Inventory>();
        LoadDataFromFile();
    }

    public void SaveOrderData()
    {
        string orderString = OrderSystem.Instance.GetOrderString();
        PlayerPrefs.SetString("Orders", orderString);
        PlayerPrefs.Save();
        SaveTime();
        SaveDataToFile();
    }

    public string LoadOrderData()
    {
        string orderString = "";
        if (PlayerPrefs.HasKey("Orders"))
            orderString = PlayerPrefs.GetString("Orders");
        return orderString;
    }

    public void SaveInventoryData()
    {
        inventoryString = inventory.GetInventoryString();
        PlayerPrefs.SetString("Inventory", inventoryString);
        PlayerPrefs.Save();
        Debug.Log("Saved!," + inventoryString);
        SaveTime();
        SaveDataToFile();
    }

    public string LoadInventoryData()
    {
        if(PlayerPrefs.HasKey("Inventory"))
            inventoryString = PlayerPrefs.GetString("Inventory");

        Debug.Log("Loaded!," + inventoryString);
        return inventoryString;
    }

    //public void SaveCollectionData(int timeDivisor)
    //{
    //    PlayerPrefs.SetInt("TimeDivisor", timeDivisor);
    //    PlayerPrefs.Save();
    //    Debug.Log("Saved!, " + timeDivisor);
    //    SaveTime();
    //    SaveDataToFile();
    //}

    //public int LoadCollectionData()
    //{
    //    int timeDivisor;
    //    if (PlayerPrefs.HasKey("TimeDivisor"))
    //        timeDivisor = PlayerPrefs.GetInt("TimeDivisor");
    //    else
    //        timeDivisor = 0;
    //    return timeDivisor;
    //}

    public void SaveContainerData(List<GarbageContainer> containers)
    {
        string containerString = "";
        foreach (var container in containers)
        {
            containerString += container.itemCount + ";";
        }
        PlayerPrefs.SetString("Containers", containerString);
        PlayerPrefs.Save();
        Debug.Log("Saved!, " + containerString);
        SaveTime();
        SaveDataToFile();
    }

    public void LoadContainerData(List<GarbageContainer> containers)
    {
        if (PlayerPrefs.HasKey("Containers"))
        {
            string containerString = PlayerPrefs.GetString("Containers");
            string[] containerValues = containerString.Split(';');

            int i = 0;
            foreach (var container in containers)
            {
                container.itemCount = Convert.ToInt32(containerValues[i]);
                i++;
            }
        }
        else
        {
            foreach (var container in containers)
            {
                container.itemCount = 10;
            }
        }
    }

    public void SaveStorageData(Dictionary<string, int> storageDictionary)
    {
        string storageString = "";

        foreach (var pair in storageDictionary)
        {
            storageString += $"{pair.Key}#{pair.Value};";
        }
        PlayerPrefs.SetString("Storage", storageString);
    }

    public Dictionary<string, int> LoadStorageData()
    {
        if (PlayerPrefs.HasKey("Storage"))
        {
            Dictionary<string, int> storageDictionary = new Dictionary<string, int>();

            string storageString = PlayerPrefs.GetString("Storage");
            string[] pairs = storageString.Split(';');
            foreach (var pair in pairs)
            {
                if (pair.Contains("#"))
                {
                    string[] values = pair.Split('#');
                    storageDictionary.Add(values[0], Convert.ToInt32(values[1]));
                }
            }

            return storageDictionary;
        }

        return null;
    }

    public void SaveTime()
    {
        string dateString = DateTime.Now.ToString();
        PlayerPrefs.SetString("Time", dateString);
        PlayerPrefs.Save();
    }

    public int GetTimeDifference()
    {
        DateTime lastTime;

        if (PlayerPrefs.HasKey("Time"))
        {
            string dateString = PlayerPrefs.GetString("Time");
            lastTime = Convert.ToDateTime(dateString);
        }
        else lastTime = DateTime.Now;

        int minuteDifference = (DateTime.Now - lastTime).Minutes;
        Debug.Log(minuteDifference);
        return minuteDifference;
    }

    public void SaveStorageCapacity()
    {
        PlayerPrefs.SetInt("StorageCapacity", Storage.Instance.MaxStorageSpace);
        PlayerPrefs.Save();
    }

    public bool LoadStorageCapacity(out int capacity)
    {
        capacity = 0;
        if (PlayerPrefs.HasKey("StorageCapacity"))
        {
            capacity = PlayerPrefs.GetInt("StorageCapacity");
            return true;
        }

        return false;
    }

    public void SaveContainerCapacity()
    {
        PlayerPrefs.SetInt("ContainerCapacity", ContainerSystem.Instance.MaxItemCount);
        PlayerPrefs.Save();
    }

    public bool LoadContainerCapacity(out int capacity)
    {
        capacity = 0;
        if(PlayerPrefs.HasKey("ContainerCapacity"))
        {
            capacity = PlayerPrefs.GetInt("ContainerCapacity");
            return true;
        }

        return false;
    }

    public void SaveDataToFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        RealTimeEffects effects = FindObjectOfType<RealTimeEffects>();
        List<GarbageContainer> containers = ContainerSystem.Instance.garbageContainers;

        SaveData data = new SaveData(LoadInventoryData(), LoadOrderData());
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadDataFromFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.Log("File not found");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        SaveData data = (SaveData)bf.Deserialize(file);
        file.Close();

        Debug.Log("Loaded data from file:\nInventory: " + data.InventoryData + "\nTimeDivisor: " + data.OrderData);

        PlayerPrefs.SetString("Inventory", data.InventoryData);
        PlayerPrefs.SetString("Orders", data.OrderData);

        PlayerPrefs.Save();
    }
}
