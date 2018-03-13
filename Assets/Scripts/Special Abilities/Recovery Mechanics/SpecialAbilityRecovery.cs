using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class SpecialAbilityRecovery {

    public Constants.SpecialAbilityRecoveryType recoveryType;
    public bool Ready { get; protected set; }

    [System.NonSerialized]
    protected SpecialAbility parentAbility;

    public virtual void Initialize(SpecialAbility parentAbility) {
       this.parentAbility = parentAbility;
        Ready = true;
        RegisterListeners();
    }

    protected virtual void RegisterListeners() {

    }

    public virtual void ManagedUpdate() {

        switch (recoveryType) {
            case Constants.SpecialAbilityRecoveryType.Timed:
                Recover();
                break;
        }
    }

    public abstract void Recover();

    public virtual void Trigger() {
        Ready = false;
    }


}
