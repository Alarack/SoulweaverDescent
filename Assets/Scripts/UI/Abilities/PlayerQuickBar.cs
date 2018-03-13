using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SlotType = PlayerAbilitySlot.SlotType;

public class PlayerQuickBar : MonoBehaviour {

    public Sprite emptySlotImage;

    //[Header("Primary slots")]
    //public List<PlayerAbilitySlot> primarySlots = new List<PlayerAbilitySlot>();

    //[Header("Cycling slots")]
    //public List<PlayerAbilitySlot> cyclingSlots = new List<PlayerAbilitySlot>();

    [Header("Slots")]
    public List<PlayerAbilitySlot> slots = new List<PlayerAbilitySlot>();

    private void Awake() {
        Initialize();
    }

    public void Initialize() {
        InitializeSlots();
    }

    private void InitializeSlots() {
        int count = slots.Count;

        for (int i = 0; i < count; i++) {
            slots[i].Initialize(this);
        }
    }



    private PlayerAbilitySlot GetFirstEmptySlot(SlotType slotType = SlotType.Cycling) {
        int count = slots.Count;

        for (int i = 0; i < count; i++) {
            if (slots[i].slotType != slotType)
                continue;


            if (slots[i].IsEmpty) {
                slots[i].currentIndex = i;
                return slots[i];
            }
        }

        return null;
    }

    private PlayerAbilitySlot GetSlotByCard(AbilityCard abilityCard, SlotType slotType = SlotType.Cycling) {
        int count = slots.Count;

        for (int i = 0; i < count; i++) {
            if (slots[i].slotType != slotType)
                continue;


            if (slots[i].CurrentCard == abilityCard) {
                return slots[i];
            }
        }

        return null;
    }

    public void SetQuickBarSlot(AbilityCard abilityCard, SlotType slotType = SlotType.Cycling) {
        //if(slotIndex > quickbarSlots.Count) {
        //    Debug.LogError("[PlayerQuickBar] index out of range. " + slotIndex + " is more than 4");
        //}

        PlayerAbilitySlot targetSlot = GetFirstEmptySlot(slotType);

        if (targetSlot == null) {
            Debug.Log("Couldn't get slot");
        }

        targetSlot.SetSlotAbility(abilityCard);
    }

    public void ClearQuickBarSlot(AbilityCard abilityCard, SlotType slotType = SlotType.Cycling) {
        //if (slotIndex > quickbarSlots.Count) {
        //    Debug.LogError("[PlayerQuickBar] index out of range. " + slotIndex + " is more than 4");
        //}
        GetSlotByCard(abilityCard, slotType).RemoveSlotAbility();
    }





}
