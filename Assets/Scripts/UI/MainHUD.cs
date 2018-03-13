using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHUD : MonoBehaviour {

    public static MainHUD mainHud;
    public PlayerQuickBar quickBar;
    //public GameResourceDisplay resourceDisplay;


    private void Awake() {
        if (mainHud == null)
            mainHud = this;
        else
            Destroy(this);
    }

    private void Start() {
        //resourceDisplay.Initialize();
    }

    public static void SetPlayerSlot(AbilityCard abilityCard, PlayerAbilitySlot.SlotType slotType = PlayerAbilitySlot.SlotType.Cycling) {
        if (mainHud != null) {
            mainHud.quickBar.SetQuickBarSlot(abilityCard, slotType);
        }
    }

    public static void ClearPlayerSlot(AbilityCard abilityCard, PlayerAbilitySlot.SlotType slotType = PlayerAbilitySlot.SlotType.Cycling) {
        mainHud.quickBar.ClearQuickBarSlot(abilityCard, slotType);
    }
    
    public static PlayerAbilitySlot GetAbilitySlotByIndex(int index) {
        return mainHud.quickBar.slots[index];
    }
}
