using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkenMissile : GuidedMissile {

    [Header("Drunken Behaviour")]
    public float drunkenInterval;


    private Timer drunktimer;


    public override void Initialize(Effect parentEffect, LayerMask mask, float life = 0, float damage = 0) {
        base.Initialize(parentEffect, mask, life, damage);

        //drunktimer = new Timer(drunkenInterval, true, lookScript.UpdateError);
    }

    protected override void Update() {
        base.Update();

        //drunktimer.UpdateClock();
    }





}
