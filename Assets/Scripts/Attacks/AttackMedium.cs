using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackMedium : MonoBehaviour {


    public string impactEffect;

    public StatCollectionData statTemplate;
    public StatCollection stats;
    public LayerMask LayerMask { get; protected set; }
    public Constants.EntityFacing ParentFacing { get; protected set; }

    protected bool penetrating;
    protected int numPen;
    protected int curPen;

    protected float life;
    protected float damage;

    protected Effect parentEffect;




    public virtual void Initialize(Effect parentEffect, LayerMask mask, float life = 0f, float damage = 0f) {
        this.parentEffect = parentEffect;
        LayerMask = mask;
        stats = new StatCollection(statTemplate);
        //stats.Initialize(statTemplate);
        ParentFacing = parentEffect.Source.Facing;


        if (this.parentEffect is EffectAttack) {
            EffectAttack attackEffect = this.parentEffect as EffectAttack;
            penetrating = attackEffect.penetrate;
            numPen = attackEffect.numPenetrations;
            impactEffect = attackEffect.impactEffectName;
        }


        this.life = life + stats.GetStatModifiedValue(StatCollection.BaseStat.BaseStatType.Lifetime);
        this.damage = damage + stats.GetStatModifiedValue(StatCollection.BaseStat.BaseStatType.BaseDamage);

        if (this.life > 0f) {
            Invoke("CleanUp", this.life);
        }

    }


    public virtual void CleanUp() {


    }

}
