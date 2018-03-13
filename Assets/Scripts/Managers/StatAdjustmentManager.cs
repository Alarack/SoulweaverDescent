using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatAdjustmentManager : MonoBehaviour {

    public static StatAdjustmentManager statAdjustmentManager;

    void Awake() {

        if (statAdjustmentManager == null) {
            statAdjustmentManager = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Update() {

    }

    //public static void AddStaticPlayerStatAdjustment(StatCollection.BaseStat.BaseStatType targetStat, float value) {
    //    ApplyUntrackedStatMod(null, GameManager.GetPlayer(), targetStat, value);
    //}

    public static void ApplyUntrackedStatMod(Entity causeOfChagne, Entity targetOfChagnge, StatCollection.BaseStat.BaseStatType stat, float value, StatCollection.StatModificationType modType = StatCollection.StatModificationType.Additive) {

        float sendableValue = 0f;

        if (stat == StatCollection.BaseStat.BaseStatType.Health) {

            //Debug.Log(targetOfChagnge);

            float armor = targetOfChagnge.stats.GetStatModifiedValue(StatCollection.BaseStat.BaseStatType.Armor);
            //Debug.Log(armor + " is the armor of " + targetOfChagnge.entityName);
            sendableValue = Mathf.Clamp(value + armor, value, 0f);

            float damageReduction = targetOfChagnge.stats.GetStatModifiedValue(StatCollection.BaseStat.BaseStatType.DamageReduction);

            float convertedDR = Mathf.Clamp(  Mathf.Abs(damageReduction - 1f), 0f, 1f );

            sendableValue *= convertedDR;

        }
        else {
            sendableValue = value;
        }


        targetOfChagnge.stats.ApplyUntrackedMod(stat, sendableValue, modType);

        statAdjustmentManager.SendStatChangeEvent(causeOfChagne, targetOfChagnge, stat, sendableValue);

        if (stat == StatCollection.BaseStat.BaseStatType.Health && value < 0f) {
            VisualEffectManager.MakeFloatingText(Mathf.Abs(sendableValue).ToString(), targetOfChagnge.transform.position);
        }

    }

    public static void ApplyTrackedStatMod(Entity causeOfChagne, Entity targetOfChange, StatCollection.BaseStat.BaseStatType stat, StatCollection.StatModifer mod) {

        targetOfChange.stats.ApplyTrackedMod(stat, mod);

        statAdjustmentManager.SendStatChangeEvent(causeOfChagne, targetOfChange, stat, mod.value);

        if (stat == StatCollection.BaseStat.BaseStatType.Health && mod.value < 0f) {
            VisualEffectManager.MakeFloatingText(Mathf.Abs(mod.value).ToString(), targetOfChange.transform.position);
        }
    }

    public static void RemoveTrackedStatMod(Entity targetOfChange, StatCollection.BaseStat.BaseStatType stat, StatCollection.StatModifer mod) {
        targetOfChange.stats.RemoveTrackedMod(stat, mod);

        Debug.Log("Removing a mod: " + stat + " value of " + mod.value);

        statAdjustmentManager.SendStatChangeEvent(null, targetOfChange, stat, mod.value);
    }


    private void SendStatChangeEvent(Entity causeOfChagne, Entity targetOfChagnge, StatCollection.BaseStat.BaseStatType stat, float value) {
        //EventData data = new EventData();

        //data.AddMonoBehaviour("Cause", causeOfChagne);
        //data.AddMonoBehaviour("Target", targetOfChagnge);
        //data.AddInt("Stat", (int)stat);
        //data.AddFloat("Value", value);

        ////Debug.Log("Event Sent: " + stat.ToString() + " :: " + value);
        //EventGrid.EventManager.SendEvent(Constants.GameEvent.StatChanged, data);


    }


}
