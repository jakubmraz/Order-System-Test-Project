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
        //Debug.Log(GetTooltipPosition());
        //Debug.Log(CheckIfWithinScreenBoundaries());
        switch (CheckIfWithinScreenBoundaries())
        {
            case "":
                break;
            case "up":
                transform.position = new Vector3(item.transform.position.x, item.transform.position.y - backgroundPanel.rect.height * 0.3f, item.transform.position.z);
                break;
            case "down":
                break;
            case "left":
                break;
            case "right":
                transform.position = new Vector3(item.transform.position.x - backgroundPanel.rect.width, item.transform.position.y + backgroundPanel.rect.height * 1.3f, item.transform.position.z);
                break;
        }
    }

    public void HideTooltip()
    {
        backgroundPanel.gameObject.SetActive(false);
    }

    private string CheckIfWithinScreenBoundaries()
    {
        Vector3 position = GetTooltipPosition();

        if (position.x + backgroundPanel.rect.width > Screen.width) return "right";
        if (position.x < 0) return "left";
        if (position.y > 0) return "up";
        if (position.y - backgroundPanel.rect.height < -Screen.height) return "down";
        return "";
    }

    private Vector3 GetTooltipPosition()
    {
        RectTransform canvas = (RectTransform) transform.parent;
        return new Vector3(transform.localPosition.x + canvas.rect.width/2, transform.localPosition.y - canvas.rect.height/2, 0);
    }
}
