using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    [SerializeField] private Image desiredItemImage;
    [SerializeField] private Button satisfyButton;

    private Inventory inventory;
    private Slider patienceSlider;

    public string DesiredItem;
    public int PatienceTime;

    void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        patienceSlider = GetComponentInChildren<Slider>();

        patienceSlider.maxValue = PatienceTime;
        patienceSlider.value = patienceSlider.maxValue;

        if (DesiredItem != "")
            desiredItemImage.sprite = ItemDataAccessor.Instance.GetItemList().FirstOrDefault(item => item.Name == DesiredItem).Sprite;
        else
            ChooseDesire();

        StartCoroutine(PatienceCountdownCoroutine());

    }

    void ChooseDesire()
    {
        Items items = new Items();
        int selection = Random.Range(0, items.ItemList.Count);
        DesiredItem = items.ItemList[selection].Name;
        desiredItemImage.sprite = ItemDataAccessor.Instance.GetItemList().FirstOrDefault(item => item.Name == DesiredItem).Sprite;
    }

    IEnumerator PatienceCountdownCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); //1f
            patienceSlider.value--;
            if (patienceSlider.value <= 0)
                LeaveAngrily();
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(inventory.CheckForItem(DesiredItem))
                satisfyButton.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
            satisfyButton.gameObject.SetActive(false);
    }

    public void Satisfy()
    {
        inventory.RemoveItem(DesiredItem);
        Destroy(gameObject);
    }

    void LeaveAngrily()
    {
        Destroy(gameObject);
    }
}
