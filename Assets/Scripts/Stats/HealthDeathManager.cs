using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class HealthDeathManager : MonoBehaviour {

    public bool cheat;
    public GameObject deathEffect;
    //public HealthBar healthBar;

    protected Entity owner;
    //protected LootManager lootManager;




    public virtual void Initialize(Entity owner) {
        this.owner = owner;

        //lootManager = GetComponent<LootManager>();

        //RegisterListeners();
    }

    //protected virtual void RegisterListeners() {
    //    EventGrid.EventManager.RegisterListener(Constants.GameEvent.StatChanged, OnStatChanged);
    //}

    //public virtual void RemoveListeners() {
    //    EventGrid.EventManager.RemoveMyListeners(this);
    //}

    //protected void OnStatChanged(EventData data) {
    //    Constants.BaseStatType stat = (Constants.BaseStatType)data.GetInt("Stat");
    //    Entity target = data.GetMonoBehaviour("Target") as Entity;
    //    Entity cause = data.GetMonoBehaviour("Cause") as Entity;

    //    if (target != owner)
    //        return;

    //    if(stat == Constants.BaseStatType.Health) {
    //        //Debug.Log(owner.stats.GetStatModifiedValue(Constants.BaseStatType.Health) + " is the health of " + owner.gameObject.name);

    //        float currentHealth = owner.stats.GetStatModifiedValue(Constants.BaseStatType.Health);
    //        float maxHealth = owner.stats.GetStatMaxValue(Constants.BaseStatType.Health);

    //        UpdateHealthBar(currentHealth, maxHealth);

    //        if (currentHealth <= 0f) {
    //            if(!cheat)
    //                Die(cause);
    //        }
    //    }

    //}

    private void UpdateHealthBar(float currentHealth, float maxHealth) {
        //if (healthBar == null)
        //    return;

        //float ratio = currentHealth / maxHealth;

        //healthBar.AdjustHealthBar(ratio);
    }



    protected virtual void Die(Entity cause = null) {
        ShowDeathEffect();
        //GameManager.UnregisterEntity(owner);
        //owner.UnregisterListeners();

        ////Debug.Log(owner.gameObject + " has died");

        //if(owner.gameObject.tag == "Player") {
        //    GameManager.ReturnToMainMenu();
        //    return;
        //}

        //if(lootManager != null) {
        //    lootManager.SpawnLoot();
        //}

        //EventData data = new EventData();
        //data.AddMonoBehaviour("Target", owner);
        //data.AddMonoBehaviour("Cause", cause);

        //EventGrid.EventManager.SendEvent(Constants.GameEvent.EntityDied, data);


        Destroy(owner.gameObject);
    }

    protected virtual void ShowDeathEffect() {
        if (deathEffect == null)
            return;

        GameObject deathVisual = Instantiate(deathEffect, owner.transform.position, Quaternion.identity) as GameObject;


    }

}
