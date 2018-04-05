using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBounce : MonoBehaviour {

    //public int numBounce;
    public float bounceHeight = 7f;


    public Transform rayOrigin;
    public LayerMask mask;

    private Rigidbody2D myBody;

    private void Awake() {
        myBody = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        CheckGround();
    }

    private void CheckGround() {
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin.position, Vector2.down, 0.2f, mask);

        if (hit && bounceHeight > 1f) {
            Bounce();
        }
    }


    private void Bounce() {
        //SoundManager.PlaySingle(myAudio, bounceSound, 0.2f, false);
        int xvar = Random.Range(-5, 6);
        int yvar = Random.Range(0, 6);
        myBody.velocity = new Vector2(myBody.velocity.x + xvar, bounceHeight + yvar);
        myBody.angularVelocity = xvar * 100;
        bounceHeight -= 1;
        //	numBounce--;
    }


}
