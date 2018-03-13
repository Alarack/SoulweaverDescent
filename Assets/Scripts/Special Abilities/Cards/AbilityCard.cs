using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityCard {

    public string cardName;
    public bool InUse { get { return IsAbilityInUse(); } }
    //public SpecialAbility ability;

    public List<SpecialAbility> abiliites = new List<SpecialAbility>();

    public AbilityDeck currentDeck;
    public AbilityDeck previousDeck;

    private Entity source;

    public AbilityCard(SpecialAbilityData abilityData, Entity source) {
        this.source = source;

        //ability = new SpecialAbility();
        //ability.Initialize(source, abilityData);

        AddAbility(abilityData);

        cardName = abilityData.abilityName;

        AbilityDeck.ALL_CARDS.activeCards.Add(this);
    }

    public void ManagedUpdate() {
        //ability.ManagedUpdate();

        int count = abiliites.Count;

        for (int i = 0; i < count; i++) {
            abiliites[i].ManagedUpdate();

        }
    }

    public void Activate(Controller2D.CollisionInfo colInfo) {

        List<SpecialAbility> abilitiesToUse = DetermineAbilityToActivate(colInfo.below);

        int count = abilitiesToUse.Count;

        for(int i = 0; i < count; i++) {
            abilitiesToUse[i].Activate();
        }


        //ability.Activate();
    }

    public void AddAbility(SpecialAbilityData abilityData) {
        SpecialAbility newAbility = new SpecialAbility();
        newAbility.Initialize(source, abilityData);


        abiliites.AddUnique(newAbility);
    }

    private bool IsAbilityInUse() {
        int count = abiliites.Count;

        for (int i = 0; i < count; i++) {
            if (abiliites[i].InUse)
                return true;
        }

        return false;
    }

    private List<SpecialAbility> DetermineAbilityToActivate(bool grounded) {
        List<SpecialAbility> results = new List<SpecialAbility>();

        int count = abiliites.Count;

        for(int i = 0; i < count; i++) {
            if (AbilityDeckManager.IsCardInUse() && abiliites[i].overrideOtherAbilities == false) {
                Debug.Log("Another ability is in use");
                continue;
            }


            if (abiliites[i].abilityLimitations == Constants.SpecialAbilityLimitations.None) {
                results.Add(abiliites[i]);
                continue;
            }

            if (grounded && abiliites[i].abilityLimitations == Constants.SpecialAbilityLimitations.Grounded) {
                results.Add(abiliites[i]);
                continue;
            }

            if (grounded == false && abiliites[i].abilityLimitations == Constants.SpecialAbilityLimitations.Arial) {
                results.Add(abiliites[i]);
                continue;
            }
        }

        return results;
    }

}
