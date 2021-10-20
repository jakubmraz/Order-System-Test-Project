using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SavingLoading : MonoBehaviour
{
    private string inventoryString = "";
    private Inventory inventory;

    void Awake()
    {
        inventory = GetComponent<Inventory>();
    }

    public void SaveData()
    {
        inventoryString = inventory.GetInventoryString();
        PlayerPrefs.SetString("Inventory", inventoryString);
        PlayerPrefs.Save();
        Debug.Log("Saved!," + inventoryString);
    }

    public string LoadData()
    {
        if(PlayerPrefs.HasKey("Inventory"))
            inventoryString = PlayerPrefs.GetString("Inventory");

        Debug.Log("Loaded!," + inventoryString);
        return inventoryString;
    }
}
