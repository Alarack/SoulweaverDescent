using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomCollectionData))]
public class RoomCollectionDataEditor : Editor {

    private RoomCollectionData _roomCollectionData;


    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();

        _roomCollectionData = (RoomCollectionData)target;


        //_roomCollectionData.startRooms = EditorHelper.DrawExtendedList("Start Rooms", _roomCollectionData.startRooms, "Room", DrawRoomList);
        _roomCollectionData.startRooms = EditorHelper.DrawList("Start Rooms", _roomCollectionData.startRooms, true, null, true, DrawSimpleRoomList);

        EditorGUILayout.Separator();
        //_roomCollectionData.topRooms = EditorHelper.DrawExtendedList("Top Rooms", _roomCollectionData.topRooms, "Room", DrawRoomList);
        _roomCollectionData.topRooms = EditorHelper.DrawList("Top Rooms", _roomCollectionData.topRooms, true, null, true, DrawSimpleRoomList);

        EditorGUILayout.Separator();
        //_roomCollectionData.bottomRooms = EditorHelper.DrawExtendedList("Bottom Rooms", _roomCollectionData.bottomRooms, "Room", DrawRoomList);
        _roomCollectionData.bottomRooms = EditorHelper.DrawList("Bottom Rooms", _roomCollectionData.bottomRooms, true, null, true, DrawSimpleRoomList);

        EditorGUILayout.Separator();
        //_roomCollectionData.leftRooms = EditorHelper.DrawExtendedList("Left Rooms", _roomCollectionData.leftRooms, "Room", DrawRoomList);
        _roomCollectionData.leftRooms = EditorHelper.DrawList("Left Rooms", _roomCollectionData.leftRooms, true, null, true, DrawSimpleRoomList);

        EditorGUILayout.Separator();
        //_roomCollectionData.rightRooms = EditorHelper.DrawExtendedList("Right Rooms", _roomCollectionData.rightRooms, "Room", DrawRoomList);
        _roomCollectionData.rightRooms = EditorHelper.DrawList("Right Rooms", _roomCollectionData.rightRooms, true, null, true, DrawSimpleRoomList);


        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }



    private Room DrawRoomList(Room entry) {
        entry.roomType = EditorHelper.EnumPopup("Room Type", entry.roomType);
        entry.totalConnections = EditorHelper.DrawList("Total Connections", entry.totalConnections, true, Room.RoomConnection.None, true, DrawListOfRoomConnections);

        //EditorGUILayout.Separator();
        //entry.doors = EditorHelper.DrawExtendedList("Doors", entry.doors, "Door", DrawRoomDoorList);
        //EditorGUILayout.Separator();

        entry.roomSize = EditorGUILayout.Vector2Field("Room Size", entry.roomSize);


        return entry;
    }



    private Room.RoomConnection DrawListOfRoomConnections(List<Room.RoomConnection> list, int index) {
        Room.RoomConnection result = EditorHelper.EnumPopup("Entry", list[index]);
        return result;
    }

    private Room.RoomDoor DrawRoomDoorList(Room.RoomDoor entry) {
        entry.connection = EditorHelper.EnumPopup("Connection", entry.connection);

        entry.door = EditorHelper.ObjectField("Door", entry.door, true);
        entry.seal = EditorHelper.ObjectField("Seal", entry.seal, true);


        return entry;
    }

    private Room DrawSimpleRoomList(List<Room> list, int index) {
        Room result = EditorHelper.ObjectField<Room>("Room", list[index]);

        return result;
    }

}
