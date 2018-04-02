using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour {

    public enum FloorDepthName {
        Entry = 0,
        Descent = 1,
        Depths = 2,
        Abyss = 3
    }


    public enum EntityFacing {
        Right,
        Left,
    }

    public enum SpecialAbilityEffectType {
        None,
        AttackEffect,
        StatusEffect
    }

    public enum SpecialAbilityRecoveryType {
        None = 1,
        Timed = 2,
        Kills = 3,
        DamageDealt = 4,
        DamageTaken = 5,
        CurrencyChanged = 6,
    }

    public enum SpecialAbilityActivationMethod {
        None = 0,
        Manual = 1,
        Timed = 2,
        DamageDealt = 3,
        Passive = 4,
        EntityKilled = 5,
        DamageTaken = 6,
        EffectApplied = 7,
    }

    public enum SpecialAbilityLimitations {
        None = 0,
        Grounded = 1,
        Arial = 2,
    }

    public enum SpecialAbilityType {
        None = 0,
        Attack = 1,
        Buff = 2,
    }

    public enum EffectDeliveryMethod {
        None = 0,
        Raycast = 1,
        Melee = 2,
        Projectile = 3,
        SelfTargeting = 4,
        Rider = 5,
    }

    public enum EffectEventOption {

    }

    public enum StatusEffectType {
        None = 0,
        //Burning = 1,
        Stun = 2,
        //KnockBack = 3,
        AffectMovement = 4,
        DamageOverTime = 5,
        StaticStatAdjustment = 6,
        DurationalStatAdjustment = 7,
    }

    public enum StatusStackingMethod {
        None = 0,
        LimitedStacks = 1,
        StacksWithOtherAbilities = 2,
    }

    public enum GameEvent {
        None = 0,
        AbilityActivated = 1,
        StatChanged = 2,
        AnimationEvent = 3,
        EntityDied = 4,
        EffectApplied = 5,
        DifficultyChange = 6,
        ItemAquired = 7,
        ItemRemoved = 8,
        ItemEquipped = 9,
        ItemUnequipped = 10,
    }


}
