using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour {

    public LayerMask mask;
    public Item MyItem { get; private set; }

    public ItemData defaultItem;


    public void Initialize(ItemData item = null) {
        MyItem = ItemFactory.CreateItem(item);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if ((mask & 1 << other.gameObject.layer) == 1 << other.gameObject.layer) {
            Inventory inventory = other.gameObject.GetComponentInChildren<Inventory>();

            CheckAndAddItem(inventory);

            CleanUp();
        }
    }

    private void CheckAndAddItem(Inventory inventory) {
        if (inventory == null) {
            Debug.LogError("Inventory not found");
            return;
        }

        if (MyItem == null) {
            Initialize(defaultItem);
            inventory.AddItem(MyItem);
        }
        else {
            inventory.AddItem(MyItem);
        }

    }

    private void CleanUp() {
        Destroy(gameObject);
    }


}
