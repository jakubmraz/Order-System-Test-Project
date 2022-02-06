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

    private const int DefaultAmountDesired = 3;
    //Minutes; to set hours, just write [hours] * 60
    private const int ContractCooldown = 3;
    
    private bool contractActive;
    private ItemData itemDesired;
    private int amountDesired;
    private int amountDelivered;
    private DateTime nextContractTime;

    [SerializeField] private Canvas popupCanvas;
    [SerializeField] private TextMeshProUGUI popupName;
    [SerializeField] private Image popupItemImage;
    [SerializeField] private TextMeshProUGUI popupAmount;
    [SerializeField] private Slider popupSlider;
    [SerializeField] private RectTransform popupActiveContract;
    [SerializeField] private RectTransform popupInactiveContract;
    [SerializeField] private TextMeshProUGUI popupInactiveName;
    [SerializeField] private TextMeshProUGUI popupInactiveCooldown;
    [SerializeField] private Button popupInactiveStartContractButton;

    void Update()
    {
        if (popupCanvas.gameObject.activeInHierarchy)
        {
            UpdatePopupWindow();
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

    public bool GetContractInfo(out ItemData itemDesired, out int amountDesired, out int amountDelivered, out DateTime nextContractTime)
    {
        if (contractActive)
        {
            itemDesired = this.itemDesired;
            amountDesired = this.amountDesired;
            amountDelivered = this.amountDelivered;
            nextContractTime = this.nextContractTime;

            return true;
        }
        else
        {
            itemDesired = ItemDataAccessor.Instance.GetItemList()[0];
            amountDesired = 0;
            amountDelivered = 0;
            nextContractTime = this.nextContractTime;

            return false;
        }
    }

    public void LoadContract(bool active, ItemData itemDesired, int amountDesired, int amountDelivered,
        DateTime nextContractTime)
    {
        contractActive = active;
        this.itemDesired = itemDesired;
        this.amountDesired = amountDesired;
        this.amountDelivered = amountDelivered;
        this.nextContractTime = nextContractTime;

        if (active)
        {
            ContractSystem.Instance.AddContract(this);
        }
    }

    public void DeliverItem()
    {
        if (Inventory.Instance.CheckForNonBrokenItem(itemDesired.Name))
        {
            Inventory.Instance.RemoveItem(itemDesired.Name);
            amountDelivered++;
        }

        if (amountDelivered >= amountDesired)
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
        //Give rewards
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
        itemDesired = buildingData.DesiredItems[Random.Range(0, GetBuildingDesiredItems().Count - 1)];
        ContractSystem.Instance.AddContract(this);

        amountDelivered = 0;
        amountDesired = DefaultAmountDesired;

        ContractSystem.Instance.SaveContractData();
    }

    private void UpdatePopupWindow()
    {
        if (contractActive)
        {
            popupActiveContract.gameObject.SetActive(true);
            popupInactiveContract.gameObject.SetActive(false);
            popupName.text = GetBuildingName();
            popupItemImage.sprite = itemDesired.Sprite;
            popupAmount.text = amountDelivered + "/" + amountDesired;
            popupSlider.maxValue = amountDesired;
            popupSlider.value = amountDelivered;
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
