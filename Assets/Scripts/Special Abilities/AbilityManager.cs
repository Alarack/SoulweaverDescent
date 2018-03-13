using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityManager : MonoBehaviour {

    public List<SpecialAbilityData> abilityData = new List<SpecialAbilityData>();


    protected Entity source;
    protected List<SpecialAbility> abilities = new List<SpecialAbility>();

    public virtual void Initialize(Entity source) {
        this.source = source;

        PopulateSpecialAblities(abilityData);
    }


    public virtual void PopulateSpecialAblities(List<SpecialAbilityData> abilityData) {
        for (int i = 0; i < abilityData.Count; i++) {
            SpecialAbility newAbility = new SpecialAbility();

            newAbility.Initialize(source, abilityData[i], abilityData[i].sequencedAbilities);
            abilities.Add(newAbility);

            //Debug.Log("Adding " + newAbility.abilityName);
        }
    }


    protected virtual void Update() {
        for(int i = 0; i < abilities.Count; i++) {
            abilities[i].ManagedUpdate();
        }

    }

    protected bool IsAbilityInUse() {
        for(int i = 0; i < abilities.Count; i++) {
            if (abilities[i].InUse) {
                return true;
            }
                
        }

        return false;
    }


}
