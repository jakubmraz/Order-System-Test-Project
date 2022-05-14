using System.Collections;
using System;
using UnityEngine;

public class TrashManager : MonoBehaviour
{
    public static TrashManager Instance { get; private set; }

    private const int TrashRespawnTime = 3; //minutes

    [SerializeField] private GameObject trashpileHolder;
    private Trashpile[] trashpiles;
    private DateTime[] pickups;

    private void Awake()
    {
        Instance = this;        
    }

    private void Start()
    {
        trashpiles = trashpileHolder.GetComponentsInChildren<Trashpile>();
        pickups = new DateTime[trashpiles.Length];
        CheckForTrash();

        if(LoadPickupTimes(out DateTime[] loaded))
        {
            pickups = loaded;

            for(int i = 0; i < pickups.Length; i++)
            {
                if((DateTime.Now - pickups[i]).TotalMinutes < TrashRespawnTime)
                {
                    trashpiles[i].gameObject.SetActive(false);
                }
            }
        }
    }

    private void CheckForTrash()
    {
        StartCoroutine(CheckCoroutine());

        IEnumerator CheckCoroutine()
        {
            while (true)
            {
                foreach (Trashpile pile in trashpiles)
                {
                    if (!pile.gameObject.activeInHierarchy)
                    {
                        if ((DateTime.Now - pile.PickedUpTime).TotalMinutes > TrashRespawnTime)
                        {
                            pile.RespawnPile();
                        }
                    }
                }

                yield return new WaitForSeconds(60f);
            }            
        }
    }

    public void UpdatePickupArray()
    {
        for(int i = 0; i < trashpiles.Length; i++)
        {
            pickups[i] = trashpiles[i].PickedUpTime;
        }

        SavePickupTimes();
    }

    private void SavePickupTimes()
    {
        JsonDateTime[] serializablePickups = new JsonDateTime[pickups.Length];

        for(int i = 0; i < pickups.Length; i++)
        {
            serializablePickups[i] = pickups[i];
        }

        Pickupholder holder = new Pickupholder()
        {
            Pickups = serializablePickups
        };

        string pickupJson = JsonUtility.ToJson(holder);
        Debug.Log(pickupJson);
        PlayerPrefs.SetString("TrashPickups", pickupJson);
    }

    private bool LoadPickupTimes(out DateTime[] pickups)
    {
        pickups = new DateTime[trashpiles.Length];

        if (!PlayerPrefs.HasKey("TrashPickups"))
        {
            return false;
        }

        Pickupholder holder = JsonUtility.FromJson<Pickupholder>(PlayerPrefs.GetString("TrashPickups"));

        for (int i = 0; i < pickups.Length; i++)
        {
            pickups[i] = holder.Pickups[i];
        }

        return true;
    }

    [Serializable]
    class Pickupholder
    {
        public JsonDateTime[] Pickups;
    }

    [Serializable]
    struct JsonDateTime
    {
        public string value;
        public static implicit operator DateTime(JsonDateTime jdt)
        {
            return DateTime.Parse(jdt.value);
        }
        public static implicit operator JsonDateTime(DateTime dt)
        {
            JsonDateTime jdt = new JsonDateTime();
            jdt.value = dt.ToString();
            return jdt;
        }
    }
}
