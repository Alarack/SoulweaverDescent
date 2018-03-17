using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardFactory {


    public static AbilityCard CreateCard(SpecialAbilityData abilityData, Entity owner, PlayerAbilitySlot.SlotType cardSlotType = PlayerAbilitySlot.SlotType.None) {

        AbilityCard newCard = new AbilityCard(abilityData, owner, cardSlotType);

        return newCard;
    }


}
