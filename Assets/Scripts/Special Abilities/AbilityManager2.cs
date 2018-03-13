using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager2 : MonoBehaviour {



    private Animator anim;
    private Controller2D controller;

    public void Initialize(Animator anim, Controller2D controller) {
        this.anim = anim;
        this.controller = controller;
    }

    public void LightAttack() {

        if(controller.collisionInfo.below == false) {
            Debug.Log("Performing Light Arial Attack");
        }
        else {
            Debug.Log("Performing Light Attack");
            anim.SetTrigger("LightAttack");
        }


    }

    public void HeavyAttack() {
        Debug.Log("Performing Heavy Attack");
        anim.SetTrigger("HeavyAttack");
    }


}
