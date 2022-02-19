using System;
using System.Collections;
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

    private DateTime lastEnergyGain;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LoadPlayerData();
        EnergyGainStartup();
        StartCoroutine(PassiveEnergyCoroutine());
    }

    void Update()
    {
        
    }

    private IEnumerator PassiveEnergyCoroutine()
    {
        while (true)
        {
            Debug.Log("Energy:" + PlayerEnergy);

            yield return new WaitForSeconds(60f);
            AddEnergy(1);

            lastEnergyGain = DateTime.Now;
            SaveEnergyTime();
        }
    }

    private void SaveEnergyTime()
    {
        PlayerPrefs.SetString("EnergyTime", lastEnergyGain.ToString());
        PlayerPrefs.Save();
    }

    private void LoadEnergyTime()
    {
        if (PlayerPrefs.HasKey("EnergyTime"))
        {
            lastEnergyGain = DateTime.Parse(PlayerPrefs.GetString("EnergyTime"));
        }
        else
        {
            lastEnergyGain = DateTime.Now;
        }
    }

    private void EnergyGainStartup()
    {
        LoadEnergyTime();
        TimeSpan timeSpan = DateTime.Now.Subtract(lastEnergyGain);
        AddEnergy(timeSpan.Minutes);

        lastEnergyGain = DateTime.Now;
        SaveEnergyTime();
    }

    public bool CheckEnergy(int amount)
    {
        if (PlayerEnergy < amount) return false;
        return true;
    }

    public void AddMoney(int amount)
    {
        PlayerMoney += amount;
        SavePlayerData();
    }

    public void AddEnergy(int amount)
    {
        PlayerEnergy += amount;
        if (PlayerEnergy > PlayerEnergyMax)
        {
            PlayerEnergy = PlayerEnergyMax;
        }
        SavePlayerData();
    }

    public void AddXp(int amount)
    {
        PlayerXp += amount;
        if (PlayerXp >= PlayerXpRequired)
        {
            PlayerLevel++;
            PlayerXp -= PlayerXpRequired;
        }
        SavePlayerData();
    }

    public void SavePlayerData()
    {
        string playerData =
            $"{PlayerName}§{PlayerLevel}§{PlayerXp}§{PlayerXpRequired}§{PlayerEnergy}§{PlayerEnergyMax}§{PlayerMoney}";
        PlayerPrefs.SetString("PlayerData", playerData);
        PlayerPrefs.Save();
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
