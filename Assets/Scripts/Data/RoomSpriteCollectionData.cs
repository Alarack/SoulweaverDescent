using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Room Sprite Collection Data")]
[System.Serializable]
public class RoomSpriteCollectionData : ScriptableObject {

    public enum TileSet {
        None = 0,
        TempleStone = 1,
    }

    public enum TileType {
        None = 0,
        BackgroundBrick = 1,

    }

    public List<RoomSpriteCollection> spriteCollections = new List<RoomSpriteCollection>();


    public List<Sprite> GetSpritesByTileSetAndType(TileSet set, TileType type) {
        RoomSpriteCollection targetCollection = GetSpriteCollectionByTileSet(set);
        List<Sprite> results = GetSpritesByTileType(targetCollection, type);

        return results;
    }

    public RoomSpriteCollection GetSpriteCollectionByTileSet(TileSet set) {
        int count = spriteCollections.Count;

        for (int i = 0; i < count; i++) {
            if (spriteCollections[i].tileSet == set) {
                return spriteCollections[i];
            }
        }

        return null;
    }

    public List<Sprite> GetSpritesByTileType(RoomSpriteCollection collection, TileType type) {
        if (collection == null)
            return null;

        int count = collection.tileSets.Count;

        for (int i = 0; i < count; i++) {
            if(collection.tileSets[i].tileType == type) {
                return collection.tileSets[i].sprites;
            }
        }

        return null;
    }


    [System.Serializable]
    public class RoomSpriteCollection {
        public TileSet tileSet = TileSet.TempleStone;
        public List<TileSubset> tileSets = new List<TileSubset>();
    }

    [System.Serializable]
    public class TileSubset {
        public TileType tileType = TileType.BackgroundBrick;
        public List<Sprite> sprites = new List<Sprite>();
    }

}
