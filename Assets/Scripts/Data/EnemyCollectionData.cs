using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Collection Data")]
[System.Serializable]
public class EnemyCollectionData : ScriptableObject {

    public List<EnemyEntry> allEnemies = new List<EnemyEntry>();


    public List<GameObject> GetAllEnemiesByFloorDepth(Constants.FloorDepthName floor) {
        List<GameObject> results = new List<GameObject>();

        int count = allEnemies.Count;

        for (int i = 0; i < count; i++) {
            if (allEnemies[i].possibleLocations.Contains(floor)) {
                results.Add(allEnemies[i].enemyPrefab);
            }
        }

        return results;
    }

    public GameObject GetRandomEnemyByFloorDepth(Constants.FloorDepthName floor) {
        List<GameObject> options = GetAllEnemiesByFloorDepth(floor);
        int randomIndex = Random.Range(0, options.Count);

        return options[randomIndex];
    }


    [System.Serializable]
    public class EnemyEntry {
        public List<Constants.FloorDepthName> possibleLocations = new List<Constants.FloorDepthName>();
        public float threatValue;
        public GameObject enemyPrefab;
    }


}
