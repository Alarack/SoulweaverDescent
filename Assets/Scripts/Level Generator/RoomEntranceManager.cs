using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEntranceManager : MonoBehaviour {

    public RoomEntrance[] entrances;


    private void Awake() {
        GatherEntrances();
    }


    private void GatherEntrances() {
        entrances = GetComponentsInChildren<RoomEntrance>();
    }


    public void CloseDoors() {
        int count = entrances.Length;

        for (int i = 0; i < count; i++) {
            entrances[i].PlayCloseTween();
        }
    }

    public void OpenDoors() {
        int count = entrances.Length;

        for (int i = 0; i < count; i++) {
            entrances[i].PlayOpenTween();
        }
    }

    public void ToggleDoors() {
        int count = entrances.Length;

        for (int i = 0; i < count; i++) {
            entrances[i].Toggle();
        }
    }




}
