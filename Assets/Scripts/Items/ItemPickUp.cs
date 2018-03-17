using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour {

    public LayerMask mask;
    public Item MyItem { get; private set; }

    public ItemData defaultItem;

    private void Start() {
        if(defaultItem != null) {
            //Initialize(defaultItem);
        }
    }


    public void Initialize(ItemData item = null) {
        MyItem = ItemFactory.CreateItem(item);
    }



    private void OnCollisionEnter2D(Collision2D other) {
        if ((mask & 1 << other.gameObject.layer) == 1 << other.gameObject.layer) {

            Debug.Log(other.gameObject.name + " picked me up");

            Inventory inventory = other.gameObject.GetComponentInChildren<Inventory>();

            if(inventory != null) {
                if(MyItem == null) {
                    Initialize(defaultItem);
                    inventory.AddItem(MyItem);
                }
                else {
                    inventory.AddItem(MyItem);
                }


            }

        }
    }


}
