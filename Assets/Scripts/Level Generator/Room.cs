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

    public RoomType roomType;
    public List<RoomConnection> totalConnections = new List<RoomConnection>();
    [Space(10)]
    public List<RoomConnection> occupiedConnections = new List<RoomConnection>();
    [Space(10)]
    public List<RoomDoor> doors = new List<RoomDoor>();

    public Vector2 roomSize = new Vector2(18f, 10f);
    public Vector2 roomOffset = new Vector2(0f, 0f);

    [HideInInspector]
    public Vector2 roomPosition;

    [HideInInspector]
    public Bounds bounds;


    public void Awake() {
        bounds = new Bounds(transform.localPosition, roomSize);
    }

    public bool IsPointInRoom(Vector2 point) {
        return bounds.Contains(point);
    }

    public bool HasFreeConnections() {
        return totalConnections != occupiedConnections;
    }

    public void SealUnusedConnections() {
        int count = doors.Count;

        for (int i = 0; i < count; i++) {
            if (occupiedConnections.Contains(doors[i].connection)) {
                //Debug.Log(doors[i].connection + " is already occupied");
                continue;
            }

            //Debug.Log(doors[i].connection + " unused");
            doors[i].Seal();
            //occupiedConnections.Add(doors[i].connection);
        }
    }

    public RoomConnection GetRandomFreeConnection() {
        int randomIndex;

        int occupiedCount = occupiedConnections.Count;
        int totalCount = totalConnections.Count;

        if (occupiedCount == totalCount)
            return RoomConnection.None;


        List<RoomConnection> freeConnection = new List<RoomConnection>();

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
