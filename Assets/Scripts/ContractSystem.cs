using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractSystem : MonoBehaviour
{
    public static ContractSystem Instance { get; private set; }

    [SerializeField] private List<ContractBuilding> contractBuildings;
    private List<ContractBuilding> activeContracts;

    private const int PossibleContracts = 3;

    void Awake()
    {
        Instance = this;

        activeContracts = new List<ContractBuilding>();
    }

    void Start()
    {
        LoadContractData();
    }

    public List<ContractBuilding> GetActiveContracts()
    {
        return activeContracts;
    }

    public bool CanAcceptContract()
    {
        if (activeContracts.Count >= PossibleContracts) return false;
        else return true;
    }

    public void AddContract(ContractBuilding newContract)
    {
        activeContracts.Add(newContract);
    }

    public void RemoveContract(ContractBuilding contract)
    {
        if (activeContracts.Contains(contract))
        {
            activeContracts.Remove(contract);
        }
        else Debug.Log("Something's wrong.");
    }

    public void SaveContractData()
    {
        string contractData = "";

        foreach (var contractBuilding in contractBuildings)
        {
            bool active = contractBuilding.GetContractInfo(out ItemData itemDesired, out int amountDesired,
                out int amountDelivered, out DateTime nextContractTime);
            contractData +=
                $"{active}#{itemDesired.Name}#{amountDesired}#{amountDelivered}#{nextContractTime};";
        }

        PlayerPrefs.SetString("ContractData", contractData);
    }

    public void LoadContractData()
    {
        if (!PlayerPrefs.HasKey("ContractData")) return;

        string contractData = PlayerPrefs.GetString("ContractData");
        string[] contracts = contractData.Split(';');

        int i = 0;
        foreach (var contractBuilding in contractBuildings)
        {
            if (contracts[i] == "") break;

            string[] contractDetails = contracts[i].Split('#');
            Debug.Log(contracts[i]);
            contractBuilding.LoadContract(Convert.ToBoolean(contractDetails[0]), ItemDataAccessor.Instance.GetItemData(contractDetails[1]),
                Convert.ToInt32(contractDetails[2]), Convert.ToInt32(contractDetails[3]), DateTime.Parse(contractDetails[4]));

            i++;
        }
    }
}
