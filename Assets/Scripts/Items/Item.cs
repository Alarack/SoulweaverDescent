using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {

    public PaperdollSlot.PaperdollSlotType validSlot;

    public string itemName;
    public float itemValue;

    public Sprite itemIcon;

    public int ItemID { get; private set; }


    public void Initialize() {
        ItemID = IDFactory.GenerateItemID();

        ItemFactory.RegisterItem(this);

        RegisterEventListeners();
    }

    private void RegisterEventListeners() {
        EventGrid.EventManager.RegisterListener(Constants.GameEvent.ItemEquipped, OnEquip);
        EventGrid.EventManager.RegisterListener(Constants.GameEvent.ItemUnequipped, OnUnequip);
    }

    private void OnEquip(EventData data) {
        Item item = ItemFactory.GetItemByID(data.GetInt("ItendID"));

        if (item != this)
            return;

    }

    private void OnUnequip(EventData data) {
        Item item = ItemFactory.GetItemByID(data.GetInt("ItendID"));

        if (item != this)
            return;

    }


}
