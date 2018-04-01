using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Room Data Collection")]
[System.Serializable]
public class RoomCollectionData : ScriptableObject {


    public List<Room> startRooms = new List<Room>();
    public List<Room> topRooms = new List<Room>();
    public List<Room> bottomRooms = new List<Room>();
    public List<Room> leftRooms = new List<Room>();
    public List<Room> rightRooms = new List<Room>();



}
