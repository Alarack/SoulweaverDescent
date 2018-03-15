using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    private List<Item> items = new List<Item>();






    public Item GetItem(Item item) {
        if (items.Contains(item))
            return item;

        return null;
    }

    public Item GetItem(int itemID) {
        int count = items.Count;

        for(int i = 0; i < count; i++) {
            if (items[i].ItemID == itemID)
                return items[i];
        }

        return null;
    }

    public void AddItem(Item item) {
        if (items.AddUnique(item)) {
            EventData data = new EventData();
            data.AddInt("ItemID", item.ItemID);

            EventGrid.EventManager.SendEvent(Constants.GameEvent.ItemAquired, data);
        }
    }

    public void RemoveItem(Item item) {
        if (items.RemoveIfContains(item)) {
            EventData data = new EventData();
            data.AddInt("ItemID", item.ItemID);

            EventGrid.EventManager.SendEvent(Constants.GameEvent.ItemRemoved, data);
        }
    }



}
