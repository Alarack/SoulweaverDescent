using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RoomBackgroundGenerator : MonoBehaviour {

    public RoomSpriteCollectionData spriteCollectionData;
    public RoomSpriteCollectionData.TileSet tileSet;
    public RoomSpriteCollectionData.TileType tileType;



    //public List<Sprite> backgroundStones = new List<Sprite>();
    public GameObject stoneTemplate;
    public Transform backgroundHolder;
    public int width;
    public int height;

    public List<GameObject> spawnedObjects = new List<GameObject>();


    public Color RandomTint() {
        Color tint = Random.ColorHSV(0f, 0f, 0f, 0f, .55f, .75f);

        return tint;
    }

    public Sprite RandomSprite() {
        List<Sprite> sprites = spriteCollectionData.GetSpritesByTileSetAndType(tileSet, tileType);


        int randomIndex = Random.Range(0, sprites.Count);


        return sprites[randomIndex];

        //return backgroundStones[randomIndex];
    }


    public void SetTemplate() {
        SpriteRenderer spriteRenderer = stoneTemplate.GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = RandomSprite();
        spriteRenderer.color = RandomTint();

    }

    public void SetTemplate(GameObject tile) {
        SpriteRenderer spriteRenderer = tile.GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = RandomSprite();
        spriteRenderer.color = RandomTint();
    }

    public void ClearTiles() {
        int count = spawnedObjects.Count;

        for (int i = 0; i < count; i++) {
            DestroyImmediate(spawnedObjects[i]);
        }
        spawnedObjects.Clear();
    }

    public void SpawnTiles() {
        ClearTiles();

        for (int i = 0; i < height * 2; i++) {
            for (int j = 0; j < (width +1); j++) {
                GameObject tile = Instantiate(stoneTemplate) as GameObject;
                tile.SetActive(true);
                tile.transform.SetParent(backgroundHolder, false);

                float yPos = (float)i / 2f;

                if (i.IsOdd()) {
                    //Debug.Log(yPos+ " is I ODD" + " " + i);
                    tile.transform.localPosition = new Vector2(j -0.5f, yPos * -1);
                }
                else {
                    //Debug.Log(yPos + " is I Even" + " " + i);
                    tile.transform.localPosition = new Vector2(j, yPos * -1);
                }

                SetTemplate(tile);
                spawnedObjects.Add(tile);
            }
        }
    }


}
