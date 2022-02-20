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
            bool active = contractBuilding.GetContractInfo(out ContractRequest[] contractRequests, out DateTime nextContractTime);
            contractData += $"{active}§";

            foreach (var contractRequest in contractRequests)
            {
                if (contractRequest == null)
                {
                    contractData += "Bicycle#0#1§";
                }
                else
                {
                    contractData +=
                        $"{contractRequest.itemDesired.Name}#{contractRequest.amountDesired}#{contractRequest.amountDelivered}§";
                }
            }

            contractData += $"{nextContractTime};";
        }

        PlayerPrefs.SetString("ContractData", contractData);
        PlayerPrefs.Save();
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

            string[] contractRequests = contracts[i].Split('§');
            bool active = Convert.ToBoolean(contractRequests[0]);
            ContractRequest[] requests = new ContractRequest[6];
            for (int j = 1; j < 7; j++)
            {
                string[] requestDetails = contractRequests[j].Split('#');
                requests[j-1] = new ContractRequest()
                {
                    itemDesired = ItemDataAccessor.Instance.GetItemData(requestDetails[0]),
                    amountDesired = Convert.ToInt32(requestDetails[1]),
                    amountDelivered = Convert.ToInt32(requestDetails[2])
                };
            }
            DateTime nextTime = DateTime.Parse(contractRequests[7]);
            contractBuilding.LoadContract(active, requests, nextTime);
            i++;
        }
    }
}
