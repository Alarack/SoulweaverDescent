using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status {

    public Constants.StatusEffectType statusType;
    public int StackCount { get; protected set; } 
    public int maxStack;

    protected Timer durationTimer;
    protected Timer intervalTimer;
    protected GameObject target;
    protected Entity source;
    protected Entity targetEntity;
    protected SpecialAbility sourceAbility;
    protected Effect onCompleteEffect;


    public virtual void Initialize(GameObject target, float duration, float interval, Constants.StatusEffectType statusType, SpecialAbility sourceAbility, int maxStack = 1, Effect onCompleteEffect = null) {
        this.target = target;
        this.statusType = statusType;
        this.sourceAbility = sourceAbility;
        this.maxStack = maxStack;
        this.onCompleteEffect = onCompleteEffect;

        //Debug.Log(onCompleteEffect);

        targetEntity = target.GetComponent<Entity>();

        durationTimer = new Timer(duration, false, CleanUp);
        intervalTimer = new Timer(interval, true, Tick);
    }

    public virtual bool IsFromSameSource(SpecialAbility ability) {
        return sourceAbility == ability;
    }

    public virtual void Stack() {
        Debug.Log("Stacking");
        StackCount++;
    }

    public virtual void RefreshDuration() {
        durationTimer.ResetTimer();
    }

    public virtual void ModifyIntervalTime(float mod) {
        intervalTimer.ModifyDuration(mod);
    }

    public virtual void ModifyDuration(float mod) {
        durationTimer.ModifyDuration(mod);
    }

    public virtual void ManagedUpdate() {
        durationTimer.UpdateClock();
        intervalTimer.UpdateClock();
    }

    protected virtual void Tick() {
        //Debug.Log("Tickin");

    }

    protected virtual void CleanUp() {
        //Debug.Log("Cleaning " + sourceAbility.abilityName);
        //Destroy(this);
        StatusManager.RemoveStatus(targetEntity, this);

        if(onCompleteEffect != null) {
            //Debug.Log("Sending On Complete effect");
            onCompleteEffect.Activate();
        }

    }




}
