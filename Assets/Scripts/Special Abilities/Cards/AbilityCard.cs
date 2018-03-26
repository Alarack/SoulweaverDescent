using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SlotType = PlayerAbilitySlot.SlotType;

[System.Serializable]
public class AbilityCard {

    public PlayerAbilitySlot.SlotType cardSlotType;

    public string cardName;
    public bool InUse { get { return IsAbilityInUse(); } }
    public int ParentItemID { get; protected set; }
    //public SpecialAbility ability;

    public List<SpecialAbility> abilities = new List<SpecialAbility>();

    public AbilityDeck currentDeck;
    public AbilityDeck previousDeck;

    private Entity source;

    public AbilityCard(Entity source, SlotType cardSlotType = SlotType.None, int itemID = -1) {
        SetupCard(source, cardSlotType, itemID);

        //this.source = source;
        //this.cardSlotType = cardSlotType;
        //this.ParentItemID = itemID;
        //ability = new SpecialAbility();
        //ability.Initialize(source, abilityData);

        //AddAbility(abilityData);

        //cardName = abilityData.abilityName;

        
    }

    public AbilityCard(List<SpecialAbilityData> abilities, Entity source, SlotType cardSlotType = SlotType.None, int itemID = -1) {
        SetupCard(source, cardSlotType, itemID);

        int count = abilities.Count;
        for(int i = 0; i < count; i++) {

            //Debug.Log("Adding an ability " + abilities[i].abilityName + " to card");
            AddAbility(abilities[i], abilities[i].sequencedAbilities);
        }
    }

    private void SetupCard(Entity source, SlotType cardSlotType = SlotType.None, int itemID = -1) {
        this.source = source;
        this.cardSlotType = cardSlotType;
        this.ParentItemID = itemID;

        AbilityDeck.ALL_CARDS.activeCards.Add(this);
    }

    public void ManagedUpdate() {
        //ability.ManagedUpdate();

        int count = abilities.Count;

        for (int i = 0; i < count; i++) {
            abilities[i].ManagedUpdate();

        }
    }

    public bool Activate(Controller2D.CollisionInfo colInfo) {

        //List<SpecialAbility> abilitiesToUse = DetermineAbilityToActivate(colInfo.below);

        //int count = abilitiesToUse.Count;

        //for(int i = 0; i < count; i++) {
        //    abilitiesToUse[i].Activate();
        //}

        return ActivateAllPossibleAbilities();

        //ability.Activate();
    }

    private bool ActivateAllPossibleAbilities() {
        bool finalResult = false;
        List<bool> activationResults = new List<bool>();

        int count = abilities.Count;

        for (int i = 0; i < count; i++) {
            if (AbilityDeckManager.IsCardInUse() && abilities[i].overrideOtherAbilities == false) {
                Debug.Log("Another ability is in use, so " + abilities[i].abilityName + " cannnot activate");
                continue;
            }

            Debug.Log(abilities[i].abilityName + " is being activated from an ability card");

            activationResults.Add(abilities[i].Activate());
        }

        int boolCount = activationResults.Count;

        for(int i = 0; i < count; i++) {
            if(activationResults[i] == true) {
                finalResult = true;
                break;
            }
        }

        return finalResult;
    }

    public void AddAbility(SpecialAbilityData abilityData, List<SpecialAbilityData> sequencedAbilities = null) {
        SpecialAbility newAbility = new SpecialAbility();
        newAbility.Initialize(source, abilityData, sequencedAbilities);


        abilities.AddUnique(newAbility);
    }

    private bool IsAbilityInUse() {
        int count = abilities.Count;

        for (int i = 0; i < count; i++) {
            if (abilities[i].InUse)
                return true;
        }

        return false;
    }

    //private List<SpecialAbility> DetermineAbilityToActivate(bool grounded) {
    //    List<SpecialAbility> results = new List<SpecialAbility>();

    //    int count = abiliites.Count;

    //    for(int i = 0; i < count; i++) {
    //        if (AbilityDeckManager.IsCardInUse() && abiliites[i].overrideOtherAbilities == false) {
    //            Debug.Log("Another ability is in use");
    //            continue;
    //        }


    //        if (abiliites[i].abilityLimitations == Constants.SpecialAbilityLimitations.None) {
    //            results.Add(abiliites[i]);
    //            continue;
    //        }

    //        if (grounded && abiliites[i].abilityLimitations == Constants.SpecialAbilityLimitations.Grounded) {
    //            results.Add(abiliites[i]);
    //            continue;
    //        }

    //        if (grounded == false && abiliites[i].abilityLimitations == Constants.SpecialAbilityLimitations.Arial) {
    //            results.Add(abiliites[i]);
    //            continue;
    //        }
    //    }

    //    return results;
    //}

}
