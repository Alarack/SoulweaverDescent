using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    [Header("Basic Info")]
    public string entityName;

    [Header("Attack Origin Points")]
    public Transform leftShotOrigin;
    public Transform rightShotOrigin;
    public Transform topShotPoint;

    [Header("Entity Stats")]
    public StatCollectionData statTemplate;
    public StatCollection stats;

    //[Header("Inventory")]
    //public Inventory inventory;

    //public SpriteRenderer SpriteRenderer { get; protected set; }
    public Constants.EntityFacing Facing { get; set; }
    public Animator MyAnimator { get; protected set; }
    public AbilityManager AbilityManager { get; protected set; }
    public int SessionID { get; private set; }

    //protected EntityMovement movement;
    protected HealthDeathManager healthDeathManager;

    protected virtual void Awake() {
        SessionID = IDFactory.GenerateEntityID();
        GameManager.RegisterEntity(this);
    }


    protected virtual void Start() {
       // GameManager.RegisterEntity(this);
    }

}
