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

        _itemData.itemCards = EditorHelper.DrawExtendedList("Item Abilities", _itemData.itemCards, "Ability", DrawItemCardData);


        if (GUI.changed)
            EditorUtility.SetDirty(target);

    }


    private Item.ItemAbilityCardData DrawItemCardData(Item.ItemAbilityCardData entry) {
        entry.cardName = EditorGUILayout.TextField("Card Name", entry.cardName);
        entry.cardDescripiton = EditorGUILayout.TextField("Card Description", entry.cardDescripiton);
        entry.cardSlotType = EditorHelper.EnumPopup("Ability Type", entry.cardSlotType);
        entry.abiliites = EditorHelper.DrawList("Abilities", entry.abiliites, true, null, true, DrawSpecialAbilityData);

        return entry;
    }

    //private Item.ItemAbilitySet DrawItemAbilities(Item.ItemAbilitySet entry) {
    //    entry.cardSlotType = EditorHelper.EnumPopup("Ability Type", entry.cardSlotType);
    //    entry.abiliites = EditorHelper.DrawList("Abilities", entry.abiliites, true, null, true, DrawSpecialAbilityData);
    //    entry.hasAbilityCard = EditorGUILayout.Toggle("Has Card", entry.hasAbilityCard);

    //    return entry;
    //}

    private SpecialAbilityData DrawSpecialAbilityData(List<SpecialAbilityData> abilityData, int index) {
        SpecialAbilityData result = EditorHelper.ObjectField<SpecialAbilityData>("Ability", abilityData[index]);
        return result;
    }


}
