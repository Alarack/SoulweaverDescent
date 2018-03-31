using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public List<ItemData> startingItems = new List<ItemData>();

    private List<Item> items = new List<Item>();


    private PanelManager panelManager;
    private InventoryPanel inventoryPanel;


    public void Initialize() {
        panelManager = MainHUD.GetPanelManager();
        inventoryPanel = (InventoryPanel)panelManager.GetPanelByType(BasePanel.PanelType.Inventory);

        //Debug.Log(panelManager + " is manager");
        //Debug.Log(inventoryPanel + " is inventory");

        SetupStartingItems();
    }

    private void SetupStartingItems() {
        int count = startingItems.Count;

        for(int i = 0; i < count; i++) {
            Item newItem = ItemFactory.CreateItem(startingItems[i]);
            AddItem(newItem);



            PaperdollSlot targetSlot = inventoryPanel.GetPaperDollSlotByType(newItem.validSlot);
            InventorySlot currentSlot = inventoryPanel.GetSlotByContents(newItem);

            currentSlot.TransferItem(targetSlot);

        }
    }


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
