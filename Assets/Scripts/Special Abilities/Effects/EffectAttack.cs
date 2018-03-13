using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectAttack : Effect {

    public int effectDamage;

    public bool scaleFromBaseDamage;
    public float percentOfBaseDamage = 1f;

    public bool burstAttack;
    public int burstNumber;
    public float burstInterval;

    public string impactEffectName;
    public string fireEffectName;
    //public float range;

    public bool penetrate;
    public int numPenetrations;

    //protected Vector2 shotOrigin;


    public EffectAttack() {

    }

    public EffectAttack(EffectAttack attackEffect) {
        effectName = attackEffect.effectName;
        riderTarget = attackEffect.riderTarget;
        effectType = attackEffect.effectType;
        deliveryMethod = attackEffect.deliveryMethod;
        animationTrigger = attackEffect.animationTrigger;

        //applyToSpecificTarget = attackEffect.applyToSpecificTarget;
        //targetIndex = attackEffect.targetIndex;

        //parentAbility = attackEffect.parentAbility;
        riders = attackEffect.CloneRiders();

        effectDamage = attackEffect.effectDamage;
        scaleFromBaseDamage = attackEffect.scaleFromBaseDamage;
        percentOfBaseDamage = attackEffect.percentOfBaseDamage;
        burstAttack = attackEffect.burstAttack;
        burstNumber = attackEffect.burstNumber;
        burstInterval = attackEffect.burstInterval;
        impactEffectName = attackEffect.impactEffectName;
        fireEffectName = attackEffect.fireEffectName;
        penetrate = attackEffect.penetrate;
        numPenetrations = attackEffect.numPenetrations;

        switch (deliveryMethod) {
            case Constants.EffectDeliveryMethod.Melee:
                meleeDelivery.prefabName = attackEffect.meleeDelivery.prefabName;
                meleeDelivery.layerMask = attackEffect.meleeDelivery.layerMask;
                break;

            case Constants.EffectDeliveryMethod.Projectile:
                projectileDelivery.prefabName = attackEffect.projectileDelivery.prefabName;
                projectileDelivery.layerMask = attackEffect.projectileDelivery.layerMask;
                break;

            case Constants.EffectDeliveryMethod.Raycast:
                rayCastDelivery.layerMask = attackEffect.rayCastDelivery.layerMask;
                break;
        }

    }

    public override void Initialize(SpecialAbility parentAbility) {
        base.Initialize(parentAbility);

        switch (deliveryMethod) {
            case Constants.EffectDeliveryMethod.Raycast:
                rayCastDelivery.Initialize(parentAbility, this);
                break;

            case Constants.EffectDeliveryMethod.Projectile:
                projectileDelivery.Initialize(parentAbility, this);
                break;

            case Constants.EffectDeliveryMethod.Melee:
                meleeDelivery.Initialize(parentAbility, this);
                break;
        }
    }

    public override void Activate() {
        base.Activate();

        //Debug.Log("Activating an attack");

        if (burstAttack) {
            parentAbility.source.StartCoroutine(BurstFire(burstInterval, burstNumber));
        }
        else {
            Fire();
        }
    }

    protected virtual IEnumerator BurstFire(float delay, int number) {

        for(int i = 0; i < number; i++) {
            BeginDelivery();
            yield return new WaitForSeconds(delay);
        }
    }

    protected virtual void Fire() {
        BeginDelivery();
    }

    public override void Apply(GameObject target) {

        float damage;
        if (scaleFromBaseDamage)
            damage = effectDamage + (parentAbility.source.stats.GetStatModifiedValue(StatCollection.BaseStat.BaseStatType.BaseDamage) * percentOfBaseDamage);
        else
            damage = effectDamage;

        //Debug.Log(damage + " is being dealt to " + target.gameObject.name);

        Entity targetEntity = target.GetComponent<Entity>();

        if(targetEntity != null) {
            StatAdjustmentManager.ApplyUntrackedStatMod(Source, targetEntity, StatCollection.BaseStat.BaseStatType.Health, damage);
        }

        base.Apply(target);
    }

}