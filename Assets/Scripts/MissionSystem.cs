﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionSystem : MonoBehaviour
{
    public static MissionSystem Instance {get; private set;}

    [SerializeField] private List<MissionSO> missions;
    private bool[] completedMissions;

    [SerializeField] private RectTransform missionUiContainer;
    [SerializeField] private MissionUI missionUiPrefab;

    [SerializeField] private RectTransform missionInfoWindow;
    [SerializeField] private Image missionImage;
    [SerializeField] private TextMeshProUGUI missionName;
    [SerializeField] private TextMeshProUGUI missionDesc;
    [SerializeField] private TextMeshProUGUI missionRequirements;
    [SerializeField] private Slider misisonSlider;
    [SerializeField] private TextMeshProUGUI sliderText;
    [SerializeField] private TextMeshProUGUI xpRewardText;
    [SerializeField] private TextMeshProUGUI goldRewardText;
    [SerializeField] private Button completeButton;

    private MissionSO selectedMission;

    void Awake()
    {
        Instance = this;

        completedMissions = new bool[missions.Count];
        for(int i = 0; i < completedMissions.Length; i++)
        {
            completedMissions[i] = false;
        }

        if(PlayerPrefs.HasKey("MissionCompletion"))
        {
            string[] loaded = PlayerPrefs.GetString("MissionCompletion").Split(';');
            for(int i = 0; i < completedMissions.Length; i++)
            {
                completedMissions[i] = Convert.ToBoolean(loaded[i]);
            }
        }

        OnUIOpened();
    }

    public void OnUIOpened()
    {
        foreach(Transform child in missionUiContainer)
        {
            Destroy(child.gameObject);
        }

        foreach(MissionSO mission in missions.OrderByDescending(x => x.Chapter))
        {
            MissionUI missionUI = Instantiate(missionUiPrefab, missionUiContainer).GetComponent<MissionUI>();

            int expectedValue = mission.PrefGoalValue;
            int actualValue = 0;

            if(PlayerPrefs.HasKey(mission.PrefName)) actualValue = PlayerPrefs.GetInt(mission.PrefName);

            missionUI.UpdateUI(mission, expectedValue, actualValue, IsMissionCompleted(mission));
        }

        missionInfoWindow.gameObject.SetActive(false);
    }

    public void ShowMissionInfo(MissionSO mission)
    {
        missionInfoWindow.gameObject.SetActive(true);
        selectedMission = mission;

        int expectedValue = mission.PrefGoalValue;
        int actualValue = 0;

        if(PlayerPrefs.HasKey(mission.PrefName)) actualValue = PlayerPrefs.GetInt(mission.PrefName);

        missionImage.sprite = mission.Sprite;
        missionName.text = mission.Name;
        missionDesc.text = mission.Description;
        missionRequirements.text = mission.Goals;
        misisonSlider.maxValue = expectedValue;
        misisonSlider.value = actualValue;
        sliderText.text = $"{actualValue}/{expectedValue}";
        xpRewardText.text = mission.XpReward.ToString();
        goldRewardText.text = mission.MoneyReward.ToString();

        if(actualValue == expectedValue) completeButton.enabled = true;        
        else completeButton.enabled = false;
    }

    private bool IsMissionCompleted(MissionSO mission)
    {
        int i = missions.IndexOf(mission);
        return completedMissions[i];
    }

    public void CompleteMission()
    {
        int i = missions.IndexOf(selectedMission);
        completedMissions[i] = true;
        SaveCompletionData();

        PlayerValues.Instance.AddMoney(selectedMission.MoneyReward);
        PlayerValues.Instance.AddXp(selectedMission.XpReward);

        OnUIOpened();
    }

    public void SaveCompletionData()
    {
        string saveString = "";

        for(int i = 0; i < completedMissions.Count(); i++)
        {
            saveString += completedMissions[i] + ";";
        }

        PlayerPrefs.SetString("MissionCompletion", saveString);
        PlayerPrefs.Save();
    }
}
