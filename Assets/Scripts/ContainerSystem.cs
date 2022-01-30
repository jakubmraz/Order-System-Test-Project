using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerSystem : MonoBehaviour
{
    public static ContainerSystem Instance { get; private set; }
    public List<GarbageContainer> garbageContainers;

    void Awake()
    {
        Instance = this;
        LoadContainers();
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
}
