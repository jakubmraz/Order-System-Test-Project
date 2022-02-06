using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractInfoBoard : MonoBehaviour
{
    [SerializeField] private Canvas infoCanvas;
    [SerializeField] private List<ContractPanel> contractPanels;

    void Update()
    {
        if (infoCanvas.gameObject.activeInHierarchy)
        {
            UpdateContractPanels();
        }
    }

    void UpdateContractPanels()
    {
        int i = 0;

        foreach (var activeContract in ContractSystem.Instance.GetActiveContracts())
        {
            bool active = activeContract.GetContractInfo(out ItemData itemDesired, out int amountDesired,
                out int amountDelivered, out _);

            contractPanels[i].UpdateContractPanel(active, activeContract.GetBuildingName(), itemDesired.Sprite, amountDesired, amountDelivered);

            i++;
        }

        for (; i < contractPanels.Count; i++)
        {
               contractPanels[i].UpdateContractPanel(false, ":^)", default, default, default); 
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            infoCanvas.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            infoCanvas.gameObject.SetActive(false);
        }
    }
}
