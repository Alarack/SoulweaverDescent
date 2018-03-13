using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : RaycastController {

    [Header("Info")]
    public CollisionInfo collisionInfo;

    [Header("Slopes")]
    public float maxSlopeAngle = 80f;

    public Vector2 PlayerInput { get; private set; }

    protected override void Start() {
        base.Start();
        collisionInfo.facingDirection = 1;
    }

    public void Move(Vector2 moveAmount, bool standingOnPlatform) {
        Move(moveAmount, Vector2.zero, Vector2.zero, standingOnPlatform);
    }


    public void Move(Vector2 moveAmount, Vector2 input, Vector2 knockbackVector, bool standingOnPlatform = false) {
        UpdateRaycastOrigins();
        collisionInfo.Reset();
        collisionInfo.velocityOld = moveAmount;
        PlayerInput = input;

        //Debug.Log(knockbackVector + " is the knockback vector");

        moveAmount += knockbackVector;

        if (moveAmount.y < 0f) {
            DescendSlope(ref moveAmount);
        }

        if (moveAmount.x != 0f) {
            collisionInfo.facingDirection = (int)Mathf.Sign(moveAmount.x);
        }

        CheckHorizontalCollisions(ref moveAmount);


        if (moveAmount.y != 0f)
            CheckVerticalCollisions(ref moveAmount);


        

        transform.Translate(moveAmount);

        if (standingOnPlatform == true)
            collisionInfo.below = true;
    }

    private void CheckVerticalCollisions(ref Vector2 moveAmount) {
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + SKIN_WIDTH;

        for (int i = 0; i < verticalRayCount; i++) {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if (hit.collider != null) {
                if(hit.collider.tag == "FallthroughSurface") {
                    if (directionY == 1 || hit.distance == 0)
                        continue;

                    if (collisionInfo.fallingThroughPlatform)
                        continue;

                    if (PlayerInput.y <= -0.5f) {
                        collisionInfo.fallingThroughPlatform = true;
                        Invoke("ResetFalingThrougPlatform", 0.5f);
                        continue;
                    }

                }

                moveAmount.y = (hit.distance - SKIN_WIDTH) * directionY;
                rayLength = hit.distance;

                if (collisionInfo.climbingSlope) {
                    moveAmount.x = moveAmount.y / Mathf.Tan(collisionInfo.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(moveAmount.x);
                }

                collisionInfo.below = directionY == -1;
                collisionInfo.above = directionY == 1;
            }
        }

        if (collisionInfo.climbingSlope) {
            float directionX = Mathf.Sign(moveAmount.x);
            rayLength = Mathf.Abs(moveAmount.x) + SKIN_WIDTH;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * moveAmount.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit.collider != null) {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != collisionInfo.slopeAngle) {
                    moveAmount.x = (hit.distance - SKIN_WIDTH) * directionX;
                    collisionInfo.slopeAngle = slopeAngle;
                    collisionInfo.slopeNormal = hit.normal;
                }
            }
        }
    }

    private void CheckHorizontalCollisions(ref Vector2 moveAmount) {
        float directionX = collisionInfo.facingDirection;
        float rayLength = Mathf.Abs(moveAmount.x) + SKIN_WIDTH;

        if(Mathf.Abs(moveAmount.x) < SKIN_WIDTH) {
            rayLength = SKIN_WIDTH * 2;
        }

        for (int i = 0; i < horizontalRayCount; i++) {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (hit.collider != null) {

                if (hit.distance == 0f)
                    continue;

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (i == 0 && slopeAngle <= maxSlopeAngle) {
                    if (collisionInfo.descendingSlope) {
                        collisionInfo.descendingSlope = false;
                        moveAmount = collisionInfo.velocityOld;
                    }

                    float distanceToSlopeStart = 0f;
                    if (slopeAngle != collisionInfo.slopeAngleOld) {
                        distanceToSlopeStart = hit.distance - SKIN_WIDTH;
                        moveAmount.x -= distanceToSlopeStart * directionX;
                    }

                    ClimbSlope(ref moveAmount, slopeAngle, hit.normal);
                    moveAmount.x += distanceToSlopeStart * directionX;
                }

                if (collisionInfo.climbingSlope == false || slopeAngle > maxSlopeAngle) {
                    moveAmount.x = (hit.distance - SKIN_WIDTH) * directionX;
                    rayLength = hit.distance;

                    if (collisionInfo.climbingSlope) {
                        moveAmount.y = Mathf.Tan(collisionInfo.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
                    }


                    collisionInfo.left = directionX == -1;
                    collisionInfo.right = directionX == 1;
                }
            }
        }
    }

    private void ClimbSlope(ref Vector2 moveAmount, float slopeAngle, Vector2 slopeNormal) {
        float moveDistance = Mathf.Abs(moveAmount.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (moveAmount.y <= climbVelocityY) {
            moveAmount.y = climbVelocityY;
            moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
            collisionInfo.below = true;
            collisionInfo.climbingSlope = true;
            collisionInfo.slopeNormal = slopeNormal;
        }
    }

    private void DescendSlope(ref Vector2 moveAmount) {

        RaycastHit2D maxSlopeHitLeft = Physics2D.Raycast(raycastOrigins.bottomLeft, Vector2.down, Mathf.Abs(moveAmount.y) + SKIN_WIDTH, collisionMask);
        RaycastHit2D maxSlopeHitRight = Physics2D.Raycast(raycastOrigins.bottomRight, Vector2.down, Mathf.Abs(moveAmount.y) + SKIN_WIDTH, collisionMask);

        if(maxSlopeHitLeft ^ maxSlopeHitRight) {
            SlideDownMaxSlope(maxSlopeHitLeft, ref moveAmount);
            SlideDownMaxSlope(maxSlopeHitRight, ref moveAmount);
        }

        if (collisionInfo.slidingDownMaxSlope)
            return;

        float directionX = Mathf.Sign(moveAmount.x);
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if (hit.collider != null) {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= maxSlopeAngle) {
                if (Mathf.Sign(hit.normal.x) == directionX) {
                    if (hit.distance - SKIN_WIDTH <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x)) {
                        float moveDistance = Mathf.Abs(moveAmount.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
                        moveAmount.y -= descendVelocityY;

                        collisionInfo.slopeAngle = slopeAngle;
                        collisionInfo.descendingSlope = true;
                        collisionInfo.below = true;
                        collisionInfo.slopeNormal = hit.normal;
                    }
                }
            }
        }
    }

    private void SlideDownMaxSlope(RaycastHit2D hit, ref Vector2 moveAmount) {
        if(hit.collider != null) {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if(slopeAngle > maxSlopeAngle) {
                moveAmount.x = Mathf.Sign(hit.normal.x) * (Mathf.Abs(moveAmount.y) - hit.distance) / Mathf.Tan(slopeAngle * Mathf.Deg2Rad);

                collisionInfo.slopeAngle = slopeAngle;
                collisionInfo.slidingDownMaxSlope = true;
                collisionInfo.slopeNormal = hit.normal;
            }
        }
    }


    private void ResetFalingThrougPlatform() {
        collisionInfo.fallingThroughPlatform = false;
    }


    //[System.Serializable]
    public struct CollisionInfo {
        public bool above;
        public bool below;
        public bool left;
        public bool right;

        public bool climbingSlope;
        public bool descendingSlope;
        public bool slidingDownMaxSlope;
        public float slopeAngle;
        public float slopeAngleOld;

        public Vector2 slopeNormal;
        public Vector2 velocityOld;

        public int facingDirection;
        public bool fallingThroughPlatform;

        public void Reset() {
            above = false;
            below = false;
            left = false;
            right = false;

            climbingSlope = false;
            descendingSlope = false;
            slidingDownMaxSlope = false;
            slopeNormal = Vector2.zero;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0f;
        }

    }

}
