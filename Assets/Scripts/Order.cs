using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Order : MonoBehaviour
{
    [SerializeField] private Image desiredItem1Image;
    [SerializeField] private Image desiredItem2Image;
    [SerializeField] public Button deliverButton;
    [SerializeField] private RectTransform activeOrderUI;
    [SerializeField] private TextMeshProUGUI cooldownTimerTMP;

    public string desiredItem1;
    public string desiredItem2;

    public bool completed;
    public bool skipped;
    private DateTime completedTime;

    void Awake()
    {
        
    }

    void Update()
    {
        if (completed)
        {
            TimeSpan minutesBetween = DateTime.Now - completedTime;
            double minutesLeftDouble = OrderSystem.OrderCooldown - minutesBetween.TotalMinutes;
            if (skipped) minutesLeftDouble = OrderSystem.OrderCooldownOnSkip - minutesBetween.TotalMinutes;
            int minutesLeftWhole = Convert.ToInt32(minutesLeftDouble);
            TimeSpan timeLeft = TimeSpan.FromMinutes(minutesLeftWhole);
            cooldownTimerTMP.text = timeLeft.Hours + ":" + timeLeft.Minutes;

            if (minutesLeftWhole <= 0)
            {
                GenerateNewOrder();
            }
        }
    }

    public void PickDesiredItems()
    {
        Items items = new Items();
        int selection = Random.Range(0, items.ItemList.Count);
        desiredItem1 = items.ItemList[selection].Name;
        desiredItem1Image.sprite = items.ItemList.FirstOrDefault(item => item.Name == desiredItem1).Sprite;

        if (Random.Range(1, 10) <= 3)
        {
            selection = Random.Range(0, items.ItemList.Count);
            desiredItem2 = items.ItemList[selection].Name;
            desiredItem2Image.sprite = items.ItemList.FirstOrDefault(item => item.Name == desiredItem2).Sprite;
        }
        else
        {
            desiredItem2 = "";
            Color tempColour = desiredItem2Image.color;
            tempColour.a = 0;
            desiredItem2Image.color = tempColour;
        }
    }

    public void Satisfy()
    {
        StartCoroutine(SatisfyCoroutine());
    }

    IEnumerator SatisfyCoroutine()
    {
        yield return new WaitUntil(() => Inventory.Instance.RemoveItem(desiredItem1));
        if (desiredItem2 != "")
            yield return new WaitUntil(() => Inventory.Instance.RemoveItem(desiredItem2));

        SavingLoading.Instance.SaveInventoryData();

        OrderSystem.Instance.CheckPlayerInventory();

        completed = true;
        activeOrderUI.gameObject.SetActive(false);
        cooldownTimerTMP.gameObject.SetActive(true);

        completedTime = DateTime.Now;
    }

    public void SkipOrder()
    {
        completed = true;
        skipped = true;
        activeOrderUI.gameObject.SetActive(false);
        cooldownTimerTMP.gameObject.SetActive(true);

        completedTime = DateTime.Now;
    }

    void GenerateNewOrder()
    {
        completed = false;
        skipped = false;
        cooldownTimerTMP.gameObject.SetActive(false);
        activeOrderUI.gameObject.SetActive(true);
        PickDesiredItems();

        OrderSystem.Instance.CheckPlayerInventory();
    }

    public void GetOrderData(out string order1, out string order2, out bool completed, out bool skipped, out DateTime timeCompleted)
    {

        order1 = desiredItem1;
        order2 = desiredItem2;
        completed = this.completed;
        skipped = this.skipped;
        timeCompleted = completedTime;
    }

    public void FillLoadedData(string order1, string order2, bool completed, DateTime timeCompleted, bool skipped)
    {
        desiredItem1 = order1;
        desiredItem2 = order2;
        this.completed = completed;
        this.skipped = skipped;
        completedTime = timeCompleted;

        Items items = new Items();
        desiredItem1Image.sprite = items.ItemList.FirstOrDefault(item => item.Name == desiredItem1).Sprite;
        if (desiredItem2 != "")
        {
            desiredItem2Image.sprite = items.ItemList.FirstOrDefault(item => item.Name == desiredItem2).Sprite;
        }
        else
        {
            Color tempColour = desiredItem2Image.color;
            tempColour.a = 0;
            desiredItem2Image.color = tempColour;
        }

        if (completed)
        {
            activeOrderUI.gameObject.SetActive(false);
            cooldownTimerTMP.gameObject.SetActive(true);
        }
    }
}
