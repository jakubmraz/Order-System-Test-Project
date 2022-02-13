using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public void IncreasePlasticCapacity()
    {
        Storage.Instance.IncreaseItemCapacity("Plastic", 20);
    }

    public void IncreasePlasticContainerCapacity()
    {
        ContainerSystem.Instance.IncreaseContainerCapacity("Plastic", 20);
    }
}
