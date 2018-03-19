using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SlotType = PlayerAbilitySlot.SlotType;

public static class CardFactory {


    public static AbilityCard CreateCard(Entity owner, SlotType cardSlotType = SlotType.None, int itemID = -1) {

        AbilityCard newCard = new AbilityCard(owner, cardSlotType, itemID);

        return newCard;
    }

    public static AbilityCard CreateCard(List<SpecialAbilityData> abilities, Entity owner, SlotType cardSlotType = SlotType.None, int itemID = -1) {

        //Debug.Log("Create Card is recieving " + abilities.Count + " abilities in the card factory");

        AbilityCard newCard = new AbilityCard(abilities, owner, cardSlotType, itemID);

        return newCard;

    }


}
