using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SavingLoading : MonoBehaviour
{
    private string inventoryString = "0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;";
    private Inventory inventory;

    void Awake()
    {
        inventory = GetComponent<Inventory>();
    }

    public void SaveInventoryData()
    {
        inventoryString = inventory.GetInventoryString();
        PlayerPrefs.SetString("Inventory", inventoryString);
        PlayerPrefs.Save();
        Debug.Log("Saved!," + inventoryString);
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
}
