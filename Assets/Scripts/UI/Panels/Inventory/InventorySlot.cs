using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

    public Image itemImage;
    public Item CurrentItem { get; private set; }
    public bool IsFull { get; private set; }

    private InventoryPanel inventoryPanel;

    private void Start() {
        Initialize();
    }

    public void Initialize() {
        itemImage.gameObject.SetActive(false);
    }


    public void AssignItem(Item item) {
        if(item == null) {
            Debug.LogError("[InventorySlot] was given a null item");
            return;
        }

        itemImage.gameObject.SetActive(true);
        CurrentItem = item;
        itemImage.sprite = item.itemIcon;
        IsFull = true;
    }

    public void RemoveItem(Item item) {
        if(CurrentItem == null) {
            Debug.LogError("[InventorySlot] was told to remove an item, but it had none");
            return;
        }

        CurrentItem = null;
        itemImage.sprite = null;
        IsFull = false;
        itemImage.gameObject.SetActive(false);
    }


}
