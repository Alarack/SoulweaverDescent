using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour {

    public bool targetMouse;
    public bool transformRotation = true;

    public Transform currentTarget;

    public float rotateSpeed;
    public float error;

    public bool LookAway;


    protected Vector2 targetPos;


    protected void Update() {
        if (transformRotation)
            Aim();
    }



    public void SetTarget(Transform target) {
        currentTarget = target;
    }

    public void ClearTarget() {
        currentTarget = null;
        targetPos = Vector2.zero;
    }

    public void GetRandomTargetPos() {
        Vector2 random = Random.insideUnitCircle * 30f;

        targetPos = random;
    }

    protected virtual void AquireTarget() {
        //targetPos = Vector2.zero;

        if (targetMouse)
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        else if (currentTarget != null) {
            targetPos = currentTarget.position;
        }
    }

    //private void AquireTarget() {
    //    alltargets = TargetingUtilities.FindAllTargets(transform.position, detectionRadius, targetLayer);
    //    CurrentTarget = TargetingUtilities.FindNearestTarget(transform.position, alltargets);

    //    if (CurrentTarget != null)
    //        State = LookState.Aiming;
    //}

    protected virtual void Aim() {
        AquireTarget();

        if (targetPos != Vector2.zero) {
            transform.rotation = TargetingUtilities.SmoothRotation(targetPos, transform, rotateSpeed, error);
        }
    }
}
