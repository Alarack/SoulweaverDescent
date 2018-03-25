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

        //Debug.Log("is " + point + " within " + bounds.size);


        return bounds.Contains(point);
    }


    public bool HasFreeConnections() {
        return totalConnections != occupiedConnections;
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


}
