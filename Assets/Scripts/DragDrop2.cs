using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop2 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
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

    void Awake()
    {
        currentSlot = GetComponentInParent<ItemSlot>();
        rectTransform = GetComponent<RectTransform>();
        craftingSystem = GetComponentInParent<CraftingSystem>();
        inventory = GetComponentInParent<Inventory>();
        garbageCollection = GetComponentInParent<GarbageCollection>();
    }

    /// <summary>
    /// IBeginDragHandler
    /// Method called on drag begin.
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentSlot.IsResultSlot)
        {
            craftingSystem.CraftNewItem();
        }

        if (currentSlot.IsCollectionSlot)
        {
            garbageCollection.RespawnItem();
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
                if (slot.Item == null && !slot.IsResultSlot && !slot.IsCollectionSlot)
                {
                    // Swapping references.
                    currentSlot.Item = null;
                    currentSlot = slot;
                    currentSlot.Item = this.GetComponent<Item>();
                    craftingSystem.Craft();
                }

                else if (slot.Item != null && !slot.IsResultSlot && !slot.IsCollectionSlot)
                {
                    currentSlot.Item = slot.Item;
                    currentSlot.Item.transform.SetParent(currentSlot.transform);
                    currentSlot.Item.transform.localPosition = Vector3.zero;
                    DragDrop2 drag = currentSlot.Item.GetComponent<DragDrop2>();
                    drag.currentSlot = currentSlot;
                    currentSlot = slot;
                    currentSlot.Item = this.GetComponent<Item>();
                    craftingSystem.Craft();
                }
                // In either cases we should break check loop.
                break;
            }
        }
        // Changing parent back to slot.
        transform.SetParent(currentSlot.transform);
        // And centering item position.
        transform.localPosition = Vector3.zero;
    }
}
