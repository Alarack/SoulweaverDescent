using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RecoveryStatChanged : SpecialAbilityRecovery {

    public StatCollection.BaseStat.BaseStatType stat;
    public bool gained;
    public bool sourceCausedChange;
    public bool sourceWasChanged;

    public int targetNumber;

    private int currentNumber;

    protected override void RegisterListeners() {
        //EventGrid.EventManager.RegisterListener(Constants.GameEvent.StatChanged, OnStatChanged);

    }

    public override void Recover() {


    }

    private void CheckReady() {
        currentNumber++;

        if (currentNumber >= targetNumber) {
            currentNumber = 0;
            Ready = true;
        } 
    }


    //private void OnStatChanged(EventData data) {
    //    StatCollection.BaseStat.BaseStatType stat = (StatCollection.BaseStat.BaseStatType)data.GetInt("Stat");
    //    float value = data.GetFloat("Value");
    //    Entity target = data.GetMonoBehaviour("Target") as Entity;
    //    Entity cause = data.GetMonoBehaviour("Cause") as Entity;

    //    if (stat != this.stat)
    //        return;

    //    if (sourceWasChanged && target != parentAbility.source)
    //        return;

    //    if (sourceCausedChange && cause != parentAbility.source)
    //        return;

    //    if (gained && value < 0)
    //        return;

    //    if (!gained && value > 0)
    //        return;

    //    CheckReady();
    //}

}
