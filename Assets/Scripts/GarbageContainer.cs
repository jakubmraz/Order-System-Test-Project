using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GarbageContainer : MonoBehaviour
{
    [SerializeField] private RectTransform collectPanel;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private TextMeshProUGUI itemCountText;
    [SerializeField] private Slider itemCountSlider;

    public ItemData ContainedItem;
    public int itemCount;
    public const int maxItemCount = 50;
    public const int percentageSentOver = 100;

    void Awake()
    {
        itemImage.sprite = ContainedItem.Sprite;
        itemText.text = ContainedItem.Name;
        itemCountSlider.maxValue = maxItemCount;
    }

    void Update()
    {
        itemCountText.text = itemCount + "/" + maxItemCount;
        itemCountSlider.value = itemCount;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //garbageCollection.containedItem = ContainedItem;
            //garbageCollection.itemCount = itemCount;
            //garbageCollection.currentContainer = this;
            collectPanel.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collectPanel.gameObject.SetActive(false);
        }
    }

    public void ReplenishStorage()
    {
        itemCount++;
        ContainerSystem.Instance.SaveContainers();
    }

    public void ReplenishStorage(int amount)
    {
        itemCount += amount;
        ContainerSystem.Instance.SaveContainers();
    }

    public void SendToStorage()
    {
        float divisor = percentageSentOver / 100;
        int amountSentOver = Convert.ToInt32(itemCount / divisor);  
        Storage.Instance.FillItem(ContainedItem.Name, amountSentOver);
        itemCount -= amountSentOver;
        ContainerSystem.Instance.SaveContainers();
        SavingLoading.Instance.SaveStorageData(Storage.Instance.itemValues);
    }
}
