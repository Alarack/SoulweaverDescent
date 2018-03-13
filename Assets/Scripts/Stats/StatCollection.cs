using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BaseStatType = StatCollection.BaseStat.BaseStatType;

//[System.Serializable]
public class StatCollection {

    public enum StatModificationType {
        Additive,
        Multiplicative
    }

    private List<BaseStat> baseStats = new List<BaseStat>();
    private StatCollectionData statTemplate;



    public StatCollection() {

    }

    public StatCollection (StatCollectionData statTemplate) {
        if (statTemplate != null)
            this.statTemplate = statTemplate;
        else
            this.statTemplate = GameManager.GetDefaultStatCollection();

        InitializeDefaultStats();
    }

    public float GetStatModifiedValue(BaseStatType statType) {
        for(int i = 0; i < baseStats.Count; i++) {
            if(baseStats[i].statType == statType) {
                return baseStats[i].GetModifiedValue();
            }
        }

        return 0;
    }

   public float GetStatMaxValue(BaseStatType statType) {
        for (int i = 0; i < baseStats.Count; i++) {
            if (baseStats[i].statType == statType) {
                return baseStats[i].MaxValue;
            }
        }

        return 0;
    }

    public float GetStatMultipler(BaseStatType statType) {
        for (int i = 0; i < baseStats.Count; i++) {
            if (baseStats[i].statType == statType) {
                return baseStats[i].GetTotalMultiplier();
            }
        }

        return 1f;
    }


    public void ApplyUntrackedMod(BaseStatType statType, float value, StatModificationType modType = StatModificationType.Additive) {
        BaseStat targetStat = GetStat(statType);
        if(targetStat == null) {
            Debug.Log("Stat: " + statType + " not found");
            return;
        }
        targetStat.ModifyStat(value, modType);
    }

    public void ApplyTrackedMod(BaseStatType statType,  StatModifer mod) {
        BaseStat targetStat = GetStat(statType);
        if (targetStat == null) {
            Debug.Log("Stat: " + statType + " not found");
            return;
        }
        targetStat.ModifyStat(mod);
    }

    public void RemoveTrackedMod(BaseStatType statType, StatModifer mod) {
        BaseStat targetStat = GetStat(statType);
        if (targetStat == null) {
            Debug.Log("Stat: " + statType + " not found");
            return;
        }
        targetStat.RemoveModifier(mod);
    }


    private void InitializeDefaultStats() {
        for(int i = 0; i < statTemplate.stats.Count; i++) {
            BaseStat newStat = new BaseStat(statTemplate.stats[i].stat, statTemplate.stats[i].maxValue, statTemplate.stats[i].maxValue);
            //Debug.Log("Adding " + newStat.statType.ToString() + " with a value of " + newStat.MaxValue);
            baseStats.Add(newStat);
        }
    }

    private BaseStat GetStat(BaseStatType statType) {
        for (int i = 0; i < baseStats.Count; i++) {
            if (baseStats[i].statType == statType) {
                return baseStats[i];
            }
        }

        return null;
    }



    [System.Serializable]
    public class BaseStat {

        public enum BaseStatType {
            Health = 0,
            BaseDamage = 1,
            MoveSpeed = 2,
            CritChance = 3,
            CritMultiplier = 4,
            AttackSpeed = 5,
            RotateSpeed = 6,
            Lifetime = 7,
            ShotSpeed = 8,
            ShotSize = 9,
            ShotRotateSpeed = 10,
            ShotLifetime = 11,
            Armor = 12,
            DamageReduction = 13
        }

        public BaseStatType statType;
        public float BaseValue { get; private set; }
        public float MaxValue { get; private set; }
        private List<StatModifer> mods = new List<StatModifer>();

        public BaseStat(BaseStatType statType, float baseValue, float maxValue) {
            this.statType = statType;
            BaseValue = baseValue;
            MaxValue = maxValue;
        }

        public float GetModifiedValue() {
            float result = BaseValue + GetTotalAddativeMod();
            result *= GetTotalMultiplier();
            //Debug.Log("Getting a value of " + statType + ". Value of: " + result);
            return result;
        }

        public float GetTotalMultiplier() {
            float totalMultiplier = 1f;
            List<StatModifer> allMulipliers = new List<StatModifer>();

            for (int i = 0; i < mods.Count; i++) {
                if (mods[i].modType == StatModificationType.Multiplicative) {
                    allMulipliers.Add(mods[i]);
                    totalMultiplier += mods[i].value;
                }
            }

            if (allMulipliers.Count < 1) {
                return 1f;
            }
            //Debug.Log(totalMultiplier + " is the total multiplier on " + statType);

            return totalMultiplier;
        }

        public float GetTotalAddativeMod() {
            float result = 0;
            for (int i = 0; i < mods.Count; i++) {
                if (mods[i].modType == StatModificationType.Additive) {
                    //Debug.Log(mods[i].value + " is the value of a mod on " + statType);
                    result += mods[i].value;
                }
            }
            //Debug.Log(result + " is the result of mods on " + statType);
            return result;
        }

        public void ModifyStat(StatModifer mod) {
            mods.Add(mod);
            //Debug.Log(mod.modType + " is the mod type");

            Debug.Log(statType + " now has a value of " + GetModifiedValue());
        }

        public void RemoveModifier(StatModifer mod) {
            if (mods.Contains(mod))
                mods.Remove(mod);
        }

        public void ModifyStat(float value, StatModificationType modType = StatModificationType.Additive) {
            mods.Add(new StatModifer(value, modType));
        }

        public void ModifyMaxValue(float value, bool updateCurrent = false) {

            MaxValue += value;

            if (updateCurrent) {
                ModifyStat(value);
            }

            if (BaseValue > MaxValue)
                BaseValue = MaxValue;

            if (MaxValue <= 0f)
                MaxValue = 0f;
        }
    }


    [System.Serializable]
    public class StatModifer {
        public float value;
        public StatModificationType modType;

        public StatModifer(float value, StatModificationType modType) {
            this.value = value;
            this.modType = modType;
        }
    }


}
