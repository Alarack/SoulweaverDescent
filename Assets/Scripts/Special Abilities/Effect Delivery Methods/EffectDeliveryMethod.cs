using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectDeliveryMethod {

    public enum TargetingMethod {
        None = 0,
        StraightLeftRight = 1,
        Up = 2,
    }


    //public Constants.EffectDeliveryMethod deliveryMethodType;
    public TargetingMethod targetingMethod = TargetingMethod.StraightLeftRight;
    public LayerMask layerMask;


    protected Transform effectTransform;

    public float range;

    protected Vector2 shootDirection;

    [System.NonSerialized]
    protected SpecialAbility parentAbility;

    [System.NonSerialized]
    protected EffectAttack parentEffect;

    protected Vector2 effectOrigin;

    public virtual void Initialize(SpecialAbility parentAbility, EffectAttack parentEffect) {
        this.parentAbility = parentAbility;
        this.parentEffect = parentEffect;

    }

    public virtual void Deliver() {


    }

    protected virtual void CreateHitEffects(Vector2 location) {

    }

}
