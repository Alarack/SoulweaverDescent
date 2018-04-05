using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObjectiveManager : MonoBehaviour {

    public enum RoomObjectiveType {
        None = 0,
        KillAll = 1,
        EnemyWaves = 2,
        DestroyFeature = 3,
        Rescue = 4,
    }

    [Header("Possible Objectives")]
    public List<RoomObjectiveType> possibleObjectives = new List<RoomObjectiveType>();

    [Space(15)]
    //[HideInInspector]
    public RoomObjectiveType activeObjective;


    public RoomObjective Objective { get; private set; }

    private Room parentRoom;
    private RoomEnemyManager roomEnemyManager;

    public void Initialize(Room parentRoom, RoomEnemyManager roomEnemyManager) {
        this.parentRoom = parentRoom;
        this.roomEnemyManager = roomEnemyManager;

        if(parentRoom.roomType != Room.RoomType.Start) {
            SelectObjective();
            SetUpObjective();
        }
    }


    private void SelectObjective() {
        int randomIndex = Random.Range(0, possibleObjectives.Count);
        activeObjective = possibleObjectives[randomIndex];
    }

    public void SetUpObjective() {
        switch (activeObjective) {
            case RoomObjectiveType.None:

                break;

            case RoomObjectiveType.KillAll:
                Objective = new RoomObjectiveKillAll(this, parentRoom, roomEnemyManager);
                break;

            case RoomObjectiveType.DestroyFeature:

                break;

            case RoomObjectiveType.EnemyWaves:

                break;

            case RoomObjectiveType.Rescue:

                break;
        }
    }

}
