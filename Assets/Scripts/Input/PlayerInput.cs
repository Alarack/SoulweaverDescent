using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour {

    private Player player;

    private void Start() {
        player = GetComponent<Player>();
    }

    private void Update() {
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

        //if (Input.GetButtonDown("Fire1")) {
        //    player.AbilityManager2.LightAttack();
        //}

        //if (Input.GetButtonDown("Fire2")) {

        //}

    }



}
