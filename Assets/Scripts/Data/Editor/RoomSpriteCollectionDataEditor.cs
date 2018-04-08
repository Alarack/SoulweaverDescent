using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomSpriteCollectionData))]
public class RoomSpriteCollectionDataEditor : Editor {

    private RoomSpriteCollectionData _collectionData;

    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();

        _collectionData = (RoomSpriteCollectionData)target;


        _collectionData.spriteCollections = EditorHelper.DrawExtendedList("All Tiles", _collectionData.spriteCollections, "Collection", DrawRoomSpriteCollections);



        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }



    private RoomSpriteCollectionData.RoomSpriteCollection DrawRoomSpriteCollections(RoomSpriteCollectionData.RoomSpriteCollection entry) {
        entry.tileSet = EditorHelper.EnumPopup("Tile Set", entry.tileSet);
        entry.tileSets = EditorHelper.DrawExtendedList("Subsets", entry.tileSets, "SubSet", DrawTileSubsets);


        return entry;
    }


    private RoomSpriteCollectionData.TileSubset DrawTileSubsets(RoomSpriteCollectionData.TileSubset entry) {
        entry.tileType = EditorHelper.EnumPopup("Tile Type", entry.tileType);
        entry.sprites = EditorHelper.DrawList("Sprites", entry.sprites, true, null, true, EditorListHelper.DrawListofSprites);



        return entry;
    }


}
