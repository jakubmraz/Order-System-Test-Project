using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop2 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private float shortClickThreshold = 0.5f;
    private bool shortClick;
    private bool longClick;
    private bool dragging;
    private int draggedAmount;

    // Reference to current item slot.
    public ItemSlot currentSlot;
    // Reference to the canvas.
    private Canvas canvas;
    // Reference to UI raycaster.
    private GraphicRaycaster graphicRaycaster;
    private RectTransform rectTransform;
    private CraftingSystem craftingSystem;
    private Inventory inventory;
    private GarbageCollection garbageCollection;
    private Item item;

    public ItemSlot lastSlot;
    public bool wasStack;
    private bool hitItself;

    void Awake()
    {
        currentSlot = GetComponentInParent<ItemSlot>();
        rectTransform = GetComponent<RectTransform>();
        craftingSystem = GetComponentInParent<CraftingSystem>();
        inventory = GetComponentInParent<Inventory>();
        garbageCollection = GetComponentInParent<GarbageCollection>();
        item = GetComponent<Item>();
    }

    /// <summary>
    /// IBeginDragHandler
    /// Method called on drag begin.
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        //StopCoroutine(ShortClickCoroutine());
        //shortClick = false;

        dragging = true;
        //StopCoroutine(LongClickCoroutine());
        StopAllCoroutines();
        bool wasLongClick = false;
        draggedAmount = 1;
        if(longClick)
        {
            wasLongClick = true;
            draggedAmount = currentSlot.Item.count;
            longClick = false;
        }

        bool broken = currentSlot.Item.IsBroken;

        lastSlot = currentSlot;

        if (currentSlot.IsResultSlot)
        {
            craftingSystem.CraftNewItem();
        }

        if(currentSlot.Item.count > 1 && !wasLongClick)
        {
            lastSlot.Item = Instantiate(lastSlot.itemPrefab, lastSlot.transform).GetComponent<Item>();
            lastSlot.Item.InitializeItem(item.itemData.Name);
            if (broken) lastSlot.Item.BreakItem();
            lastSlot.Item.count = item.count - 1;
            lastSlot.Item.UpdateCountText();
            wasStack = true;

            item.count = 1;
            item.UpdateCountText();
        }

        if (!canvas)
        {
            canvas = GetComponentInParent<Canvas>();
            graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
        }
        // Change parent of our item to the canvas.
        transform.SetParent(canvas.transform, true);
        // And set it as last child to be rendered on top of UI.
        transform.SetAsLastSibling();
    }
    /// <summary>
    /// IDragHandler
    /// Method called on drag continuously.
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnDrag(PointerEventData eventData)
    {
        // Continue moving object around screen.
        //transform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y, 0) / transform.lossyScale.x; // Thanks to the canvas scaler we need to devide pointer delta by canvas scale to match pointer movement.
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, canvas.worldCamera, out pos);
        rectTransform.transform.position = canvas.transform.TransformPoint(pos);
    }
    /// <summary>
    /// IEndDragHandler
    /// Method called on drag end.
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnEndDrag(PointerEventData eventData)
    {
        // On end we need to test if we can drop item into new slot.
        var results = new List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, results);
        // Check all hits.
        foreach (var hit in results)
        {
            // If we found slot.
            var slot = hit.gameObject.GetComponent<ItemSlot>();
            if (slot)
            {
                // We should check if we can place ourselves​ there.
                if (slot.Item == null && !slot.IsResultSlot && !slot.IsCollectionSlot && (!slot.IsRecycleSlot || (item.itemData.Recipe != "000000000" && item.IsBroken)))
                {
                    // Swapping references.
                    if(!wasStack) currentSlot.Item = null;
                    currentSlot = slot;
                    currentSlot.Item = this.GetComponent<Item>();
                    if(craftingSystem)
                        craftingSystem.Craft();
                }

                else if (slot.Item != null && !slot.IsResultSlot && !slot.IsCollectionSlot && !currentSlot.IsCollectionSlot && !currentSlot.IsResultSlot && (!slot.IsRecycleSlot || (item.itemData.Recipe != "000000000" && item.IsBroken)))
                {
                    //Stack
                    if(currentSlot.Item.itemData == slot.Item.itemData && currentSlot.Item.IsBroken == slot.Item.IsBroken && currentSlot.Item.count + slot.Item.count <= Inventory.MaxStackCount)
                    {
                        if (wasStack || slot != currentSlot)
                        {
                            slot.Item.count += draggedAmount;
                            slot.Item.UpdateCountText();
                            if (wasStack) hitItself = true;
                        }
                        
                        if(slot == currentSlot && !wasStack)
                        {
                            break;
                        }

                        if(wasStack)
                        {
                            Destroy(this.gameObject);
                            break;
                        }

                        Destroy(currentSlot.Item.gameObject);
                        if(!wasStack) currentSlot.Item = null;
                        
                        break;
                    }

                    //Can't swap if from stack
                    if(slot.Item.itemData != currentSlot.Item.itemData && wasStack)
                    {
                        break;
                    }
                    if(slot.Item.IsBroken != currentSlot.Item.IsBroken && wasStack)
                    {
                        break;
                    }

                    //Swap
                    currentSlot.Item = slot.Item;
                    currentSlot.Item.transform.SetParent(currentSlot.transform);
                    currentSlot.Item.transform.localPosition = Vector3.zero;
                    DragDrop2 drag = currentSlot.Item.GetComponent<DragDrop2>();
                    drag.currentSlot = currentSlot;
                    currentSlot = slot;
                    currentSlot.Item = this.GetComponent<Item>();
                    if (craftingSystem)
                        craftingSystem.Craft();
                }
                // In either cases we should break check loop.
                break;
            }
        }

        if (currentSlot.IsResultSlot || currentSlot.IsCollectionSlot)
        {
            ItemSlot emptySlot = inventory.FindFirstEmptySlot();
            emptySlot.Item = currentSlot.Item;
            currentSlot.Item = null;
            currentSlot = emptySlot;
        }

        if (lastSlot.IsCollectionSlot)
        {
            garbageCollection.RespawnItem();
        }

        RecyclingSystem recyclingSystem;

        try
        {
            recyclingSystem = FindObjectOfType<RecyclingSystem>();

            if (!recyclingSystem.CheckIfResultSlotsEmpty())
            {
                recyclingSystem.DisableRecycleButton();
            }
            else if(recyclingSystem.CheckIfEntrySlotFilled())
            {
                recyclingSystem.EnableRecycleButton();
            }

        }
        catch
        {
            Debug.Log("Recycling system script not found.");
        }

        // Changing parent back to slot.
        transform.SetParent(currentSlot.transform);
        // And centering item position.
        transform.localPosition = Vector3.zero;

        //Isn't dragged onto anything
        if(currentSlot == lastSlot && currentSlot != null && wasStack && !hitItself)
        {
            lastSlot.Item.count++;
            lastSlot.Item.UpdateCountText();
            Destroy(this.gameObject);
        }

        wasStack = false;
        hitItself = false;
        draggedAmount = 1;
        item.SetGlow(false);
        dragging = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        /*
        if (shortClick)
        {
            ItemTooltip.Instance.ShowTooltip(item);
        }

        StopCoroutine(ShortClickCoroutine());
        shortClick = false;
        */
        longClick = false;
        item.SetGlow(false);
        //(LStopCoroutineongClickCoroutine());
        StopAllCoroutines();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //StartCoroutine(ShortClickCoroutine());
        StartCoroutine(LongClickCoroutine());
    }

    private IEnumerator ShortClickCoroutine()
    {
        shortClick = true;
        yield return new WaitForSeconds(shortClickThreshold);
        shortClick = false;
    }

    private IEnumerator LongClickCoroutine()
    {
        longClick = false;
        Debug.Log("1");
        yield return new WaitForSeconds(shortClickThreshold);
        longClick = true;
        if(!dragging) item.SetGlow(true);
        Debug.Log("2");
    }
}
