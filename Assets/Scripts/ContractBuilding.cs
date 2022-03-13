using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ContractBuilding : MonoBehaviour
{
    [SerializeField] private ContractBuildingSO buildingData;

    //Minutes; to set hours, just write [hours] * 60
    private const int ContractCooldown = 3;
    private const int ContractDuration = 24 * 60;

    private bool contractActive;
    private ContractRequest[] contractRequests;
    private DateTime nextContractTime;
    private DateTime contractStartTime;

    [SerializeField] private Canvas popupCanvas;
    [SerializeField] private TextMeshProUGUI popupName;
    [SerializeField] private Image[] popupItemImages;
    [SerializeField] private TextMeshProUGUI[] popupAmounts;
    [SerializeField] private RectTransform popupActiveContract;
    [SerializeField] private RectTransform popupInactiveContract;
    [SerializeField] private TextMeshProUGUI popupInactiveName;
    [SerializeField] private TextMeshProUGUI popupInactiveCooldown;
    [SerializeField] private Button popupInactiveStartContractButton;
    [SerializeField] private TextMeshProUGUI popupTimeCountdown;

    void Update()
    {
        if (popupCanvas.gameObject.activeInHierarchy)
        {
            UpdatePopupWindow();
        }

        if (contractActive)
        {
            if (contractStartTime.AddMinutes(ContractDuration) < DateTime.Now)
            {
                FulfillContract();
            }
        }
    }

    public string GetBuildingName()
    {
        return buildingData.BuildingName;
    }

    public List<ItemData> GetBuildingDesiredItems()
    {
        return buildingData.DesiredItems;
    }

    public bool GetContractInfo(out ContractRequest[] contractRequests, out DateTime nextContractTime, out DateTime contractStartTime)
    {
        if (contractActive)
        {
            contractRequests = this.contractRequests;
            nextContractTime = this.nextContractTime;
            contractStartTime = this.contractStartTime;
            return true;
        }
        else
        {
            contractRequests = new ContractRequest[6];
            nextContractTime = this.nextContractTime;
            contractStartTime = this.contractStartTime;
            return false;
        }
    }

    public void LoadContract(bool active, ContractRequest[] contractRequests,
        DateTime nextContractTime, DateTime contractStartTime)
    {
        contractActive = active;
        this.contractRequests = contractRequests;
        this.nextContractTime = nextContractTime;
        this.contractStartTime = contractStartTime;

        if (active)
        {
            ContractSystem.Instance.AddContract(this);
        }
    }

    public void DeliverItem()
    {
        foreach (var request in contractRequests)
        {
            for (int i = request.amountDelivered; i < request.amountDesired; i++)
            {
                if (Inventory.Instance.CheckForNonBrokenItem(request.itemDesired.Name))
                {
                    Inventory.Instance.RemoveItem(request.itemDesired.Name);
                    request.amountDelivered++;
                }
            }
        }

        bool fulfilled = true;

        foreach (var request in contractRequests)
        {
            if (request.amountDelivered < request.amountDesired)
            {
                fulfilled = false;
                break;
            }
        }

        if (fulfilled)
        {
            FulfillContract();
        }

        ContractSystem.Instance.SaveContractData();
        SavingLoading.Instance.SaveInventoryData();
    }

    private void FulfillContract()
    {
        contractActive = false;
        ContractSystem.Instance.RemoveContract(this);
        nextContractTime = DateTime.Now.AddMinutes(ContractCooldown);

        ContractSystem.Instance.SaveContractData();
        GiveRewards();
    }

    private void GiveRewards()
    {
        int completedRequests = 0;
        foreach (var request in contractRequests)
        {
            if (request.amountDelivered == request.amountDesired)
            {
                completedRequests++;
            }
        }

        //Give rewards based on number of completed requests
    }

    public void StartContract()
    {
        if (!ContractSystem.Instance.CanAcceptContract())
        {
            Debug.Log("Cannot accept any more contracts.");
            return;
        }

        if (IsOnCooldown(out _))
        {
            Debug.Log("Contract is on cooldown.");
            return;
        }

        contractActive = true;
        contractStartTime = DateTime.Now;
        contractRequests = new ContractRequest[6];
        for (int i = 0; i < contractRequests.Length; i++)
        {
            Debug.Log(GetBuildingDesiredItems().Count);
            contractRequests[i] = new ContractRequest
            {
                itemDesired = buildingData.DesiredItems[Random.Range(0, GetBuildingDesiredItems().Count)],
                amountDelivered = 0
            };

            if (contractRequests[i].itemDesired.Final) contractRequests[i].amountDesired = 1;
            else contractRequests[i].amountDesired = Random.Range(3, 5);
        }

        ContractSystem.Instance.AddContract(this);

        ContractSystem.Instance.SaveContractData();
    }

    private void UpdatePopupWindow()
    {
        if (contractActive)
        {
            popupActiveContract.gameObject.SetActive(true);
            popupInactiveContract.gameObject.SetActive(false);
            popupName.text = GetBuildingName();
            popupTimeCountdown.text = contractStartTime.AddMinutes(ContractDuration).Subtract(DateTime.Now).Hours +
                                      ":" + contractStartTime.AddMinutes(ContractDuration).Subtract(DateTime.Now).Minutes;

            for (int i = 0; i < 6; i++)
            {
                popupItemImages[i].sprite = contractRequests[i].itemDesired.Sprite;
                popupAmounts[i].text = contractRequests[i].amountDelivered + "/" + contractRequests[i].amountDesired;
            }
        }
        else
        {
            popupActiveContract.gameObject.SetActive(false);
            popupInactiveContract.gameObject.SetActive(true);
            popupInactiveName.text = GetBuildingName();

            if (IsOnCooldown(out TimeSpan timespan))
            {
                popupInactiveStartContractButton.gameObject.SetActive(false);
                popupInactiveCooldown.gameObject.SetActive(true);
                popupInactiveCooldown.text = $"Next contract in: {timespan.Hours}:{timespan.Minutes}";
            }
            else
            {
                popupInactiveCooldown.gameObject.SetActive(false);
                popupInactiveStartContractButton.gameObject.SetActive(true);
            }
        }
    }

    private bool IsOnCooldown(out TimeSpan timespan)
    {
        if (nextContractTime == default) return false;

        timespan = nextContractTime.Subtract(DateTime.Now);
        if (timespan.TotalMinutes <= 0) return false;
        else return true;

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            popupCanvas.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            popupCanvas.gameObject.SetActive(false);
        }
    }
}