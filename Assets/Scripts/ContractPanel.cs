using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContractPanel : MonoBehaviour
{
    [SerializeField] private RectTransform ActiveContractPanel;
    [SerializeField] private TextMeshProUGUI BuildingName;
    [SerializeField] private Image ItemImage;
    [SerializeField] private Slider AmountSlider;
    [SerializeField] private TextMeshProUGUI AmountNumber;

    public void UpdateContractPanel(bool active, string name, Sprite sprite, int amountDesired, int amountDelivered)
    {
        if (active)
        {
            ActiveContractPanel.gameObject.SetActive(true);
            BuildingName.text = name;
            ItemImage.sprite = sprite;
            AmountSlider.maxValue = amountDesired;
            AmountSlider.value = amountDelivered;
            AmountNumber.text = $"{amountDelivered}/{amountDesired}";
        }
        else ActiveContractPanel.gameObject.SetActive(false);
    }
}
