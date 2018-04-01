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

    [Header("Room Connections")]
    public List<RoomConnection> totalConnections = new List<RoomConnection>();

    [HideInInspector]
    public List<RoomConnection> occupiedConnections = new List<RoomConnection>();

    [Header("Doors")]
    public List<RoomDoor> doors = new List<RoomDoor>();

    [Header("Size and offset")]
    public Vector2 roomSize = new Vector2(18f, 10f);
    public Vector2 roomOffset = new Vector2(0f, 0f);

    [Header("Entry Positions")]
    public List<Transform> entryPositions = new List<Transform>();

    [HideInInspector]
    public Vector2 roomPosition;

    [HideInInspector]
    public Vector2 mapPosition;

    [HideInInspector]
    public Bounds bounds;

    public bool Explored { get; private set; }
    public bool Occupied { get; private set; }
    public RoomEntranceManager roomEntranceManager { get; private set; }

    public void Awake() {
        bounds = new Bounds(transform.localPosition, roomSize);
        roomEntranceManager = GetComponentInChildren<RoomEntranceManager>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Return) && Occupied && roomEntranceManager != null) {
            roomEntranceManager.ToggleDoors();
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

        if (roomEntranceManager == null)
            return;

        int count = roomEntranceManager.entrances.Length;

        for (int i = 0; i < count; i++) {
            if (occupiedConnections.Contains(roomEntranceManager.entrances[i].connection))
                continue;

            roomEntranceManager.entrances[i].Seal();
        }
        


        //int count = doors.Count;

        //for (int i = 0; i < count; i++) {
        //    if (occupiedConnections.Contains(doors[i].connection)) {
        //        //Debug.Log(doors[i].connection + " is already occupied");
        //        continue;
        //    }
        //    doors[i].Seal();
        //}
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

        Debug.Log("Player Has entered " + gameObject.name);

        Vector2 hafCamSize = new Vector2(9f, 5f);

        Vector2 minXPos = roomPosition - new Vector2((roomSize.x /2) - hafCamSize.x, 0f);
        Vector2 minYPos = roomPosition - new Vector2(0f, (roomSize.y / 2) - hafCamSize.y);

        Vector2 maxXPos = roomPosition + new Vector2((roomSize.x / 2) - hafCamSize.x, 0f);
        Vector2 maxYPos = roomPosition + new Vector2(0f, (roomSize.y / 2) - hafCamSize.y);


        MainHUD.SetCameraBounds(minXPos, maxXPos, minYPos, maxYPos);

        Explored = true;
        Occupied = true;

        if(entryPositions.Count > 0)
            other.transform.position = GetNearestEntryPosition(other.transform.position);

        if(roomType != RoomType.Start) {
            roomEntranceManager.CloseDoors();
        }

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
