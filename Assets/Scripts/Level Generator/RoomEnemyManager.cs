using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEnemyManager : MonoBehaviour {

    public EnemyCollectionData enemyCollectionData;

    public int maxSpawns;

    public List<Transform> spawnPoints = new List<Transform>();


    private List<Entity> spawnedEntities = new List<Entity>();
    private Room parentRoom;

    public void Initialize(Room parentRoom) {
        this.parentRoom = parentRoom;

        RegisterEventListeners();
    }

    private void RegisterEventListeners() {
        SystemGrid.EventManager.RegisterListener(Constants.GameEvent.EntityDied, OnEntityDied);
    }

    private void OnDisable() {
        SystemGrid.EventManager.RemoveMyListeners(this);
    }


    private void OnEntityDied(EventData data) {
        Entity target = data.GetMonoBehaviour("Target") as Entity;

        if (spawnedEntities.Contains(target) == false)
            return;

        spawnedEntities.Remove(target);

        if(spawnedEntities.Count == 0) {

        }

    }




    public Vector2 GetRandomSpawnPoint() {
        int randomIndex = Random.Range(0, spawnPoints.Count);

        return spawnPoints[randomIndex].position;
    }

    public void SpawnAtEachPoint() {
        int count = spawnPoints.Count;

        for (int i = 0; i < count; i++) {
            SpawnAtLocation(spawnPoints[i].position);
        }
    }

    public void SpawnAtLocation(Vector2 location) {
        GameObject enemy = Instantiate(enemyCollectionData.GetRandomEnemyByFloorDepth(parentRoom.floorDepth), location, Quaternion.identity) as GameObject;
        Entity entityScript = enemy.GetComponent<Entity>();

        spawnedEntities.Add(entityScript);

    }


}
