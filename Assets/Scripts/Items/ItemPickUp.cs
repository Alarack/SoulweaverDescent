using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour {

    public LayerMask mask;
    public Item myItem;


    private void Start() {
        if (myItem.ItemID < 1)
            myItem.Initialize();
    }



    private void OnCollisionEnter2D(Collision2D other) {
        if ((mask & 1 << other.gameObject.layer) == 1 << other.gameObject.layer) {

            Debug.Log(other.gameObject.name + " picked me up");

            Inventory inventory = other.gameObject.GetComponentInChildren<Inventory>();

            if(inventory != null) {
                inventory.AddItem(myItem);
            }

        }
    }


}
