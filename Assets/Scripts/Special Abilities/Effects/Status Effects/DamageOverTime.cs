using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : Status {


    protected float effectDamage;
    protected float baseDamage;
    

    public void InitializeDamageOverTime(float damge, Entity source) {
        effectDamage = damge;
        baseDamage = damge;
        this.source = source;
    }


    protected override void Tick() {
        base.Tick();

        if (targetEntity != null) {
            StatAdjustmentManager.ApplyUntrackedStatMod(source, target.GetComponent<Entity>(), StatCollection.BaseStat.BaseStatType.Health, effectDamage);
            //Debug.Log(effectDamage);
        }
        else {
            //Debug.Log("Target null");
        }

    }

    public override void Stack() {
        base.Stack();
        effectDamage += baseDamage;
        RefreshDuration();
    }

}
