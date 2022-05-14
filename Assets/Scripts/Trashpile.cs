using System;
using UnityEngine;

public class Trashpile : MonoBehaviour
{
    public DateTime PickedUpTime;

    public void OnTriggerEnter(Collider other)
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            PickedUpTime = DateTime.Now;
            TrashManager.Instance.UpdatePickupArray();
        }
    }

    public void RespawnPile()
    {
        gameObject.SetActive(true);
        PickedUpTime = default;
    }
}
