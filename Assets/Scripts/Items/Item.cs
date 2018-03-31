using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {

    public PaperdollSlot.PaperdollSlotType validSlot;

    public string itemName;
    public float itemValue;
    public Sprite itemIcon;

    public List<AbilityCard> abilityCards = new List<AbilityCard>();


    public int ItemID { get; private set; }
    public ItemData ItemData { get; protected set; }
    public bool Equipped { get; protected set; }


    public Item(ItemData data = null) {
        Initialize();

        if (data != null) {
            ItemData = data;
            SetUpItem();
            CreateAblityCards();
        }
        else {
            Debug.LogError("[Item] created with null ItemData");
        }
    }

    public PlayerAbilitySlot.SlotType GetCardSlotType() {
        switch (validSlot) {
            case PaperdollSlot.PaperdollSlotType.Weapon:
                return PlayerAbilitySlot.SlotType.Primary;

            default:
                return PlayerAbilitySlot.SlotType.Cycling;
        }
    }


    private void Initialize() {
        ItemID = IDFactory.GenerateItemID();
        ItemFactory.RegisterItem(this);
        RegisterEventListeners();
    }

    private void SetUpItem() {
        validSlot = ItemData.validSlot;
        itemName = ItemData.itemName;
        itemValue = ItemData.itemValue;
        itemIcon = ItemData.itemIcon;
    }

    private void CreateAblityCards() {
        int count = ItemData.itemCards.Count;

        for(int i = 0; i < count; i++) {
            AbilityCard card = CardFactory.CreateCard(ItemData.itemCards[i].abiliites, GameManager.GetPlayer(), ItemData.itemCards[i].cardSlotType, ItemID);

            //Debug.Log(ItemData.itemCards[i].abiliites.Count + " abilities found");

            abilityCards.Add(card);
        }
    }


    private void RegisterEventListeners() {
        EventGrid.EventManager.RegisterListener(Constants.GameEvent.ItemEquipped, OnEquip);
        EventGrid.EventManager.RegisterListener(Constants.GameEvent.ItemUnequipped, OnUnequip);
    }


    public void Equip(PaperdollSlot slot) {
        Equipped = true;
    }

    public void UnEquip(PaperdollSlot slot) {
        Equipped = false;
    }

    private void OnEquip(EventData data) {
        int itemID = data.GetInt("ItemID");
        Item item = ItemFactory.GetItemByID(itemID);

        //Debug.Log("Item with ID " + itemID + " has been equipped");
        if (item != this)
            return;

        //Debug.Log(itemName + " has been equipped");
    }

    private void OnUnequip(EventData data) {
        int itemID = data.GetInt("ItemID");
        Item item = ItemFactory.GetItemByID(itemID);

        if (item != this)
            return;

        //Debug.Log(itemName + " has been unequipped");
    }


    [System.Serializable]
    public class ItemAbilityCardData {
        public string cardName;
        public string cardDescripiton;
        public PlayerAbilitySlot.SlotType cardSlotType;
        //public List<ItemAbilitySet> itemCardAbilities = new List<ItemAbilitySet>();
        public List<SpecialAbilityData> abiliites = new List<SpecialAbilityData>();

        public ItemAbilityCardData() {

        }
    }


}
