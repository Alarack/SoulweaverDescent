﻿using System.Collections;
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

    //public AbilityCard FindCardInDeck(AbilityCard card) {
    //    AbilityDeck library = GetDeck(AbilityDeck.DeckType.Library);
    //    AbilityDeck discardPile = GetDeck(AbilityDeck.DeckType.DiscardPile);

    //    if(library.activeCards)


    //}

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

}
