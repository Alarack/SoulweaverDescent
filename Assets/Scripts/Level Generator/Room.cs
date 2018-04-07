using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    public enum RoomType {
        Basic = 0,
        Start = 1,
        Exit = 2,
    }

    public enum RoomConnection {
        Top = 0,
        Bottom = 1,
        Left = 2,
        Right = 3,
        None = 4,
    }

    [Header("Room Type")]
    public RoomType roomType;

    [Header("Floor Depth")]
    public Constants.FloorDepthName floorDepth;

    [Header("Room Connections")]
    public List<RoomConnection> totalConnections = new List<RoomConnection>();

    [Header("Size and offset")]
    public Vector2 roomSize = new Vector2(18f, 10f);
    public Vector2 roomOffset = new Vector2(0f, 0f);

    [Header("Entry Positions")]
    public List<Transform> entryPositions = new List<Transform>();

    [HideInInspector]
    public List<RoomConnection> occupiedConnections = new List<RoomConnection>();

    [HideInInspector]
    public Vector2 mapPosition;

    [HideInInspector]
    public Bounds bounds;

    public bool Explored { get; private set; }
    public bool Occupied { get; private set; }
    public RoomEntranceManager RoomEntranceManager { get; private set; }

    private RoomEnemyManager roomEnemyManager;
    private RoomObjectiveManager roomObjectiveManager;

    public void Awake() {
        bounds = new Bounds(transform.localPosition, roomSize);
        RoomEntranceManager = GetComponentInChildren<RoomEntranceManager>();
        roomEnemyManager = GetComponent<RoomEnemyManager>();
        roomObjectiveManager = GetComponent<RoomObjectiveManager>();

    }

    public void Initialize() {
        if (roomEnemyManager != null)
            roomEnemyManager.Initialize(this);

        if (roomObjectiveManager != null) 
            roomObjectiveManager.Initialize(this, roomEnemyManager);

    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Return) && Occupied && RoomEntranceManager != null) {
            RoomEntranceManager.ToggleDoors();
        }
    }

    public bool IsPointInRoom(Vector2 point) {
        return bounds.Contains(point);
    }

    public bool DoesRoomOverlap(Vector2 roomSize, Vector2 point) {
        float topCenter = point.y - 1 + roomSize.y / 2;
        float bottomCenter = point.y + 1 - roomSize.y / 2;
        float leftCenter = point.x + 1 - roomSize.x / 2;
        float rightCenter = point.x - 1 + roomSize.x / 2;

        Vector2 testTop = new Vector2(point.x, topCenter);
        Vector2 testBottom = new Vector2(point.x, bottomCenter);
        Vector2 testLeft = new Vector2(leftCenter, point.y);
        Vector2 testRight = new Vector2(rightCenter, point.y);

        if (IsPointInRoom(testTop)) {
            //Debug.Log(testTop + " is inside " + gameObject.name + " a top");
            return true;
        }
        if (IsPointInRoom(testBottom)) {
            //Debug.Log(testBottom + " is inside " + gameObject.name + " a bottom");
            return true;
        }
        if (IsPointInRoom(testLeft)) {
            //Debug.Log(testLeft + " is inside " + gameObject.name + " a left");
            return true;
        }
        if (IsPointInRoom(testRight)) {
            //Debug.Log(testRight + " is inside " + gameObject.name + " a right");
            return true;
        }

        return false;
    }

    public bool HasFreeConnections() {
        return totalConnections.Count != occupiedConnections.Count;
    }

    public void SealUnusedConnections() {
        if (RoomEntranceManager == null) {
            Debug.LogError(gameObject.name + " does not have a Room Entrance Manager Script");
            return;
        }

        int count = RoomEntranceManager.entrances.Length;

        for (int i = 0; i < count; i++) {
            if (occupiedConnections.Contains(RoomEntranceManager.entrances[i].connection))
                continue;

            RoomEntranceManager.entrances[i].Seal();
        }
    }

    public RoomConnection GetRandomFreeConnection() {
        int randomIndex;
        //int occupiedCount = occupiedConnections.Count;

        if (HasFreeConnections() == false)
            return RoomConnection.None;

        List<RoomConnection> freeConnection = new List<RoomConnection>();

        int totalCount = totalConnections.Count;
        for (int i = 0; i < totalCount; i++) {
            if (occupiedConnections.Contains(totalConnections[i])) {
                continue;
            }
            else {
                freeConnection.Add(totalConnections[i]);
            }
        }

        randomIndex = Random.Range(0, freeConnection.Count);

        return freeConnection[randomIndex];
    }

    public Vector2 GetNearestEntryPosition(Vector2 location) {
        return Finder.GetNearestPoint(entryPositions, location).position;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag != "Player")
            return;

        //Debug.Log("Player Has entered " + gameObject.name);
        SetCameraConfines();

        Explored = true;
        Occupied = true;

        if(entryPositions.Count > 0)
            other.transform.position = GetNearestEntryPosition(other.transform.position);

        if(roomType != RoomType.Start) {
            RoomEntranceManager.CloseDoors();
        }

    }

    private void SetCameraConfines() {
        Vector2 halfCamSize = new Vector2(9f, 5f);

        Vector2 minXPos = (Vector2)transform.position - new Vector2((roomSize.x / 2) - halfCamSize.x, 0f);
        Vector2 minYPos = (Vector2)transform.position - new Vector2(0f, (roomSize.y / 2) - halfCamSize.y);
        Vector2 maxXPos = (Vector2)transform.position + new Vector2((roomSize.x / 2) - halfCamSize.x, 0f);
        Vector2 maxYPos = (Vector2)transform.position + new Vector2(0f, (roomSize.y / 2) - halfCamSize.y);

        MainHUD.SetCameraBounds(minXPos, maxXPos, minYPos, maxYPos);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag != "Player")
            return;

        Occupied = false;
    }



    [System.Serializable]
    public struct RoomDoor {
        public GameObject door;
        public GameObject seal;

        public RoomConnection connection;

        public void Seal() {
            door.SetActive(false);
            seal.SetActive(true);
        }
    }


}
