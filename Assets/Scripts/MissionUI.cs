using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionUI : MonoBehaviour
{
    private MissionSO containedMission;

    [SerializeField] private Image missionImage;
    [SerializeField] private TextMeshProUGUI missionName;
    [SerializeField] private TextMeshProUGUI missionDesc;
    [SerializeField] private Slider misisonSlider;
    [SerializeField] private TextMeshProUGUI sliderText;
    [SerializeField] private TextMeshProUGUI xpRewardText;
    [SerializeField] private TextMeshProUGUI goldRewardText;

    public void UpdateUI(MissionSO mission, int expectedValue, int realValue, bool completed)
    {
        containedMission = mission;

        missionImage.sprite = mission.Sprite;
        missionName.text = mission.Name;
        missionDesc.text = mission.Description;
        misisonSlider.maxValue = expectedValue;
        misisonSlider.value = realValue;
        sliderText.text = $"{realValue}/{expectedValue}";
        xpRewardText.text = mission.XpReward.ToString();
        goldRewardText.text = mission.MoneyReward.ToString();
        //GetComponent<Button>().enabled = !completed;
    }

    public void ShowMissionInfo()
    {
        MissionSystem.Instance.ShowMissionInfo(containedMission);
    }
}
