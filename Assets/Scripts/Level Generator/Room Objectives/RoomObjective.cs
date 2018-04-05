using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomObjective {



    public bool IsComplete { get; protected set; }

    protected RoomObjectiveManager manager;
    protected Room parentRoom;



    public RoomObjective(RoomObjectiveManager manager, Room parentRoom) {
        this.manager = manager;
        this.parentRoom = parentRoom;
    }


    public virtual void Initialize() {

    }

    public abstract void Begin();

    public virtual void Complete() {
        IsComplete = true;
    }

}
