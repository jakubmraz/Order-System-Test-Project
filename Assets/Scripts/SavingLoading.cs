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

    public void SaveCollectionData(int timeDivisor)
    {
        PlayerPrefs.SetInt("TimeDivisor", timeDivisor);
        PlayerPrefs.Save();
        Debug.Log("Saved!, " + timeDivisor);
        SaveTime();
        SaveDataToFile();
    }

    public int LoadCollectionData()
    {
        int timeDivisor;
        if (PlayerPrefs.HasKey("TimeDivisor"))
            timeDivisor = PlayerPrefs.GetInt("TimeDivisor");
        else
            timeDivisor = 0;
        return timeDivisor;
    }

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

    public void SaveDataToFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        RealTimeEffects effects = FindObjectOfType<RealTimeEffects>();
        List<GarbageContainer> containers = effects.GarbageContainers;

        SaveData data = new SaveData(LoadInventoryData(), LoadCollectionData(), PlayerPrefs.GetString("Containers"), PlayerPrefs.GetString("Time"));
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
            Debug.LogError("File not found");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        SaveData data = (SaveData)bf.Deserialize(file);
        file.Close();

        Debug.Log("Loaded data from file:\nInventory: " + data.InventoryData + "\nTimeDivisor: " + data.CollectionData + "\nContainers: " + data.ContainerData + "\nTime: " + data.TimeData);

        PlayerPrefs.SetString("Inventory", data.InventoryData);
        PlayerPrefs.SetInt("TimeDivisor", data.CollectionData);
        PlayerPrefs.SetString("Containers", data.ContainerData);
        PlayerPrefs.SetString("Time", data.TimeData);

        PlayerPrefs.Save();
    }
}
