using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EffectStatus : Effect {

    //Base Stats
    public Constants.StatusEffectType statusType;
    public Constants.StatusStackingMethod stackMethod;
    public int maxStack = 1;
    public float duration;
    public float interval;
    public string onCompleteEffectName;

    //Affect Movement
    public AffectMovement.AffectMovementType affectMoveType;
    public float affectMoveValue;
    public float knocbackAngle;
    protected Vector2 knockbackVector;

    //Damage Over Time
    public bool scaleFromBaseDamage;
    public float damagePerInterval;
    public float percentOfBaseDamage;

    //Static Stat Adjustment
    public StatCollection.BaseStat.BaseStatType statType;
    public float statAdjustmentValue;
    public StatCollection.StatModificationType modType;

    //Durational Stat Adjustment


    protected Effect onCompleteEffect;

    public EffectStatus() {

    }

    public EffectStatus(EffectStatus effectStatus) {
        effectName = effectStatus.effectName;
        riderTarget = effectStatus.riderTarget;
        effectType = effectStatus.effectType;
        deliveryMethod = effectStatus.deliveryMethod;
        animationTrigger = effectStatus.animationTrigger;

        //applyToSpecificTarget = effectStatus.applyToSpecificTarget;
        //targetIndex = effectStatus.targetIndex;
        riders = effectStatus.CloneRiders();

        scaleFromBaseDamage = effectStatus.scaleFromBaseDamage;
        percentOfBaseDamage = effectStatus.percentOfBaseDamage;
        damagePerInterval = effectStatus.damagePerInterval;

        statusType = effectStatus.statusType;
        stackMethod = effectStatus.stackMethod;
        maxStack = effectStatus.maxStack;
        duration = effectStatus.duration;
        interval = effectStatus.interval;

        onCompleteEffectName = effectStatus.onCompleteEffectName;

        affectMoveType = effectStatus.affectMoveType;
        affectMoveValue = effectStatus.affectMoveValue;
        knocbackAngle = effectStatus.knocbackAngle;

        statType = effectStatus.statType;
        statAdjustmentValue = effectStatus.statAdjustmentValue;
        modType = effectStatus.modType;



        switch (deliveryMethod) {
            case Constants.EffectDeliveryMethod.Melee:
                meleeDelivery.prefabName = effectStatus.meleeDelivery.prefabName;
                meleeDelivery.layerMask = effectStatus.meleeDelivery.layerMask;
                break;

            case Constants.EffectDeliveryMethod.Projectile:
                projectileDelivery.prefabName = effectStatus.projectileDelivery.prefabName;
                projectileDelivery.layerMask = effectStatus.projectileDelivery.layerMask;
                break;
        }

    }

    public override void Initialize(SpecialAbility parentAbility) {
        base.Initialize(parentAbility);

        //Debug.Log("Initializing " + effectName);

        if(string.IsNullOrEmpty(onCompleteEffectName) == false) {
            //Debug.Log("Looking for an on complete effect");
            onCompleteEffect = parentAbility.GetEffectByName(onCompleteEffectName);
        }
            

        //if (onCompleteEffect != null)
        //    Debug.Log(onCompleteEffect.effectName + " is an on complete effect");

    }


    public override void Activate() {
        base.Activate();
        BeginDelivery();
    }

    public override void Apply(GameObject target) {
        base.Apply(target);

        //if (!CheckForSpecificTarget(target))
        //    return;

        switch (statusType) {
            case Constants.StatusEffectType.None:
                //Status newStatus = target.AddComponent<Status>();
                Status newStatus = new Status();


                newStatus.Initialize(target, duration, interval, statusType, parentAbility);

                StatusManager.AddStatus(target.GetComponent<Entity>(), newStatus, this, parentAbility); 
                break;

            case Constants.StatusEffectType.AffectMovement:
                //AffectMovement newAffectMovement = target.AddComponent<AffectMovement>();
                AffectMovement newAffectMovement = new AffectMovement();

                //knockbackVector = TargetingUtilities.DegreeToVector2(knocbackAngle);
                knockbackVector = TargetingUtilities.DirectionFromAngle(knocbackAngle, false);

                if (Source.Facing == Constants.EntityFacing.Left) {
                    knockbackVector = new Vector2(-knockbackVector.x, knockbackVector.y);
                }

                //Debug.Log(knockbackVector + " is the knockback vector");

                newAffectMovement.Initialize(target, duration, interval, statusType, parentAbility, maxStack, onCompleteEffect);
                newAffectMovement.InitializeAffectMovement(affectMoveType, affectMoveValue, knockbackVector);

                StatusManager.AddStatus(target.GetComponent<Entity>(), newAffectMovement, this, parentAbility); 
                break;

            case Constants.StatusEffectType.Stun:
                Stun newStun = new Stun();

                newStun.Initialize(target, duration, interval, statusType, parentAbility);
                newStun.InitializeStun();

                StatusManager.AddStatus(target.GetComponent<Entity>(), newStun, this, parentAbility);
                break;

            case Constants.StatusEffectType.DamageOverTime:


                float damage;
                if (scaleFromBaseDamage)
                    damage = damagePerInterval + (parentAbility.source.stats.GetStatModifiedValue(StatCollection.BaseStat.BaseStatType.BaseDamage) * percentOfBaseDamage);
                else
                    damage = damagePerInterval;

                //DamageOverTime newDot = target.AddComponent<DamageOverTime>();
                DamageOverTime newDot = new DamageOverTime();
                newDot.Initialize(target, duration, interval, statusType, parentAbility, maxStack);
                newDot.InitializeDamageOverTime(damage, parentAbility.source);

                StatusManager.AddStatus(target.GetComponent<Entity>(), newDot, this, parentAbility); 

                Debug.Log("Applying " + damage + " over time");

                break;

            case Constants.StatusEffectType.StaticStatAdjustment:
                StatCollection.StatModifer modStatic = new StatCollection.StatModifer(statAdjustmentValue, modType);

                Debug.Log("Stat " + statType + " is being adjusted by " + statAdjustmentValue);

                StatAdjustmentManager.ApplyTrackedStatMod(Source, target.GetComponent<Entity>(), statType, modStatic);

                break;

            case Constants.StatusEffectType.DurationalStatAdjustment:
                //StatCollection.StatModifer modDur = new StatCollection.StatModifer(statAdjustmentValue, modType);

                Debug.Log("Setting durational stuff");

                DurationalStatChange newDurationalStatChange = new DurationalStatChange();
                newDurationalStatChange.Initialize(target, duration, interval, statusType, parentAbility, maxStack);
                newDurationalStatChange.InitializeDurationalStatChange(statType, modType, statAdjustmentValue);

                StatusManager.AddStatus(target.GetComponent<Entity>(), newDurationalStatChange, this, parentAbility);

                break;
        }



    }






}
