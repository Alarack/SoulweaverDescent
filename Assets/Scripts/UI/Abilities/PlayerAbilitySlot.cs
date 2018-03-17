using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilitySlot : MonoBehaviour {

    public enum SlotType {
        Primary,
        Cycling,
        Passive,
        None
    }

    public SlotType slotType;

    public Image dimmer;
    public Image icon;

    public bool IsEmpty { get; private set; }
    public int currentIndex;

    private PlayerQuickBar parent;
    //private SpecialAbility ability;
    public AbilityCard CurrentCard { get; private set; }
    private SpecialAbilityRecovery recovery;

    public void Initialize(PlayerQuickBar parent) {
        this.parent = parent;

        //SetSlotAbility(ability);
        //SetIcon();
        icon.sprite = parent.emptySlotImage;
        IsEmpty = true;
    }

    public void SetSlotAbility(AbilityCard abilityCard) {
        this.CurrentCard = abilityCard;
        recovery = abilityCard.abiliites[0].Recovery;
        SetIcon();

        //Debug.Log("Adding a card to a slot");

        if (abilityCard != null)
            IsEmpty = false;
    }

    public void RemoveSlotAbility() {
        CurrentCard = null;
        recovery = null;
        icon.sprite = parent.emptySlotImage;

        IsEmpty = true;
    }

    private void SetIcon() {
        icon.sprite = CurrentCard.abiliites[0].abilityIcon;
    }

    private void Update() {
        if (CurrentCard == null)
            return;

        if(recovery != null)
            UpdateCooldown();
        else if (dimmer.fillAmount != 0) {
            dimmer.fillAmount = 0;
        }

    }

    private void UpdateCooldown() {
        if (!recovery.Ready) {
            switch (recovery.recoveryType) {
                case Constants.SpecialAbilityRecoveryType.Timed:
                    dimmer.fillAmount = Mathf.Abs( ((RecoveryCooldown)recovery).RatioOfRecovery - 1);

                    break;
            }
        }

        if (recovery.Ready && dimmer.fillAmount != 0) {
            dimmer.fillAmount = 0;
        }

    }



}
