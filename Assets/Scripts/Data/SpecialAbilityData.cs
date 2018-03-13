using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SpecialAbility")]
[System.Serializable]
public class SpecialAbilityData : ScriptableObject {

    public string abilityName;
    public Sprite abilityIcon;
    public float useDuration;
    public bool overrideOtherAbilities;
    public float procChance = 1f;
    public List<Constants.SpecialAbilityEffectType> effectTypes = new List<Constants.SpecialAbilityEffectType>();
    public Constants.SpecialAbilityRecoveryType recoveryType;
    public Constants.SpecialAbilityActivationMethod activationMethod;
    public Constants.SpecialAbilityType abilityType;
    public Constants.SpecialAbilityLimitations abilityLimitations;

    public AbilityRecoveryManager recoveryManager = new AbilityRecoveryManager();

    public EffectHolder effectHolder = new EffectHolder();

    public List<SpecialAbilityData> sequencedAbilities = new List<SpecialAbilityData>();
    public float sequenceWindow = 0.5f;
    public string animationTrigger;



    public SpecialAbilityRecovery GetRecoveryMechanic() {
        SpecialAbilityRecovery result = ObjectCopier.Clone(recoveryManager.GetRecoveryMethodByType(recoveryType)) as SpecialAbilityRecovery;

        return result;
    }

    public List<Effect> GetAllEffects() {
        List<Effect> results = new List<Effect>();
        for(int i = 0; i < effectTypes.Count; i++) {

            EffectSet holder = effectHolder.GetEffectSet(effectTypes[i]);

            if(holder != null) {
                results.AddRange(holder.effects);
            }

        }
        return results;
    }


    [System.Serializable]
    public class EffectHolder {
        public List<EffectAttack> attacks = new List<EffectAttack>();
        public List<EffectStatus> statusEffects = new List<EffectStatus>();


        public EffectSet GetEffectSet(Constants.SpecialAbilityEffectType effectType) {
            switch (effectType) {
                case Constants.SpecialAbilityEffectType.AttackEffect:
                    List<Effect> attackBundle = new List<Effect>();

                    for(int i = 0; i < attacks.Count; i++) {
                        EffectAttack clonedAttack = new EffectAttack(attacks[i]);
                        attackBundle.Add(clonedAttack);
                    }

                    EffectSet attackEffect = new EffectSet(effectType, attackBundle);

                    return attackEffect;

                case Constants.SpecialAbilityEffectType.StatusEffect:
                    List<Effect> statusBundle = new List<Effect>();

                    for(int i = 0; i < statusEffects.Count; i++) {
                        EffectStatus clonedStatus = new EffectStatus(statusEffects[i]);
                        statusBundle.Add(clonedStatus);
                    }

                    EffectSet statusAttacks = new EffectSet(effectType, statusBundle);

                    return statusAttacks;

                default:
                    return null;
            }
        }
    }

    [System.Serializable]
    public class EffectSet {
        public Constants.SpecialAbilityEffectType effectType;
        public List<Effect> effects = new List<Effect>();


        public EffectSet(Constants.SpecialAbilityEffectType effectType, List<Effect> effects) {
            this.effects = effects;
            this.effectType = effectType;
        }

    }


}
