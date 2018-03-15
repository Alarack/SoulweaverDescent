using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {

    public string itemName;
    public float itemValue;

    public Sprite itemIcon;

    public int ItemID { get; private set; }


    public void Initialize() {
        ItemID = IDFactory.GenerateItemID();

        ItemFactory.RegisterItem(this);
    }


}
