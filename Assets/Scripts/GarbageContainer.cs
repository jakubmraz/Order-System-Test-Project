using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GarbageContainer : MonoBehaviour
{
    [SerializeField] private Button collectButton;
    [SerializeField] private GarbageCollection garbageCollection;

    public string ContainedItem;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            garbageCollection.containedItem = ContainedItem;
            collectButton.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collectButton.gameObject.SetActive(false);
        }
    }
}
