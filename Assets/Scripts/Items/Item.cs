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
        int count = ItemData.itemAbilities.Count;

        for(int i = 0; i < count; i++) {
            Debug.Log(GameManager.GetPlayer() + " is player");

            AbilityCard card = CardFactory.CreateCard(ItemData.itemAbilities[i].ability, GameManager.GetPlayer(), ItemData.itemAbilities[i].cardSlotType);

            abilityCards.Add(card);
        }
    }


    private void RegisterEventListeners() {
        EventGrid.EventManager.RegisterListener(Constants.GameEvent.ItemEquipped, OnEquip);
        EventGrid.EventManager.RegisterListener(Constants.GameEvent.ItemUnequipped, OnUnequip);
    }

    private void OnEquip(EventData data) {
        int itemID = data.GetInt("ItemID");
        Item item = ItemFactory.GetItemByID(itemID);

        //Debug.Log("Item with ID " + itemID + " has been equipped");
        if (item != this)
            return;

        Debug.Log(itemName + " has been equipped");
    }

    private void OnUnequip(EventData data) {
        int itemID = data.GetInt("ItemID");
        Item item = ItemFactory.GetItemByID(itemID);

        if (item != this)
            return;

        Debug.Log(itemName + " has been unequipped");
    }


    [System.Serializable]
    public class ItemAbility {

        public SpecialAbilityData ability;
        public PlayerAbilitySlot.SlotType cardSlotType;
        public bool hasAbilityCard;


        public ItemAbility() {

        }

        public ItemAbility(SpecialAbilityData ability, PlayerAbilitySlot.SlotType cardSlotType, bool hasAbilityCard) {
            this.ability = ability;
            this.cardSlotType = cardSlotType;
            this.hasAbilityCard = hasAbilityCard;
        }

    }


}
