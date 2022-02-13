using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerSystem : MonoBehaviour
{
    public static ContainerSystem Instance { get; private set; }
    public List<GarbageContainer> garbageContainers;

    public Dictionary<string, int> ItemCapacities;
    public int BaseItemCapacity = 50;

    void Awake()
    {
        Instance = this;
        
        ItemCapacities = new Dictionary<string, int>();

        LoadContainers();
        if (SavingLoading.Instance.LoadContainerCapacity(out Dictionary<string, int> capacities))
        {
            ItemCapacities = capacities;
        }
        else
        {
            foreach (var item in ItemDataAccessor.Instance.GetBaseItemList())
            {
                ItemCapacities.Add(item.Name, BaseItemCapacity);
            }
        }

        foreach (var cap in ItemCapacities)
        {
            Debug.Log(cap.Key + " " + cap.Value);
        }

        UpdateContainerCaps();
    }

    public GarbageContainer GetContainer(ItemData item)
    {
        foreach (var container in garbageContainers)
        {
            if (container.ContainedItem == item)
                return container;
        }

        return null;
    }

    public GarbageContainer GetContainer(string item)
    {
        foreach (var container in garbageContainers)
        {
            if (container.ContainedItem.Name == item)
                return container;
        }

        return null;
    }

    public void LoadContainers()
    {
        SavingLoading.Instance.LoadContainerData(garbageContainers);
    }

    public void SaveContainers()
    {
        SavingLoading.Instance.SaveContainerData(garbageContainers);
    }

    public void ChangeContainerCapacity(string itemName, int value)
    {
        ItemCapacities[itemName] = value;
        SavingLoading.Instance.SaveContainerCapacity(ItemCapacities);
        UpdateContainerCaps();
    }

    public void IncreaseContainerCapacity(string itemName, int value)
    {
        ItemCapacities[itemName] += value;
        SavingLoading.Instance.SaveContainerCapacity(ItemCapacities);
        UpdateContainerCaps();
    }

    private void UpdateContainerCaps()
    {
        foreach (var container in garbageContainers)
        {
            foreach (var cap in ItemCapacities)
            {
                if (cap.Key == container.ContainedItem.Name)
                {
                    container.itemCapacity = cap.Value;
                    break;
                }

                container.itemCapacity = BaseItemCapacity;
            }
        }
    }
}
