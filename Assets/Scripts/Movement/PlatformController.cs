using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : RaycastController {

    [Header("Platform Mask")]
    public LayerMask passengerMask;

    [Header("Platform Movement")]
    public float speed;
    public bool cyclic;

    [Header("Platform Pausing")]
    public bool waitAtWaypoint;
    public float waitTime;

    [Header("Platform Easing")]
    [Range(0, 5)]
    public float easeAmount;

    [Header("Platform WayPoints")]
    public Vector3[] localWaypoints;


    private int fromWaypointIndex;
    private float percentBetweenWaypoints;
    private Timer waitTimer;
    private bool waiting;

    private Vector3[] globalWaypoints;
    private List<PassengerMovement> passengerMovement;
    private Dictionary<Transform, Controller2D> passengerDictionary = new Dictionary<Transform, Controller2D>();

    protected override void Start() {
        base.Start();

        if (waitAtWaypoint) {
            waitTimer = new Timer(waitTime, true, FinishWaiting);
        }

        globalWaypoints = new Vector3[localWaypoints.Length];

        int count = localWaypoints.Length;
        for (int i = 0; i < count; i++) {
            globalWaypoints[i] = localWaypoints[i] + transform.position;
        }
    }

    private void Update() {
        UpdateRaycastOrigins();


        Vector3 velocity = CalculatePlatformMovement();

        CalculatePassengerMovement(velocity);
        MovePassengers(true);
        transform.Translate(velocity);
        MovePassengers(false);
    }

    private void MovePassengers(bool beforMovePlatform) {
        int count = passengerMovement.Count;

        for(int i = 0; i < count; i++) {
            if(passengerDictionary.ContainsKey(passengerMovement[i].transform) == false) {
                passengerDictionary.Add(passengerMovement[i].transform, passengerMovement[i].transform.GetComponent<Controller2D>());
            }

            if(passengerMovement[i].moveBeforePlatform == beforMovePlatform) {
               passengerDictionary[passengerMovement[i].transform].Move(passengerMovement[i].velocity, passengerMovement[i].standingOnPlatform);
            }
        }
    }

    private void FinishWaiting() {
        waiting = false;
    }

    private float Ease(float x) {
        float a = easeAmount +1;

        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

    private Vector3 CalculatePlatformMovement() {

        if(waiting && waitAtWaypoint) {
            waitTimer.UpdateClock();
            return Vector3.zero;
        }

        fromWaypointIndex %= globalWaypoints.Length;
        int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
        float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
        percentBetweenWaypoints += Time.deltaTime * speed / distanceBetweenWaypoints;
        percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
        float easedPercent = Ease(percentBetweenWaypoints);

        Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], easedPercent);

        if(percentBetweenWaypoints >= 1f) {
            percentBetweenWaypoints = 0f;
            fromWaypointIndex++;
            if(cyclic == false) {
                if (fromWaypointIndex >= globalWaypoints.Length - 1) {
                    fromWaypointIndex = 0;
                    System.Array.Reverse(globalWaypoints);
                }
            }

            waiting = true;
           
        }


        return newPos - transform.position;
    }


    private void CalculatePassengerMovement(Vector3 velocity) {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();
        passengerMovement = new List<PassengerMovement>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        //Vertical Moveing Playform
        if (velocity.y != 0f) {
            float rayLength = Mathf.Abs(velocity.y) + SKIN_WIDTH;

           // Debug.Log(verticalRayCount + " is vertical ray count");

            for (int i = 0; i < verticalRayCount; i++) {
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

                if (hit.collider != null && hit.distance != 0) {
                    if (movedPassengers.Contains(hit.transform) == false) {
                        movedPassengers.Add(hit.transform);

                        float pushX = (directionY == 1) ? velocity.x : 0f;
                        float pushY = velocity.y - (hit.distance - SKIN_WIDTH) * directionY;

                        AddPassengerMovement(hit.transform, new Vector3(pushX, pushY, 0f), directionY == 1, true);

                    }
                }
            }
        }

        //Horizontal Moving Platform
        if (velocity.x != 0) {
            float rayLength = Mathf.Abs(velocity.x) + SKIN_WIDTH;

            for (int i = 0; i < horizontalRayCount; i++) {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

                if (hit.collider != null && hit.distance != 0) {
                    if (movedPassengers.Contains(hit.transform) == false) {
                        movedPassengers.Add(hit.transform);

                        float pushX = velocity.x - (hit.distance - SKIN_WIDTH) * directionX;
                        float pushY = -SKIN_WIDTH;

                        AddPassengerMovement(hit.transform, new Vector3(pushX, pushY, 0f), false, true);
                    }
                }

            }
        }

        //Passenger ontop of a horizontally or downward moving platform.
        if(directionY == -1 || velocity.y == 0 && velocity.x != 0) {
            float rayLength = SKIN_WIDTH * 2;

            for (int i = 0; i < verticalRayCount; i++) {
                Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

                if (hit.collider != null && hit.distance != 0) {
                    if (movedPassengers.Contains(hit.transform) == false) {
                        movedPassengers.Add(hit.transform);

                        float pushX =velocity.x;
                        float pushY = velocity.y;

                        AddPassengerMovement(hit.transform, new Vector3(pushX, pushY, 0f), true, false);
                    }
                }
            }
        }

    }

    private void AddPassengerMovement(Transform transform, Vector3 velocity, bool standingOnPlatform, bool moveBeforePlatform) {
        PassengerMovement entry = new PassengerMovement(transform, velocity, standingOnPlatform, moveBeforePlatform);
        passengerMovement.Add(entry);
    }


    private struct PassengerMovement {
        public Transform transform;
        public Vector3 velocity;
        public bool standingOnPlatform;
        public bool moveBeforePlatform;

        public PassengerMovement(Transform transform, Vector3 velocity, bool standingOnPlatform, bool moveBeforePlatform) {
            this.transform = transform;
            this.velocity = velocity;
            this.standingOnPlatform = standingOnPlatform;
            this.moveBeforePlatform = moveBeforePlatform;
        }
    }


    private void OnDrawGizmos() {
        if(localWaypoints != null) {
            Gizmos.color = Color.red;
            float size = 0.3f;

            int count = localWaypoints.Length;

            for(int i = 0; i < count; i++) {
                Vector3 globalWaypointPosition = (Application.isPlaying) ? globalWaypoints[i] :  localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPosition - Vector3.up * size, globalWaypointPosition + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPosition - Vector3.left * size, globalWaypointPosition + Vector3.left * size);
            }

        }
    }
}
