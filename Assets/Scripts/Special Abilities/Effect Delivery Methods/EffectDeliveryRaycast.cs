using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectDeliveryRaycast : EffectDeliveryMethod {


    public override void Deliver() {
        base.Deliver();

        //EffectAttack attackEffect = parentEffect as EffectAttack;

        //Debug.Log("Deliverying a raycast");

        if (parentEffect.penetrate) {
            TryShootMultiRay();
        }
        else {
            TryShootRay();
        }


    }

    private void ConfigureRay() {
        if (range == 0)
            range = Mathf.Infinity;

        switch (targetingMethod) {
            case TargetingMethod.StraightLeftRight:
                if (parentAbility.source.Facing == Constants.EntityFacing.Left) {
                    shootDirection = Vector2.left;
                    effectOrigin = parentAbility.source.leftShotOrigin.position;
                }
                else {
                    shootDirection = Vector2.right;
                    effectOrigin = parentAbility.source.rightShotOrigin.position;
                }
                break;
        }

    }

    private void TryShootRay() {
        ConfigureRay();

        RaycastHit2D hit = Physics2D.Raycast(effectOrigin, shootDirection, range, layerMask);

        if (hit.collider != null) {
            parentEffect.Apply(hit.collider.gameObject);
            CreateHitEffects(hit);
        }

    }

    private void TryShootMultiRay() {
        ConfigureRay();

        //Debug.Log("Shooting");

        RaycastHit2D[] hits = Physics2D.RaycastAll(effectOrigin, shootDirection, range, layerMask);
        int count;

        if (parentEffect.numPenetrations > 0) {
            count = Mathf.Min(hits.Length, (parentEffect.numPenetrations));
        }
        else {
            count = hits.Length;
        }

        //Debug.Log(count + " hits");

        for (int i = 0; i < count; i++) {
            RaycastHit2D hit = hits[i];

            if (hit.collider != null) {
                parentEffect.Apply(hit.collider.gameObject);
                CreateHitEffects(hit);
            }
        }
    }




    private void CreateHitEffects(RaycastHit2D hit) {

        Vector2 rayDir = Vector2.Reflect((hit.point - effectOrigin).normalized, hit.normal);

        //Quaternion impactRotation = TargetingUtilities.CalculateImpactRotation(rayDir);

        GameObject hitPrefab = Resources.Load(((EffectAttack)parentEffect).impactEffectName) as GameObject;
        GameObject hitEffect = VisualEffectManager.CreateVisualEffect(hitPrefab, hit.point, Quaternion.identity);


        VisualEffectManager.SetParticleEffectLayer(hitEffect, hit.collider.gameObject);

        //ParticleSystem[] ps = hitEffect.GetComponentsInChildren<ParticleSystem>();
        //SpriteRenderer hitSprite = hit.collider.gameObject.GetComponentInChildren<SpriteRenderer>();

        //for (int i = 0; i < ps.Length; i++) {
        //    ps[i].GetComponent<ParticleSystemRenderer>().sortingOrder = hitSprite.sortingOrder;
        //}

    }



}
