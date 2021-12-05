using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingStation : MonoBehaviour
{
    //Originally made for only the crafting station; however, the code is reusable for other stations too

    [SerializeField] private Button upcycleButton;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            upcycleButton.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            upcycleButton.gameObject.SetActive(false);
        }
    }
}
