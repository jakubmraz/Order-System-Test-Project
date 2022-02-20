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
            bool active = activeContract.GetContractInfo(out ContractRequest[] contractRequests, out _);

            contractPanels[i].UpdateContractPanel(active, activeContract.GetBuildingName(), contractRequests);

            i++;
        }

        for (; i < contractPanels.Count; i++)
        {
            contractPanels[i].UpdateContractPanel(false, ":^)", default);
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