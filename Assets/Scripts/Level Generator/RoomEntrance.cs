using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RoomEntrance : MonoBehaviour {

    public Room.RoomConnection connection;

    [Header("Door")]
    public GameObject door;
    public Collider2D doorCollider;

    [Header("Seal")]
    public GameObject seal;

    [Header("Tweens")]
    public DOTweenAnimation doorClosingTween;

    public bool Closed { get; protected set; }
    public bool Sealed { get; protected set; }



    private void Awake() {
        doorCollider.enabled = false;
    }

    public void PlayOpenTween() {
        doorClosingTween.DOPlayBackwardsById("Closing");
        OpenDoor();
    }

    public void PlayCloseTween() {
        doorClosingTween.DOPlayForwardById("Closing");
        CloseDoor();
    }

    public void CloseDoor() {
        Closed = true;
        doorCollider.enabled = true;
    }

    public void OpenDoor() {
        Closed = false;
        doorCollider.enabled = false;
    }

    public void Toggle() {
        if (Closed)
            PlayOpenTween();
        else
            PlayCloseTween();
    }


    public void Seal() {
        door.SetActive(false);
        seal.SetActive(true);
        Sealed = true;
    }
}
