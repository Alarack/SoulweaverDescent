using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemFactory {

    public static List<Item> allItems = new List<Item>();

    public static Item CreateItem(ItemData item = null) {
        Item newItem = new Item(item);

        return newItem;
    }


    public static void RegisterItem(Item item) {
        allItems.Add(item);
    }

    public static Item GetItemByID(int id) {
        int count = allItems.Count;

        for(int i = 0; i < count; i++) {
            if (allItems[i].ItemID == id)
                return allItems[i];
        }

        return null;
    }


}
