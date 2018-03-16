using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler {

    public static Item draggedItem;

    public Image itemImage;
    public Item CurrentItem { get; private set; }
    public bool IsFull { get { return CurrentItem != null; }}
    public Camera uiCam;

    protected InventoryPanel inventoryPanel;
    protected Canvas itemCanvas;

    private void Start() {
        
        //Initialize();
        
    }

    public virtual void Initialize(InventoryPanel inventoryPanel) {
        itemCanvas = GetComponentInChildren<Canvas>();
        this.inventoryPanel = inventoryPanel;
        itemImage.gameObject.SetActive(false);
    }


    public virtual void AssignItem(Item item) {
        if(item == null) {
            Debug.LogError("[InventorySlot] was given a null item");
            return;
        }

        itemImage.gameObject.SetActive(true);
        CurrentItem = item;
        itemImage.sprite = item.itemIcon;
    }

    public virtual void RemoveItem() {
        if(CurrentItem == null) {
            Debug.LogError("[InventorySlot] was told to remove an item, but it had none");
            return;
        }

        CurrentItem = null;
        itemImage.sprite = null;
        itemImage.gameObject.SetActive(false);
    }

    public void TransferItem(InventorySlot destination) {
        if (destination.IsFull) {
            Debug.LogError("[InventorySlot] cannot transfer an item to a full slot");
            return;
        }

        if(destination is PaperdollSlot) {
            PaperdollSlot dollSlot = destination as PaperdollSlot;

            if(CurrentItem.validSlot != dollSlot.slotType) {
                Debug.Log(CurrentItem.itemName + " cannot be equipped to a " + dollSlot.slotType + " slot");
                return;
            }
        }

        destination.AssignItem(CurrentItem);
        RemoveItem();

    }



    #region DRAG AND DROP

    public void OnBeginDrag(PointerEventData eventData) {
        SetItemCanvasLayerOnTop();
        draggedItem = CurrentItem;
    }

    public void OnDrag(PointerEventData eventData) {
        itemImage.transform.position = uiCam.ScreenToWorldPoint( Input.mousePosition);
    }

    public void OnEndDrag(PointerEventData eventData) {
        itemImage.transform.localPosition = Vector2.zero;

        ResetItemCanvasLayer();
    }

    public virtual void OnDrop(PointerEventData eventData) {
        if (IsFull)
            return;

        //Debug.Log(gameObject.name + " is recieveing a drop " + InventorySlot.draggedItem.itemName);
        InventorySlot previousSlot = inventoryPanel.GetSlotByContents(draggedItem);

        if(previousSlot != null) {
            //Debug.Log("I, " + gameObject.name + ", wish to take " + draggedItem.itemName + " from " + previousSlot.gameObject.name);
            previousSlot.TransferItem(this);
        }
    }

    #endregion

    protected void SetItemCanvasLayerOnTop() {
        if (itemCanvas.sortingOrder == 100)
            return;

        itemCanvas.sortingOrder = 100;
    }

    protected void ResetItemCanvasLayer() {
        itemCanvas.sortingOrder = 6;
    }


}
