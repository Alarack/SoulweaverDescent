using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class MovingEntity : Entity {

    [Header("Jumping")]
    public float maxJumpheight = 4f;
    public float minJumpHeight = 1f;
    public float timeToJumpApex = 0.4f;

    [Header("Moving")]
    public float moveSpeed = 6f;

    [Header("Acceleration")]
    public float accelerationTimeAirborn = 0.2f;
    public float accelerationTimeGrounded = 0.1f;

    protected Vector3 velocity;
    protected float gravity;
    protected float velocityXSmoothing;
    public Controller2D Controller { get; protected set; }

    protected float maxJumpVelocity;
    protected float minJumpVelocity;

    //Knockback
    protected bool knockbackActive;
    protected Vector2 currentKnockbackVelocity = Vector2.zero;
    protected float knockbackSmoothingX;
    protected float knockbackSmoothingY;

    protected bool floating;

    protected override void Start() {
        base.Start();
        Controller = GetComponent<Controller2D>();
        MyAnimator = GetComponentInChildren<Animator>();
        stats = new StatCollection(statTemplate);

        gravity = -(2 * maxJumpheight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    protected virtual void Update() {
        CalculateVelocity();
        //HandleWallSliding();

        SmoothDownKnockBack();

        Controller.Move(velocity * Time.deltaTime, Vector2.zero, currentKnockbackVelocity * Time.deltaTime);

        ResetYVelocity(ref velocity.y);

        //SetJumpingFallingAnim(velocity.y);
    }

    public void ModifyVelociy(Vector2 mod) {
        currentKnockbackVelocity += mod;
        knockbackActive = true;
    }

    public void ResetY() {
        Debug.Log("floating?");
        floating = true;
        ResetYVelocity(ref velocity.y);
    }

    protected void SmoothDownKnockBack() {
        if(knockbackActive == false) {
            currentKnockbackVelocity.x = Mathf.SmoothDamp(currentKnockbackVelocity.x, 0f, ref knockbackSmoothingX, 0.1f);
            currentKnockbackVelocity.y = Mathf.SmoothDamp(currentKnockbackVelocity.y, 0f, ref knockbackSmoothingY, 0.1f);

            //Debug.Log(currentKnockbackVelocity + " is the current in smooth down");

            //currentKnockbackVelocity.x = Mathf.Lerp(currentKnockbackVelocity.x, 0f, 0.5f);
            //currentKnockbackVelocity.x = Mathf.Lerp(currentKnockbackVelocity.y, 0f, 0.5f);
        }
    }

    public void ClearKnockbackVelocity() {
        //currentKnockbackVelocity = Vector2.zero;
        floating = false;
        knockbackActive = false;
    }

    protected float GetVelocitySmoothingTime() {
        return Controller.collisionInfo.below ? accelerationTimeGrounded : accelerationTimeAirborn;
    }

    protected virtual void CalculateVelocity() {
        float targetVelocityX = moveSpeed /*+ currentKnockbackVelocity.x*/;
        //Debug.Log(targetVelocityX + " :: " + currentKnockbackVelocity.x);

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, GetVelocitySmoothingTime());
        velocity.y += (gravity /*+ currentKnockbackVelocity.y*/) * Time.deltaTime;
    }

    protected void ResetYVelocity(ref float yVelocity) {
        if (Controller.collisionInfo.above || Controller.collisionInfo.below)
            if (Controller.collisionInfo.slidingDownMaxSlope == true) {
                yVelocity += Controller.collisionInfo.slopeNormal.y * -gravity * Time.deltaTime;
            }
            else {
                velocity.y = 0f;
            }

    }

}
