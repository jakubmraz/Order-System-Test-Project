using TMPro;
using UnityEngine;

public class ItemTooltip : MonoBehaviour
{
    public static ItemTooltip Instance { get; private set; }

    private Camera camera;
    [SerializeField] private RectTransform backgroundPanel;
    [SerializeField] private TextMeshProUGUI itemNameTMP;
    [SerializeField] private TextMeshProUGUI itemDescTMP;
    [SerializeField] private TextMeshProUGUI itemValueTMP;

    void Awake()
    {
        Instance = this;
        camera = FindObjectOfType<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HideTooltip();
        }
    }

    public void ShowTooltip(Item item)
    {
        backgroundPanel.gameObject.SetActive(true);
        itemNameTMP.text = item.itemData.Name;
        if (item.IsBroken) itemNameTMP.text = "Broken " + item.itemData.Name;
        itemDescTMP.text = item.itemData.Description;
        itemValueTMP.text = item.itemData.Value + " kr";

        transform.position = new Vector3(item.transform.position.x, item.transform.position.y + backgroundPanel.rect.height*1.3f, item.transform.position.z);
        Debug.Log(transform.position.x + backgroundPanel.rect.width + " " + Screen.width + " " + CheckIfWithinScreenBoundaries());
        if(!CheckIfWithinScreenBoundaries())
            transform.position = new Vector3(item.transform.position.x - backgroundPanel.rect.width, item.transform.position.y + backgroundPanel.rect.height * 1.3f, item.transform.position.z);
    }

    public void HideTooltip()
    {
        backgroundPanel.gameObject.SetActive(false);
    }

    private bool CheckIfWithinScreenBoundaries()
    {
        if (transform.position.x + backgroundPanel.rect.width > Screen.width) return false;
        if (transform.position.x < 0) return false;
        if (transform.position.y < 0) return false;
        if (transform.position.y + backgroundPanel.rect.height > Screen.height) return false;
        return true;
    }
}
