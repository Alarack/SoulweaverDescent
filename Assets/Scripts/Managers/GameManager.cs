using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager gameManager;

    //[Header("Items")]
    //public ItemPools itemPools;

    //[Header("Spawns")]
    //public SpawnManager spawnManager;

    [Header("Default Stats")]
    public StatCollectionData defaultStats;
    public StatCollectionData defaultProjectileStats;

    //[Header("Difficulty")]
    //public GameDifficulty gameDifficulty;

    //[Header("Panels")]
    //public PauseMenu pauseMenu;

    [Header("Scene Changes")]
    public string mainMenuScene;

    public static bool GamePaused { get; set; }

    private static List<Entity> allEntities = new List<Entity>();

    private void Awake() {

        if (gameManager == null)
            gameManager = this;
        else {
            Destroy(gameObject);
        }

    }

    private void Start() {
        //if (spawnManager != null)
        //    spawnManager.Initialize();

        //if (gameDifficulty != null)
        //    gameDifficulty.Initialize();
    }

    private void Update() {
        //if (gameDifficulty != null)
        //    gameDifficulty.ManagedUpdate();

        //if (Input.GetKeyDown(KeyCode.Escape)) {
        //    PauseGame();
        //}
    }

    private void PauseGame() {
        //pauseMenu.TogglePause();
    }


    public static StatCollectionData GetDefaultStatCollection() {
        return gameManager.defaultStats;
    }

    public static StatCollectionData GetDefaultProjectileStats() {
        return gameManager.defaultProjectileStats;
    }

    //public static ItemPools GetItemPools() {
    //    return gameManager.itemPools;
    //}

    public static void RegisterEntity(Entity target) {
        if (!allEntities.Contains(target))
            allEntities.Add(target);
        else {
            Debug.LogError(target.entityName + " is already registered");
        }
    }

    public static void UnregisterEntity(Entity target) {
        if (allEntities.Contains(target))
            allEntities.Remove(target);
        else {
            Debug.LogError(target.entityName + " is not registered");
        }
    }

    public static Entity GetEntityByID(int id) {
        int count = allEntities.Count;

        for (int i = 0; i < count; i++) {
            if (allEntities[i].SessionID == id) {
                return allEntities[i];
            }
        }

        return null;
    }

    public static Entity GetPlayer() {
        int count = allEntities.Count;

        for (int i = 0; i < count; i++) {
            if (allEntities[i].gameObject.tag == "Player") {
                return allEntities[i];
            }
        }

        return null;
    }

    public static void ReturnToMainMenu() {
        SceneManager.LoadScene(gameManager.mainMenuScene);
    }

    //public static List<DifficultyData.DifficultyEntry> GetDifficultyEntries() {
    //    return gameManager.gameDifficulty.difficultyData.difficultyEntries;
    //}


}
