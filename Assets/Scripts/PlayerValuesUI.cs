using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerValuesUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PlayerName;
    [SerializeField] private TextMeshProUGUI PlayerLevel;
    [SerializeField] private Slider XpSlider;
    [SerializeField] private TextMeshProUGUI XpValue;
    [SerializeField] private TextMeshProUGUI PlayerEnergy;
    [SerializeField] private TextMeshProUGUI PlayerMoney;

    public void UpdateUI()
    {
        PlayerName.text = PlayerValues.Instance.PlayerName;
        PlayerLevel.text = "" + PlayerValues.Instance.PlayerLevel;
        XpSlider.maxValue = PlayerValues.Instance.PlayerXpRequired;
        XpSlider.value = PlayerValues.Instance.PlayerXp;
        XpValue.text = PlayerValues.Instance.PlayerXp + "/" + PlayerValues.Instance.PlayerXpRequired;
        PlayerEnergy.text = PlayerValues.Instance.PlayerEnergy + "/" + PlayerValues.Instance.PlayerEnergyMax;
        PlayerMoney.text = "" + PlayerValues.Instance.PlayerMoney;
    }
}
