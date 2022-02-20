using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContractPanel : MonoBehaviour
{
    [SerializeField] private RectTransform ActiveContractPanel;
    [SerializeField] private TextMeshProUGUI BuildingName;
    [SerializeField] private Image[] ItemImages;
    [SerializeField] private TextMeshProUGUI[] ItemAmounts;

    public void UpdateContractPanel(bool active, string name, ContractRequest[] contractRequests)
    {
        if (active)
        {
            ActiveContractPanel.gameObject.SetActive(true);
            BuildingName.text = name;

            for (int i = 0; i < 6; i++)
            {
                ItemImages[i].sprite = contractRequests[i].itemDesired.Sprite;
                ItemAmounts[i].text = contractRequests[i].amountDelivered + "/" + contractRequests[i].amountDesired;
            }
        }
        else ActiveContractPanel.gameObject.SetActive(false);
    }
}
