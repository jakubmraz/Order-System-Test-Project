using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealTimeEffects : MonoBehaviour
{
    public List<GarbageContainer> GarbageContainers;
    [SerializeField] private SavingLoading savingLoading;

    public int DefaultTimeDivisor;
    [HideInInspector] public int timeDivisor; //Containers replenish every x minutes where x = timeDivisor (I think)

    // Start is called before the first frame update
    void Start()
    {
        //Load time divisor from save data
        if(DefaultTimeDivisor == 0)
            DefaultTimeDivisor = 10;

        timeDivisor = savingLoading.LoadCollectionData();
        savingLoading.LoadContainerData(GarbageContainers);
        if (timeDivisor == 0)
            timeDivisor = DefaultTimeDivisor;
        StartCoroutine(TriggersEveryMinute());
    }

    IEnumerator TriggersEveryMinute()
    {
        while (true)
        {
            yield return new WaitForSeconds(60f);
            MinuteTick();
            savingLoading.SaveCollectionData(timeDivisor);
            savingLoading.SaveContainerData(GarbageContainers);
        }
    }

    void MinuteTick()
    {
        Debug.Log(DateTime.Now.Minute + " " + timeDivisor + " " + DateTime.Now.Minute%timeDivisor);
        if (DateTime.Now.Minute % timeDivisor == 0)
        {
            foreach (var container in GarbageContainers)
            {
                container.ReplenishStorage();
            }
        }
    }
}
