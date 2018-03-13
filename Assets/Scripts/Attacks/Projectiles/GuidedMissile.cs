using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LookAtTarget))]
public class GuidedMissile : Projectile {

    [Header("Basic Guide Stats")]
    public float trackTime;
    public float trackDelay;
    [Header("Speed Manipulation")]
    public bool increaseSpeedAfterTargetAquire;
    public float speedMultiplier;
    //[Header("Drunk Stuff")]
    //public bool drunken;
    //public float drunkTime;

    //private float drunkTimer;
    protected bool tracked = false;
    protected LookAtTarget lookScript;


    public override void Initialize(Effect parentEffect, LayerMask mask, float life = 0, float damage = 0) {
        base.Initialize(parentEffect, mask, life, damage);

        lookScript = GetComponent<LookAtTarget>();
        //lookScript.Initialize(this);

        //StartCoroutine(Tracking());
    }

    protected virtual void Update() {
        //lookScript.ManagedUpdate();
    }

    //protected IEnumerator Tracking() {
    //    yield return new WaitForSeconds(trackDelay);

    //    tracked = true;
    //    lookScript.ReaquireTarget();
    //    yield return new WaitForSeconds(trackTime);

    //    tracked = false;

    //    if (increaseSpeedAfterTargetAquire) {
    //        stats.ApplyUntrackedMod(Constants.BaseStatType.MoveSpeed, speedMultiplier, null, StatCollection.StatModificationType.Multiplicative);
    //        ProjectileMovement.UpdateBaseStats();
    //    }

    //}

}
