using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MovingEntity {


    [Header("Decks")]
    public AbilityDeckManager deckManager;



    [Header("Sprites")]
    public SpriteRenderer spriteRenderer;
    //public Animator MyAnimator {get; private set;}

    //[Header("Jumping")]
    //public float maxJumpheight = 4f;
    //public float minJumpHeight = 1f;
    //public float timeToJumpApex = 0.4f;

    [Header("Moving")]
    //public float moveSpeed = 6f;
    public float wallSlideSpeedMax = 3f;

    [Header("Wall Jumping")]
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;
    public float wallStickTime = 0.25f;

    //[Header("Acceleration")]
    //public float accelerationTimeAirborn = 0.2f;
    //public float accelerationTimeGrounded = 0.1f;


    //public AbilityManager2 AbilityManager2 { get; private set; }

    //private float maxJumpVelocity;
    //private float minJumpVelocity;
    //private float gravity;
    private bool wallSliding;
    private bool stickToWall;
    private int wallDirX;

    //private Vector3 velocity;
    //private float velocityXSmoothing;

    //private Controller2D controller;
    private Timer wallStickTimer;

    private Vector2 directionalInput;

    protected AbilityManager inateAbiliites;
    protected Inventory inventory;


    private int jumpHash = Animator.StringToHash("Jumping");
    private int fallHash = Animator.StringToHash("Falling");
    private int arialHash = Animator.StringToHash("Light Air Attack");


    protected override void Start() {
        //GameManager.RegisterEntity(this);
        stats = new StatCollection(statTemplate);

        Controller = GetComponent<Controller2D>();
        MyAnimator = GetComponentInChildren<Animator>();

        deckManager = GetComponentInChildren<AbilityDeckManager>();
        deckManager.Initialize(this);

        inateAbiliites = GetComponentInChildren<AbilityManager>();
        inateAbiliites.Initialize(this);

        inventory = GetComponentInChildren<Inventory>();
        inventory.Initialize();

        //AbilityManager2 = GetComponent<AbilityManager2>();
        //AbilityManager2.Initialize(MyAnimator, controller);


        wallStickTimer = new Timer(wallStickTime, true, UnStickFromWall);

        gravity = -(2 * maxJumpheight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs( gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    protected override void Update() {
        CalculateVelocity();
        HandleWallSliding();

        //if (floating) {
        //    velocity.y = 0f;
        //}

        SmoothDownKnockBack();

        Controller.Move(velocity * Time.deltaTime,  directionalInput, currentKnockbackVelocity * Time.deltaTime);

        ResetYVelocity(ref velocity.y);

        SetJumpingFallingAnim(velocity.y);
    }

    public void SetDirectionalInput(Vector2 input) {
        directionalInput = input;

        bool running = directionalInput != Vector2.zero;
        MyAnimator.SetBool("Running", running);
        

        SetFacing(directionalInput.x);
    }

    private void SetFacing(float input) {
        if(wallSliding == false) {
            if (input < 0) {
                spriteRenderer.flipX = true;
                Facing = Constants.EntityFacing.Left;
            }
            if (input > 0) {
                spriteRenderer.flipX = false;
                Facing = Constants.EntityFacing.Right;
            }

        }
        else {
            if (input < 0) {
                spriteRenderer.flipX = false;
                Facing = Constants.EntityFacing.Right;
            }
            if (input > 0) {
                spriteRenderer.flipX = true;
                Facing = Constants.EntityFacing.Left;
            }
        }
    }

    public void OnAbilitySlotKeyPressed(int i) {
        PlayerAbilitySlot targetSlot = MainHUD.GetAbilitySlotByIndex(i - 1);

        if(targetSlot.CurrentCard != null) {
            deckManager.Hand.PlayCard(targetSlot.CurrentCard, Controller.collisionInfo, targetSlot.slotType);
        }


        //deckManager.Hand.PlayCard()
    }

    public void SetJumpingFallingAnim(float yVelocity) {

        bool falling = yVelocity < 0 && Controller.collisionInfo.below == false /*&& MyAnimator.GetBool(arialHash) == false*/;
        bool jumping = yVelocity > 0 && Controller.collisionInfo.below == false /*&& MyAnimator.GetBool(arialHash) == false*/;


        if(MyAnimator.GetBool(fallHash) != falling)
            MyAnimator.SetBool(fallHash, falling);

        if (MyAnimator.GetBool(jumpHash) != jumping)
            MyAnimator.SetBool(jumpHash, jumping);
    }

    public void OnJumpInputDown() {
        if (wallSliding) {
            if (wallDirX == Mathf.Sign(directionalInput.x)) {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0) {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else if (wallDirX > directionalInput.x || wallDirX < directionalInput.x) {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }
        }//End of Wall Sliding

        if (Controller.collisionInfo.below) {
            if (Controller.collisionInfo.slidingDownMaxSlope) {
                velocity.y = maxJumpVelocity * Controller.collisionInfo.slopeNormal.y;
                velocity.x = maxJumpVelocity * Controller.collisionInfo.slopeNormal.x;

                //if (directionalInput.x != -Mathf.Sign(controller.collisionInfo.slopeNormal.x)) { //Not Jumping against Max Slope
                //    velocity.y = maxJumpVelocity * controller.collisionInfo.slopeNormal.y;
                //    velocity.x = maxJumpVelocity * controller.collisionInfo.slopeNormal.x;
                //}
            }
            else {
                velocity.y = maxJumpVelocity + currentKnockbackVelocity.y;
            }
        }
    }

    public void OnJumpInputUp() {
        velocity.y = Mathf.Min(velocity.y, minJumpVelocity);
    }

    private void UnStickFromWall() {
        stickToWall = false;
    }

    private bool CheckWallSliding() {
        return (Controller.collisionInfo.left || Controller.collisionInfo.right) && Controller.collisionInfo.below == false && velocity.y < 0f;
    }

    private void HandleWallSliding() {
        wallDirX = (Controller.collisionInfo.left) ? -1 : 1;
        wallSliding = CheckWallSliding();

        MyAnimator.SetBool("WallSliding", wallSliding);

        if (wallSliding) {
            velocity.y = Mathf.Max(-wallSlideSpeedMax, velocity.y);

            if (stickToWall) {
                velocityXSmoothing = 0f;
                velocity.x = 0f;

                if (directionalInput.x != wallDirX && directionalInput.x != 0f) {
                    wallStickTimer.UpdateClock();
                }
                else {
                    wallStickTimer.ResetTimer();
                }
            }
            else {
                stickToWall = true;
            }
        }
    }


    protected override void CalculateVelocity() {

        //Debug.Log(currentKnockbackVelocity.x + " is my current knockback X in Calculate method");
        //Debug.Log(currentKnockbackVelocity.y + " is my current knockback Y in Calculate method");

        float targetVelocityX = (directionalInput.x * moveSpeed) /*+ currentKnockbackVelocity.x*/;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, GetVelocitySmoothingTime());
        velocity.y += (gravity /*+ currentKnockbackVelocity.y*/) * Time.deltaTime;

        //Debug.Log(velocity.y + " is my Y Velocity");
    }

}
