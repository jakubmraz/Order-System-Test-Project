using System;

using UnityEngine;

public class PlayerValues : MonoBehaviour
{
    public static PlayerValues Instance { get; private set; }

    public string PlayerName;
    public int PlayerLevel;
    public int PlayerXp;
    public int PlayerXpRequired;
    public int PlayerEnergy;
    public int PlayerEnergyMax;
    public int PlayerMoney;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SavePlayerData()
    {
        string playerData =
            $"{PlayerName}§{PlayerLevel}§{PlayerXp}§{PlayerXpRequired}§{PlayerEnergy}§{PlayerEnergyMax}§{PlayerMoney}";
        PlayerPrefs.SetString("PlayerData", playerData);
    }

    public bool LoadPlayerData()
    {
        if(!PlayerPrefs.HasKey("PlayerData")) return false;

        string[] loadedData = PlayerPrefs.GetString("PlayerData").Split('§');

        PlayerName = loadedData[0];
        PlayerLevel = Convert.ToInt32(loadedData[1]);
        PlayerXp = Convert.ToInt32(loadedData[2]);
        PlayerXpRequired = Convert.ToInt32(loadedData[3]);
        PlayerEnergy = Convert.ToInt32(loadedData[4]);
        PlayerEnergyMax = Convert.ToInt32(loadedData[5]);
        PlayerMoney = Convert.ToInt32(loadedData[6]);

        return true;
    }
}
