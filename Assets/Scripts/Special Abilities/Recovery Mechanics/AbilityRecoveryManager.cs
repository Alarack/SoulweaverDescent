using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityRecoveryManager  {

    public List<Constants.SpecialAbilityRecoveryType> recoveryTypes = new List<Constants.SpecialAbilityRecoveryType>();
    public RecoveryCooldown recoveryCooldown = new RecoveryCooldown();


    public SpecialAbilityRecovery GetRecoveryMethodByType(Constants.SpecialAbilityRecoveryType recoveryType) {
        switch (recoveryType) {
            case Constants.SpecialAbilityRecoveryType.Timed:
                return recoveryCooldown;

            default:

                return null;
        }

    }

    [System.Serializable]
    public class RecoverySet {
        public Constants.SpecialAbilityRecoveryType recoveryType;
        public SpecialAbilityRecovery recoveryMechanic;

        public RecoverySet(Constants.SpecialAbilityRecoveryType recoveryType, SpecialAbilityRecovery recoveryMechanic) {
            this.recoveryType = recoveryType;
            this.recoveryMechanic = recoveryMechanic;

        }

    }

}
