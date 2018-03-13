using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAbilityManager : AbilityManager {

    private List<PlayerAbilityContainer> playerAbilities = new List<PlayerAbilityContainer>();


    public override void Initialize(Entity source) {
        base.Initialize(source);

        for (int i = 0; i < abilities.Count; i++) {
            PlayerAbilityContainer newAbility = new PlayerAbilityContainer(abilities[i], (i + 1));
            playerAbilities.Add(newAbility);

            //MainHUD.SetPlayerSlot(abilities[i]);

            //Debug.Log("Adding " + abilities[i].abilityName);
        }
    }

    protected override void Update() {
        base.Update();

        if (Input.GetButtonDown("Dodge")) {
            //Debug.Log("Dodging key pressed");

            if(((MovingEntity)source).Controller.collisionInfo.below) {
                ActivatePlayerAbility(1);
            }


        }


        //for (int i = 0; i < playerAbilities.Count; i++) {
        //    if (Input.GetButtonDown("Fire" + (i + 1))) {
        //        ActivatePlayerAbility(i + 1);
        //    }
        //}
    }

    public void ActivatePlayerAbility(int abilityKey) {
        for (int i = 0; i < playerAbilities.Count; i++) {
            if(playerAbilities[i].abilityKey == abilityKey) {

                if (IsAbilityInUse() && !playerAbilities[i].ability.overrideOtherAbilities) {
                    Debug.LogError("another ability is current in use");
                    return;
                }

                //Debug.Log("Player is activating " + playerAbilities[i].ability.abilityName);
                playerAbilities[i].ability.Activate();
            }
        }
    }


    [System.Serializable]
    public class PlayerAbilityContainer {
        public SpecialAbility ability;
        public int abilityKey;

        public PlayerAbilityContainer(SpecialAbility ability, int abilityKey) {
            this.ability = ability;
            this.abilityKey = abilityKey;
        }

    }


}