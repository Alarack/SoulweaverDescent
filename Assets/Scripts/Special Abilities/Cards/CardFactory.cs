using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardFactory {


    public static AbilityCard CreateCard(SpecialAbilityData abilityData, Entity owner) {

        AbilityCard newCard = new AbilityCard(abilityData, owner);

        return newCard;
    }


}
