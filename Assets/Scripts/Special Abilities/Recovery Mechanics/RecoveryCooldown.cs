using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RecoveryCooldown : SpecialAbilityRecovery {

    public float cooldown;

    public float RatioOfRecovery { get { return timer.Ratio; } }
    private Timer timer;


    public override void Initialize(SpecialAbility parentAbility) {
        base.Initialize(parentAbility);
        timer = new Timer(cooldown, false, Refresh);

    }

    public override void Recover() {
        if(!Ready)
            timer.UpdateClock();
    }

    private void Refresh() {
        Ready = true;
    }

    public override void Trigger() {
        base.Trigger();
        timer.ResetTimer();
    }

}
