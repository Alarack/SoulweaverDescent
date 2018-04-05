using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapEntry : MonoBehaviour {

    [Header("Images")]
    public Image mainImage;
    public Image icon;
    public Image playerIndicator;

    [Header("Conneections")]
    public List<MapDoor> doors = new List<MapDoor>();

    private Room room;
    private MapPanel mapPanel;

    public void Initialize(Room room, MapPanel mapPanel) {
        this.mapPanel = mapPanel;
        this.room = room;

        ShowConnections();
        SetRoomVisual();
    }


    public void ShowConnections() {
        int count = doors.Count;

        for (int i = 0; i < count; i++) {
            if (room.occupiedConnections.Contains(doors[i].connection)) {
                doors[i].Show();
            }
            else {
                doors[i].Hide();
            }
        }
    }

    public void SetRoomVisual() {
        switch (room.roomType) {
            case Room.RoomType.Start:
                mainImage.color = Color.cyan;
                break;
        }

        if (room.Occupied)
            playerIndicator.gameObject.SetActive(true);
        else {
            playerIndicator.gameObject.SetActive(false);
        }
    }


    [System.Serializable]
    public struct MapDoor {
        public Room.RoomConnection connection;
        public Image connectionImage;

        public void Show() {
            connectionImage.gameObject.SetActive(true);
        }

        public void Hide() {
            connectionImage.gameObject.SetActive(false);
        }
    }

}
