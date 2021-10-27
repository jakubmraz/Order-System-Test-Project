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
}
