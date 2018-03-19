using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDeckManager : MonoBehaviour {

    [Header("Decks")]
    public List<DeckEntry> allDecks = new List<DeckEntry>();


    public float drawTime;

    private Timer drawTimer;
    private Entity owner;

    public AbilityDeck Hand { get; private set; }


    public void Initialize(Entity owner) {
        this.owner = owner;
        drawTimer = new Timer(drawTime, true, DrawCardFromLibrary);

        FindDecks();
        InitDecks();

        Hand = GetDeck(AbilityDeck.DeckType.Hand);

        RegisterEventListeners();
    }

    private void RegisterEventListeners() {
        EventGrid.EventManager.RegisterListener(Constants.GameEvent.ItemEquipped, OnItemEquipped);
        EventGrid.EventManager.RegisterListener(Constants.GameEvent.ItemUnequipped, OnItemUnequipped);
    }


    #region EVENTS

    private void OnItemEquipped(EventData data) {
        int itemID = data.GetInt("ItemID");
        Item item = ItemFactory.GetItemByID(itemID);

        List<AbilityCard> itemCards = GetCardSetByItemID(itemID);
        AddCardSet(itemCards, item.GetCardSlotType());

        //Debug.Log("Adding " + itemCards.Count + " cards to " + item.GetCardSlotType() + " from " + item.itemName);

    }

    private void OnItemUnequipped(EventData data) {
        int itemID = data.GetInt("ItemID");
        Item item = ItemFactory.GetItemByID(itemID);

        List<AbilityCard> itemCards = GetCardSetByItemID(itemID);

        RemoveCardSet(itemCards);

        //Debug.Log("Removing " + itemCards.Count + " cards from " + item.GetCardSlotType() + " from " + item.itemName);
    }


    #endregion



    //private void SwapCardSet(List<AbilityCard> inCards, PlayerAbilitySlot.SlotType slot) {
    //    List<AbilityCard> outCards = GetAllCardsBySlotType(slot);

    //    RemoveCardSet(outCards);
    //    AddCardSet(inCards, slot);
    //}

    private void AddCardSet(List<AbilityCard> cards, PlayerAbilitySlot.SlotType slot) {
        switch (slot) {
            case PlayerAbilitySlot.SlotType.Cycling:
                TransferCardSet(cards, GetDeck(AbilityDeck.DeckType.Library));
                break;

            case PlayerAbilitySlot.SlotType.Primary:
                TransferCardSet(cards, GetDeck(AbilityDeck.DeckType.PrimaryCards));
                break;
        }
    }

    private void RemoveCardSet(List<AbilityCard> cards) {
        TransferCardSet(cards, AbilityDeck.NOT_IN_GAME);
    }

    private List<AbilityCard> GetAllCardsBySlotType(PlayerAbilitySlot.SlotType slot) {
        List<AbilityCard> results = new List<AbilityCard>();

        int count = AbilityDeck.ALL_CARDS.activeCards.Count;

        for(int i = 0; i < count; i++) {
            if(AbilityDeck.ALL_CARDS.activeCards[i].cardSlotType == slot) {
                results.Add(AbilityDeck.ALL_CARDS.activeCards[i]);
            }
        }

        return results;
    }

    private List<AbilityCard> GetCardSetByItemID(int id) {
        List<AbilityCard> results = new List<AbilityCard>();

        int count = AbilityDeck.ALL_CARDS.activeCards.Count;

        for (int i = 0; i < count; i++) {
            if (AbilityDeck.ALL_CARDS.activeCards[i].ParentItemID == id) {
                results.Add(AbilityDeck.ALL_CARDS.activeCards[i]);
            }
        }

        return results;
    }

    private void TransferCardSet(List<AbilityCard> cards, AbilityDeck destination) {
        int count = cards.Count;
        
        for(int i = 0; i < count; i++) {
            if(cards[i].currentDeck == null) {
                destination.Addcard(cards[i]);
                //Debug.Log(cards[i] + " had no current deck. Assigning: " + destination.deckType);
            }
            else {
                //Debug.Log(cards[i] + " is being transfered to " + destination.deckType);
                cards[i].currentDeck.TransferCard(cards[i], destination);
            }
        }
    }


    private void FindDecks() {
        AbilityDeck[] decks = GetComponentsInChildren<AbilityDeck>();

        int count = decks.Length;

        for (int i = 0; i < count; i++) {
            allDecks.Add(new DeckEntry(decks[i], decks[i].deckType));
        }
    }

    private void InitDecks() {
        int count = allDecks.Count;

        for(int i = 0; i < count; i++) {
            allDecks[i].deck.Initialize(owner, this);
        }
    }

    private void Update() {
        if(Hand.IsDeckFull() == false) {
            drawTimer.UpdateClock();
        }
    }

    public void DrawCardFromLibrary() {
        AbilityDeck library = GetDeck(AbilityDeck.DeckType.Library);

        AbilityCard targetCard = library.Draw();

        if(targetCard == null) {
            //Debug.Log("Library is empty");
            return;
        }

        //Debug.Log(targetCard.cardName + " is being drawn ");
        //Debug.Log(library + " is my lbrary");

        library.TransferCard(targetCard, AbilityDeck.DeckType.Hand);

        //MainHUD.SetPlayerSlot(targetCard);

    }

    public AbilityDeck GetDeck(AbilityDeck.DeckType type) {
        int count = allDecks.Count;

        for(int i = 0; i < count; i++) {
            if (allDecks[i].deckType == type)
                return allDecks[i].deck;
        }

        return null;
    }

    public static bool IsCardInUse() {
        int count = AbilityDeck.ALL_CARDS.activeCards.Count;

        for (int i = 0; i < count; i++) {
            if (AbilityDeck.ALL_CARDS.activeCards[i].InUse)
                return true;
        }

        return false;
    }



    [System.Serializable]
    public struct DeckEntry {
        public AbilityDeck deck;
        public AbilityDeck.DeckType deckType;

        public DeckEntry(AbilityDeck deck, AbilityDeck.DeckType deckType) {
            this.deck = deck;
            this.deckType = deckType;
        }
    }

    [System.Serializable]
    public struct AbilityCardSet {
        public List<AbilityCard> cards;
        public PlayerAbilitySlot.SlotType slotType;

        public AbilityCardSet(List<AbilityCard> cards, PlayerAbilitySlot.SlotType slotType) {
            this.cards = cards;
            this.slotType = slotType;
        }

        public void RemoveSet() {
            int count = cards.Count;

            for(int i = 0; i < count; i++) {
                cards[i].currentDeck.TransferCard(cards[i], AbilityDeck.NOT_IN_GAME);
            }
        }
    }

}
