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
        SystemGrid.EventManager.RegisterListener(Constants.GameEvent.ItemAquired, OnItemAquired);
        SystemGrid.EventManager.RegisterListener(Constants.GameEvent.ItemRemoved, OnItemRemoved);
    }

    private void OnDisable() {
        SystemGrid.EventManager.RemoveMyListeners(this);
    }

    private void GetSlots() {
        slots = GetComponentsInChildren<InventorySlot>().ToList();

        int count = slots.Count;

        for (int i = 0; i < count; i++) {
            slots[i].Initialize(this);
        }
    }

    public InventorySlot GetFirstEmptySlot() {
        int count = slots.Count;

        for(int i = 0; i < count; i++) {
            if (slots[i].IsFull == false)
                return slots[i];
        }

        return null;
    }

    public PaperdollSlot GetPaperDollSlotByType(PaperdollSlot.PaperdollSlotType slotType) {
        int count = slots.Count;

        for (int i = 0; i < count; i++) {
            if(slots[i] is PaperdollSlot) {
                PaperdollSlot slot = slots[i] as PaperdollSlot;

                if (slot.slotType == slotType && slot.IsFull == false)
                    return slot;
            }
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

        int itemID = data.GetInt("ItemID");
        Item item = ItemFactory.GetItemByID(itemID);

        
        if(GetSlotByContents(item) != null) {
            Debug.LogError("[Inventory Panel] an item " + item.itemName + " is already being displayed");
            return;
        }

        emptySlot.AssignItem(item);
    }

    public void OnItemRemoved(EventData data) {
        int itemID = data.GetInt("ItemID");
        Item item = ItemFactory.GetItemByID(itemID);
        InventorySlot currentSlot = GetSlotByContents(item);

        currentSlot.RemoveItem();
    }

    #endregion


}
