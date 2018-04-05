using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHUD : MonoBehaviour {

    public static MainHUD mainHud;
    public PlayerQuickBar quickBar;
    public PanelManager panelManager;
    //public GameResourceDisplay resourceDisplay;
    public Camera mainCamera;
    public Camera uiCam;

    private CameraFollow cameraFollow;


    private void Awake() {
        if (mainHud == null)
            mainHud = this;
        else
            Destroy(this);
    }

    private void Start() {
        //resourceDisplay.Initialize();
        cameraFollow = mainCamera.GetComponent<CameraFollow>();
    }

    public static void SetCameraBounds(Vector2 minX, Vector2 maxX, Vector2 minY, Vector2 maxY) {
        mainHud.cameraFollow.minXPos = minX;
        mainHud.cameraFollow.maxXPos = maxX;
        mainHud.cameraFollow.maxYPos = maxY;
        mainHud.cameraFollow.minYPos = minY;

    }

    public static void SetPlayerSlot(AbilityCard abilityCard, PlayerAbilitySlot.SlotType slotType = PlayerAbilitySlot.SlotType.Cycling) {
        if (mainHud != null) {
            mainHud.quickBar.SetQuickBarSlot(abilityCard, slotType);
        }
    }

    public static void ClearPlayerSlot(AbilityCard abilityCard, PlayerAbilitySlot.SlotType slotType = PlayerAbilitySlot.SlotType.Cycling) {
        mainHud.quickBar.ClearQuickBarSlot(abilityCard, slotType);
    }
    
    public static PlayerAbilitySlot GetAbilitySlotByIndex(int index) {
        return mainHud.quickBar.slots[index];
    }

    public static PanelManager GetPanelManager() {
        return mainHud.panelManager;
    }
}
