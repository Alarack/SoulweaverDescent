using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObjectiveKillAll : RoomObjective{

    private RoomEnemyManager enemyManager;



    public RoomObjectiveKillAll(RoomObjectiveManager manager, Room parentRoom, RoomEnemyManager enemyManager) : base(manager, parentRoom) {
        this.enemyManager = enemyManager;
    }

    //Set this to a coroutine to give a delay.
    public override void Begin() {
        enemyManager.SpawnAtEachPoint();
    }

    //Find a way to get when the room is cleared in a better way.
    public override void Complete() {
        base.Complete();

    }
}
