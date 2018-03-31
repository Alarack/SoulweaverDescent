using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour {

    private Player player;
    private PanelManager panelManager;

    private void Start() {
        player = GetComponent<Player>();
        panelManager = MainHUD.GetPanelManager();
    }

    private void Update() {
        if (panelManager.IsPanelOpen())
            return;

        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        player.SetDirectionalInput(directionalInput);

        if (Input.GetButtonDown("Jump")) {
            player.OnJumpInputDown();
        }

        if (Input.GetButtonUp("Jump")) {
            player.OnJumpInputUp();
        }

        for (int i = 1; i <= 6; i++) {
            if (Input.GetButtonDown("Fire" + i)) {
                player.OnAbilitySlotKeyPressed(i);
            }
        }

    }



}
