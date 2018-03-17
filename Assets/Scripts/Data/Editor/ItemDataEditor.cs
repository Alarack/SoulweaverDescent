using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemData))]
public class ItemDataEditor : Editor {

    private ItemData _itemData;


    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
        _itemData = (ItemData)target;

        _itemData.validSlot = EditorHelper.EnumPopup("Valid Slot", _itemData.validSlot);
        _itemData.itemName = EditorGUILayout.TextField("Item Name", _itemData.itemName);
        _itemData.itemValue = EditorGUILayout.FloatField("Item value", _itemData.itemValue);
        _itemData.itemIcon = EditorHelper.ObjectField<Sprite>("Item Icon", _itemData.itemIcon);

        EditorGUILayout.Separator();

        _itemData.itemAbilities = EditorHelper.DrawExtendedList("Item Abilities", _itemData.itemAbilities, "Ability", DrawItemAbilities);


        if (GUI.changed)
            EditorUtility.SetDirty(target);

    }



    private Item.ItemAbility DrawItemAbilities(Item.ItemAbility entry) {
        entry.cardSlotType = EditorHelper.EnumPopup("Ability Type", entry.cardSlotType);
        entry.ability = EditorHelper.ObjectField<SpecialAbilityData>("Ability Data", entry.ability);


        return entry;
    }


}
