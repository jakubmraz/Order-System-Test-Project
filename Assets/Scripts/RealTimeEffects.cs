using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealTimeEffects : MonoBehaviour
{
    public List<GarbageContainer> GarbageContainers;
    [HideInInspector] public int timeDivisor;

    // Start is called before the first frame update
    void Start()
    {
        timeDivisor = 10;
        StartCoroutine(TriggersEveryMinute());
    }

    IEnumerator TriggersEveryMinute()
    {
        while (true)
        {
            yield return new WaitForSeconds(60f);
            MinuteTick();
        }
    }

    void MinuteTick()
    {
        if (DateTime.Now.Minute % timeDivisor == 0)
        {
            foreach (var container in GarbageContainers)
            {
                container.ReplenishStorage();
            }
        }
    }
}
