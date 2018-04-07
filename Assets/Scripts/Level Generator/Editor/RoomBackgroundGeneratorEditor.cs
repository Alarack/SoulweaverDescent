﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomBackgroundGenerator))]
public class RoomBackgroundGeneratorEditor : Editor {

    private RoomBackgroundGenerator _roomBackgroundGenerator;


    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
        _roomBackgroundGenerator = (RoomBackgroundGenerator)target;

        _roomBackgroundGenerator.backgroundStones = EditorHelper.DrawList("Background Tiles", _roomBackgroundGenerator.backgroundStones, true, null, true, DrawListofSprites);

        EditorGUILayout.Separator();

        _roomBackgroundGenerator.stoneTemplate = EditorHelper.ObjectField<GameObject>(_roomBackgroundGenerator.stoneTemplate, true);

        EditorGUILayout.Separator();

        _roomBackgroundGenerator.width = EditorGUILayout.IntField("Width", _roomBackgroundGenerator.width);
        _roomBackgroundGenerator.height = EditorGUILayout.IntField("Height", _roomBackgroundGenerator.height);
        _roomBackgroundGenerator.backgroundHolder = EditorHelper.ObjectField<Transform>(_roomBackgroundGenerator.backgroundHolder, true);


        if (GUILayout.Button("Test")) {
            _roomBackgroundGenerator.SetTemplate();
        }

        if (GUILayout.Button("Create Tiles")) {
            _roomBackgroundGenerator.SpawnTiles();
        }

        if (GUILayout.Button("Clear Tiles")) {
            _roomBackgroundGenerator.ClearTiles();
        }


        if (GUI.changed)
            EditorUtility.SetDirty(target);


    }






    private Sprite DrawListofSprites(List<Sprite> sprites, int index) {
        Sprite result = EditorHelper.ObjectField<Sprite>("Sprite", sprites[index]);
        return result;
    }

}
