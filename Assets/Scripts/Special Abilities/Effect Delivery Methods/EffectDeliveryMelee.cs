using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectDeliveryMelee : EffectDeliveryMethod {

    public string prefabName;

    private GameObject fireEffect;

    public override void Initialize(SpecialAbility parentAbility, EffectAttack parentEffect) {
        base.Initialize(parentAbility, parentEffect);
        SystemGrid.EventManager.RegisterListener(Constants.GameEvent.AnimationEvent, OnAnimationEvent);
    }


    public override void Deliver() {
        base.Deliver();
        //Debug.Log(parentAbility.source.entityName + " " + parentAbility.source.SessionID + " is delivering an attack");

        SetFireEffect();

        if (fireEffect != null)
            CreateFireEffect();

    }

    private void ConfigureMeleeAttack() {
        switch (targetingMethod) {
            case TargetingMethod.StraightLeftRight:
                if (parentAbility.source.Facing == Constants.EntityFacing.Left) {
                    shootDirection = Vector2.left;
                    effectOrigin = parentAbility.source.leftShotOrigin.position;
                    effectTransform = parentAbility.source.leftShotOrigin;
                }
                else {
                    shootDirection = Vector2.right;
                    effectOrigin = parentAbility.source.rightShotOrigin.position;
                    effectTransform = parentAbility.source.rightShotOrigin;
                }
                break;
        }
    }

    private void OnAnimationEvent(EventData data) {
        //Debug.Log("Recieving Attack");

        int id = data.GetInt("ID");
        int abilityID = data.GetInt("AbilityID");
        string attackName = data.GetString("AttackName");
        Entity owner = GameManager.GetEntityByID(id);

        if (owner != parentAbility.source) {
            //Debug.Log("ID " + id + " is not " + parentAbility.source.SessionID);
            return;
        }

        if (abilityID != parentAbility.SessionID) {
            //Debug.Log(abilityID + " is not the ID assosiated with " + parentAbility.abilityName);
            return;
        }


        if (attackName != parentEffect.animationTrigger)
            return;

        CreateMeleeAttack();

    }

    private void CreateMeleeAttack() {
        ConfigureMeleeAttack();

        GameObject loadedPrefab = Resources.Load("HitPrefabs/" + prefabName) as GameObject;

        if (loadedPrefab == null) {
            Debug.LogError("Prefab was null : " + prefabName);
            return;
        }

        //Debug.Log("Attack Created");

        GameObject hit = VisualEffectManager.CreateVisualEffect(loadedPrefab, effectOrigin, Quaternion.identity);
        MeleeHit hitScript = hit.GetComponent<MeleeHit>();

        hit.transform.SetParent(parentAbility.source.transform, true);

        hitScript.Initialize(parentEffect, layerMask, 0f, parentEffect.effectDamage);
    }

    private void SetFireEffect() {
        fireEffect = Resources.Load("Fire Effects/" + ((EffectAttack)parentEffect).fireEffectName) as GameObject;

        //VisualEffectManager.SetParticleEffectLayer(hitEffect, hit.collider.gameObject);
    }

    private void CreateFireEffect() {
        ConfigureMeleeAttack();
        GameObject effect = VisualEffectManager.CreateVisualEffect(fireEffect, effectOrigin, effectTransform.rotation);

        if (parentAbility.source.Facing == Constants.EntityFacing.Left)
            effect.GetComponentInChildren<ParticleSystem>().transform.localScale = new Vector3(-1, 1, 1);

        VisualEffectManager.DestroyVFX(effect, 1f);
    }



}
