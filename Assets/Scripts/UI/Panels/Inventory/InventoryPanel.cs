using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryPanel : BasePanel {

    public bool IsFull { get { return GetFirstEmptySlot() == null; } }

    private List<InventorySlot> slots = new List<InventorySlot>();



    public override void Initialize(PanelManager panelManager) {
        base.Initialize(panelManager);
        GetSlots();
        RegisterEventListeners();
    }

    private void RegisterEventListeners() {
        EventGrid.EventManager.RegisterListener(Constants.GameEvent.ItemAquired, OnItemAquired);
        EventGrid.EventManager.RegisterListener(Constants.GameEvent.ItemRemoved, OnItemRemoved);
    }

    private void OnDisable() {
        EventGrid.EventManager.RemoveMyListeners(this);
    }

    private void GetSlots() {
        slots = GetComponentsInChildren<InventorySlot>().ToList();
    }

    public InventorySlot GetFirstEmptySlot() {
        int count = slots.Count;

        for(int i = 0; i < count; i++) {
            if (slots[i].IsFull == false)
                return slots[i];
        }

        return null;
    }

    public InventorySlot GetSlotByContents(Item item) {
        int count = slots.Count;

        for (int i = 0; i < count; i++) {
            if (slots[i].CurrentItem == item)
                return slots[i];
        }

        return null;
    }


    #region EVENTS

    public void OnItemAquired(EventData data) {
        InventorySlot emptySlot = GetFirstEmptySlot();
        if (IsFull) {
            Debug.Log("Inventory is Full");
            return;
        }

        Item item = data.GetMonoBehaviour("Item") as Item;

        emptySlot.AssignItem(item);
    }

    public void OnItemRemoved(EventData data) {
        Item item = data.GetMonoBehaviour("Item") as Item;
        InventorySlot currentSlot = GetSlotByContents(item);

        currentSlot.RemoveItem(item);
    }

    #endregion


}
