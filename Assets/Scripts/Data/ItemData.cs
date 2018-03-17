using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Data")]
[System.Serializable]
public class ItemData : ScriptableObject {

    public PaperdollSlot.PaperdollSlotType validSlot;

    public string itemName;
    public float itemValue;

    public Sprite itemIcon;

    public List<Item.ItemAbility> itemAbilities = new List<Item.ItemAbility>();

}
