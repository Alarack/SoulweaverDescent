using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectDeliveryProjectile : EffectDeliveryMethod {

    public Projectile.ProjectileType projectileType;
    public string prefabName;
    public float error;


    //public bool kickBack;
    //public float kickStrength;

    

    public override void Deliver() {
        base.Deliver();

        CreateProjectile();
    }

    private void ConfigureProjectile() {
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

            case TargetingMethod.Up:
                shootDirection = Vector2.up;
                effectOrigin = parentAbility.source.topShotPoint.position;
                effectTransform = parentAbility.source.topShotPoint;
                break;
        }
    }

    private void CreateProjectile() {
        ConfigureProjectile();
        CreateFireEffect();

        GameObject loadedPrefab = Resources.Load("Projectiles/" + prefabName) as GameObject;

        if(loadedPrefab == null) {
            Debug.LogError("Prefab was null");
            return;
        }

        //Quaternion rot = Quaternion.FromToRotation(effectOrigin, shootDirection);//Quaternion.LookRotation(shootDirection, Vector3.forward);

        GameObject shot = VisualEffectManager.CreateVisualEffect(loadedPrefab, effectOrigin, effectTransform.rotation);
        Projectile shotScript = shot.GetComponent<Projectile>();

        if (error != 0f) {
            float e = Random.Range(-error, error);
            shot.transform.rotation = effectTransform.rotation * Quaternion.Euler(effectTransform.rotation.x, effectTransform.rotation.y, e);

            //if (shotScript.ProjectileMovement.lobbed) {
            //    shotScript.ProjectileMovement.angle += e;
            //}

        }

        shotScript.Initialize(parentEffect, layerMask, 0f, parentEffect.effectDamage);

        //if (kickBack) {
        //    parentAbility.source.GetComponent<Rigidbody2D>().AddForce(-shotPos.up * kickStrength);
        //}

    }

    private void CreateFireEffect() {

        GameObject firePrefab = Resources.Load(((EffectAttack)parentEffect).fireEffectName) as GameObject;
        if(firePrefab == null) {
            //Debug.LogError("Fire Prefab Null");
            Debug.LogWarning(parentAbility.abilityName + " is firing with no fire prefab");
            return;
        }

        GameObject fireEffect = VisualEffectManager.CreateVisualEffect(firePrefab, effectOrigin, Quaternion.identity);

    }

}
