using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PaperdollSlot : InventorySlot, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler {

    public enum PaperdollSlotType {
        None = 0,
        Weapon = 1,
        Armor = 2,
        Helm = 3,
        Ring = 4,
        Neck = 5,
    }

    public PaperdollSlotType slotType;
    public Image fadeout;



    public override void AssignItem(Item item) {
        base.AssignItem(item);

        Debug.Log("Item with ID " + item.ItemID + " has been assigned to " + slotType + " slot");

        Debug.Log("My current item's id is " + CurrentItem.ItemID);

        fadeout.gameObject.SetActive(false);
        EventData data = new EventData();
        data.AddInt("ItemID", CurrentItem.ItemID);
        EventGrid.EventManager.SendEvent(Constants.GameEvent.ItemEquipped, data);

    }

    public override void RemoveItem() {
        fadeout.gameObject.SetActive(true);
        EventData data = new EventData();
        data.AddInt("ItemID", CurrentItem.ItemID);
        EventGrid.EventManager.SendEvent(Constants.GameEvent.ItemUnequipped, data);

        base.RemoveItem();
    }

}
