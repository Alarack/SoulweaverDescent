using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public int maxRooms = 10;
    public int passes = 3;

    public List<Room> activeRooms = new List<Room>();

    public Room[] starterRooms;

    public Room[] topRooms;
    public Room[] bottomRooms;
    public Room[] leftRooms;
    public Room[] rightRooms;


    public void Start() {
        CreateRoom(null, maxRooms);

        Branch();

        SealUnusedDoors();
    }

    public void Branch() {

        for(int i = 0; i < passes; i++) {
            Room startPoint = GetRandomRoomWithFreeConnections();

            CreateRoom(startPoint, maxRooms);

        }

    }

    public void SealUnusedDoors() {
        int count = activeRooms.Count;

        for (int i = 0; i < count; i++) {
            activeRooms[i].SealUnusedConnections();
        }
    }

    private void RestartRoomCreation(Room startPoint, int roomsToMake) {
        if(startPoint == null) {
            Debug.Log("Restart was given a null room");
            return;
        }

        CreateRoom(startPoint, roomsToMake);
    }


    public void CreateRoom(Room lastRoom = null, int roomsToMake = 10) {
        Vector2 spawnLocation = Vector2.zero;
        GameObject newRoomGameObject;
        Room newRoom = null;

        //Debug.Log("Making " + roomsToMake + " rooms");

        if (lastRoom == null) {
            spawnLocation = Vector2.zero;

            newRoomGameObject = Instantiate(starterRooms[GetRandomRoomIndex(starterRooms)].gameObject, spawnLocation, Quaternion.identity) as GameObject;
            string numberName1 = newRoomGameObject.name + " " + activeRooms.Count;
            newRoomGameObject.name = numberName1;

            newRoom = newRoomGameObject.GetComponent<Room>();
            newRoom.roomPosition = spawnLocation;

            activeRooms.Add(newRoom);

            //if(activeRooms.Count < maxRooms) {
            //    CreateRoom(newRoom);
            //}

            if(roomsToMake > 0) {
                CreateRoom(newRoom, roomsToMake - 1);
            }

            return;
        }

        Room.RoomConnection targetConnection = lastRoom.GetRandomFreeConnection();
        Room.RoomConnection oppositeConnection = GetOppositeConnection(targetConnection);

        if (targetConnection == Room.RoomConnection.None) {
            Debug.Log(lastRoom.gameObject.name + " has no free connections");
            RestartRoomCreation(GetRandomRoomWithFreeConnections(), roomsToMake);
            return;
        }


        Room nextRoom = GetRandomRoom(oppositeConnection);
        spawnLocation = GetSpawnLocationFromConnection(lastRoom, targetConnection, nextRoom);

        if(spawnLocation == Vector2.zero) {
            Debug.Log("Map ran into itself");
            RestartRoomCreation(GetRandomRoomWithFreeConnections(), roomsToMake);
            return;
        }



        newRoomGameObject = Instantiate(nextRoom.gameObject, spawnLocation, Quaternion.identity) as GameObject;
        string numberName = newRoomGameObject.name + " " + activeRooms.Count;
        newRoomGameObject.name = numberName;

        newRoom = newRoomGameObject.GetComponent<Room>();
        newRoom.roomPosition = spawnLocation;

        newRoom.occupiedConnections.Add(oppositeConnection);
        lastRoom.occupiedConnections.Add(targetConnection);

        activeRooms.Add(newRoom);

        //if (activeRooms.Count < maxRooms) {
        //    CreateRoom(newRoom);
        //}

        if (roomsToMake > 0) {
            CreateRoom(newRoom, roomsToMake - 1);
        }


    }

    private Vector2 GetSpawnLocationFromConnection(Room lastRoom, Room.RoomConnection connection, Room nextRoom) {
        Vector2 result = lastRoom.roomPosition;

        switch (connection) {
            case Room.RoomConnection.Top:
                result += new Vector2(0f, lastRoom.roomSize.y) + nextRoom.roomOffset - lastRoom.roomOffset;
                break;

            case Room.RoomConnection.Bottom:
                result -= new Vector2(0f, lastRoom.roomSize.y) - nextRoom.roomOffset + lastRoom.roomOffset;
                break;

            case Room.RoomConnection.Left:
                result -= new Vector2(lastRoom.roomSize.x, 0f) - nextRoom.roomOffset + lastRoom.roomOffset;
                break;

            case Room.RoomConnection.Right:
                result += new Vector2(lastRoom.roomSize.x, 0f) + nextRoom.roomOffset - lastRoom.roomOffset;
                break;
        }

        for(int i = 0; i < activeRooms.Count; i++) {
            if (activeRooms[i].IsPointInRoom(result) || activeRooms[i].DoesRoomOverlap(nextRoom.roomSize, result)) {
                //Debug.Log(lastRoom.gameObject.name + " is inside " + activeRooms[i].gameObject.name);
                return Vector2.zero;
            }
        }


        return result;
    }

    private Room.RoomConnection GetOppositeConnection(Room.RoomConnection connection) {
        switch (connection) {
            case Room.RoomConnection.Top:
                return Room.RoomConnection.Bottom;

            case Room.RoomConnection.Bottom:
                return Room.RoomConnection.Top;

            case Room.RoomConnection.Left:
                return Room.RoomConnection.Right;

            case Room.RoomConnection.Right:
                return Room.RoomConnection.Left;

            default:
                return Room.RoomConnection.None;
        }
    }

    private Room GetRandomRoom(Room.RoomConnection connection = Room.RoomConnection.None) {
        switch (connection) {
            case Room.RoomConnection.Top:
                return topRooms[GetRandomRoomIndex(topRooms)];

            case Room.RoomConnection.Bottom:
                return bottomRooms[GetRandomRoomIndex(bottomRooms)];

            case Room.RoomConnection.Left:
                return leftRooms[GetRandomRoomIndex(leftRooms)];

            case Room.RoomConnection.Right:
                return rightRooms[GetRandomRoomIndex(rightRooms)];

        }


        return null;
    }

    private Room GetRandomRoomWithFreeConnections() {
        int count = activeRooms.Count;

        List<Room> possibleRooms = new List<Room>();

        for (int i = 0; i < count; i++) {
            if (activeRooms[i].HasFreeConnections()) {
                possibleRooms.Add(activeRooms[i]);
            }
        }

        if (possibleRooms.Count < 1)
            return null;

        int randomIndex = Random.Range(0, possibleRooms.Count);

        return possibleRooms[randomIndex];
    }





    private int GetRandomRoomIndex(Room[] rooms) {
        int randomIndex = Random.Range(0, rooms.Length);

        return randomIndex;
    }



}
