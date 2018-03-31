using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPanel : BasePanel {

    public MapEntry mapEntryTemplate;
    public RectTransform mapEntryHolder;



    private List<MapEntry> mapEntries = new List<MapEntry>();
    private LevelGenerator levelGenerator;

    public override void Initialize(PanelManager panelManager) {
        base.Initialize(panelManager);

        levelGenerator = GameManager.GetLevelGenerator();

    }

    public override void Open() {
        base.Open();

        ClearMap();
        CreateMap();
    }

    

    public void CreateMap(MapEntry previousEntry = null) {
        int count = levelGenerator.activeRooms.Count;

        for (int i = 0; i < count; i++) {
            Room currentRooom = levelGenerator.activeRooms[i];
            if (currentRooom.Explored) {
                MapEntry newEntry = CreateMapEntry(currentRooom);
            }
        }
    }

    private void ClearMap() {
        int count = mapEntries.Count;

        for (int i = 0; i < count; i++) {
            Destroy(mapEntries[i].gameObject);
        }

        mapEntries.Clear();
    }


    private MapEntry CreateMapEntry(Room room) {
        GameObject mapEntry = Instantiate(mapEntryTemplate.gameObject) as GameObject;
        mapEntry.SetActive(true);
        mapEntry.transform.SetParent(mapEntryHolder, false);

        mapEntry.transform.localPosition = room.mapPosition;

        MapEntry entryScript = mapEntry.GetComponent<MapEntry>();
        entryScript.Initialize(room, this);
        mapEntries.Add(entryScript);

        return entryScript;
    }



}
