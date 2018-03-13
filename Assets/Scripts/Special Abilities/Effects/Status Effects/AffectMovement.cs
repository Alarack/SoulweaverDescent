using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffectMovement : Status {

    public enum AffectMovementType {
        None,
        AlterSpeed,
        Halt,
        Knockback,
        SuspendInAir
    }


    //protected BaseMovement targetMovement;
    protected AffectMovementType affectType;
    protected float amount;
    protected Vector2 knockbackVector;
    //protected List<StatCollection.StatModifer> mods = new List<StatCollection.StatModifer>();
    protected StatCollection.StatModifer mod;

    public override void Initialize(GameObject target, float duration, float interval, Constants.StatusEffectType statusType, SpecialAbility sourceAbility, int maxStack = 1, Effect onCompleteEffect = null) {
        base.Initialize(target, duration, interval, statusType, sourceAbility, maxStack, onCompleteEffect);
    }

    public void InitializeAffectMovement(AffectMovementType type, float value, Vector2 knockback) {
        affectType = type;
        amount = value;
        //targetMovement = target.GetComponent<BaseMovement>();

        switch (affectType) {
            case AffectMovementType.Halt:

                //if (targetMovement == null)
                //    return;

                //if (targetMovement is EntityMovement) {
                //    if (((EntityMovement)targetMovement).Grounded || ((EntityMovement)targetMovement).Platformed)
                //        target.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                //}

                //targetMovement.CanMove = false;
                //targetMovement.CanPivot = false;
                break;

            case AffectMovementType.Knockback:
                //target.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                //targetMovement.CanMove = false;

                //target.GetComponent<Rigidbody2D>().AddForce(knockback * value);

                ((MovingEntity)targetEntity).ModifyVelociy(knockback * value);

                break;

            case AffectMovementType.AlterSpeed:
                mod = new StatCollection.StatModifer(amount, StatCollection.StatModificationType.Multiplicative);

                //Debug.Log("Applyin a mod of " + mod.value);

                StatAdjustmentManager.ApplyTrackedStatMod(source, targetEntity, StatCollection.BaseStat.BaseStatType.MoveSpeed, mod);

                //CombatManager.ApplyUntrackedStatMod(source, target.GetComponent<Entity>(), Constants.BaseStatType.MoveSpeed, amount, StatCollection.StatModificationType.Multiplicative);
                break;

            case AffectMovementType.SuspendInAir:
                ((MovingEntity)targetEntity).ResetY();
                break;
        }


    }


    protected override void CleanUp() {
        //if (targetMovement == null) {
        //    //Debug.Log("Nove moves");
        //    StatusManager.RemoveStatus(targetEntity, this);
        //    //Destroy(this);
        //    return;
        //}

        switch (affectType) {
            case AffectMovementType.Halt:
                //targetMovement.CanMove = true;
                //targetMovement.CanPivot = true;
                break;

            case AffectMovementType.AlterSpeed:
                if(mod != null) {
                    StatAdjustmentManager.RemoveTrackedStatMod(targetEntity, StatCollection.BaseStat.BaseStatType.MoveSpeed, mod);
                }
                else {
                    Debug.Log("Mod NUll");
                }
                break;

            case AffectMovementType.Knockback:
            case AffectMovementType.SuspendInAir:
                //targetMovement.CanMove = true;
                ((MovingEntity)targetEntity).ClearKnockbackVelocity();
                break;

        }


        base.CleanUp();
    }

}
