using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDeck : MonoBehaviour {

    public enum DeckType {
        None = 0,
        Hand = 1,
        Library = 2,
        DiscardPile = 3,
        Void = 4,
        NotInGame = 5,
        AllCards = 6,
        PrimaryCards = 7
    }

    public DeckType deckType;
    public int maxSize;

    public List<CardDataEntry> cardDataEntires = new List<CardDataEntry>();

    public List<SpecialAbilityData> abilityData = new List<SpecialAbilityData>();


    public List<AbilityCard> activeCards = new List<AbilityCard>();

    public static AbilityDeck ALL_CARDS;

    private Entity owner;
    private AbilityDeckManager deckManager;

    private void Awake() {
        if (deckType == DeckType.AllCards)
            ALL_CARDS = this;
    }


    public void Initialize(Entity owner, AbilityDeckManager deckManager) {
        this.owner = owner;
        this.deckManager = deckManager;

        PopulateCards();
    }

    private void Update() {
        int count = activeCards.Count;

        for (int i = 0; i < count; i++) {
            activeCards[i].ManagedUpdate();
        }

    }



    private void PopulateCards() {
        //int count = abilityData.Count;

        //for (int i = 0; i < count; i++) {
        //    AbilityCard card = CardFactory.CreateCard(abilityData[i], owner);
        //    card.currentDeck = this;

        //    Addcard(card);
        //}

        int count1 = cardDataEntires.Count;

        for(int i =0; i < count1; i++) {
            AbilityCard card = CardFactory.CreateCard(cardDataEntires[i].data[0], owner);
            card.currentDeck = this;
            Addcard(card);

            int count2 = cardDataEntires[i].data.Count;
            if (count2 > 1) {
                for (int j = 1; j < count2; j++) {
                    card.AddAbility(cardDataEntires[i].data[j]);
                }
            }


        }


    }


    public bool IsDeckFull() {
        return activeCards.Count >= maxSize && maxSize > 0;
    }

    public AbilityCard Draw() {
        if (activeCards.Count < 1)
            return null;

        return activeCards[0];
    }

    public void Shuffle() {
        activeCards.Shuffle();
    }

    public void Addcard(AbilityCard card) {
        activeCards.Add(card);

        switch (deckType) {
            case DeckType.Hand:
                MainHUD.SetPlayerSlot(card, PlayerAbilitySlot.SlotType.Cycling);
                break;

            case DeckType.PrimaryCards:
                MainHUD.SetPlayerSlot(card, PlayerAbilitySlot.SlotType.Primary);
                break;
        }
    }

    public void RemoveCard(AbilityCard card) {

        if (activeCards.RemoveIfContains(card)) {
            if (deckType == DeckType.Hand) {
                MainHUD.ClearPlayerSlot(card);
            }
        }
    }

    public void PlayCard(AbilityCard card, Controller2D.CollisionInfo colInfo, PlayerAbilitySlot.SlotType slotType = PlayerAbilitySlot.SlotType.Cycling) {

        //if(AbilityDeckManager.IsCardInUse() == true && card.ability.overrideOtherAbilities == false) {
        //    Debug.Log("Another Ability is in use");
        //    return;
        //}

        card.Activate(colInfo);

        if(slotType == PlayerAbilitySlot.SlotType.Cycling)
            TransferCard(card, DeckType.Library);
    }


    public void TransferCard(AbilityCard card, DeckType destination) {
        AbilityDeck targetDeck = deckManager.GetDeck(destination);

        if (targetDeck.IsDeckFull()) {
            Debug.Log("Target Deck is Full");
            return;
        }

        card.previousDeck = this;
        card.currentDeck = targetDeck;

        RemoveCard(card);
        targetDeck.Addcard(card);

    }



    [System.Serializable]
    public struct CardDataEntry {
        public List<SpecialAbilityData> data;

        //public CardDataEntry() {
        //    data = new List<SpecialAbilityData>();
        //}
    }

}
