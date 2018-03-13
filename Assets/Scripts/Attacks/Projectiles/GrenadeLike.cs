using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLike : Projectile {

    public bool explodeOnContact;
    public float fuseTime;
    public float bounceModifier;
    public int maxBounces;


    //public LayerMask bounceLayer;

    private Timer fuseTimer;
    private RaycastHit2D hitTest;

    private Rigidbody2D myBody;
    private int currentBounces;

    public override void Initialize(Effect parentEffect, LayerMask mask, float life = 0, float damage = 0) {
        base.Initialize(parentEffect, mask, life, damage);

        fuseTimer = new Timer(fuseTime, false, CleanUp);
        myBody = GetComponent<Rigidbody2D>();
    }

    private void Update() {

        //Debug.DrawRay(transform.position, GetComponent<Rigidbody2D>().velocity, Color.red);

        hitTest = Physics2D.Raycast(transform.position, myBody.velocity, 0.1f, LayerMask);

        if (hitTest.collider != null) {
            if (explodeOnContact) {
                parentEffect.Apply(hitTest.collider.gameObject);
                CleanUp();
            }

        }

    }

    protected override void OnTriggerEnter2D(Collider2D other) {
        if ((LayerMask & 1 << other.gameObject.layer) == 1 << other.gameObject.layer && explodeOnContact) {
            parentEffect.Apply(other.gameObject);
            CleanUp();
        }

        //if ((bounceLayer & 1 << other.gameObject.layer) == 1 << other.gameObject.layer) {
        //    Bounce();
        //}
    }

    private void OnCollisionEnter2D(Collision2D other) {

        if (maxBounces != 0) {
            if (currentBounces < maxBounces) {
                currentBounces++;
            }
            else {
                CleanUp();
                return;
            }
        }

        if (myBody.velocity.magnitude > 2f) {
            myBody.velocity *= bounceModifier;
            //Debug.Log(myBody.velocity.magnitude + " is the mag");
        }
        else {
            myBody.velocity = Vector2.zero;
        }

    }


    private void Bounce() {

        if (maxBounces != 0) {
            if (currentBounces < maxBounces) {
                currentBounces++;
            }
            else {
                CleanUp();
                return;
            }
        }
           

        Rigidbody2D body = GetComponent<Rigidbody2D>();
        //Collider2D col = GetComponent<Collider2D>();
        //float width = col.bounds.size.x;

        //RaycastHit2D hit = Physics2D.Raycast(hitTest.point + hitTest.normal, body.velocity, width * 5, bounceLayer);

        if (hitTest.collider != null) {

            Vector2 reflect = Vector2.Reflect(body.velocity, hitTest.normal);

            Vector2 variance = Random.insideUnitCircle * 5f;

            body.velocity = reflect * bounceModifier;

        }
        else {
            Debug.Log("Hit nothing");
        }

    }
}
