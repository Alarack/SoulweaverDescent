using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpecialAbility {

    public Constants.SpecialAbilityType abilityType;
    public Constants.SpecialAbilityActivationMethod activationMethod;
    public Constants.SpecialAbilityLimitations abilityLimitations;
    public string abilityName;
    [System.NonSerialized]
    public Sprite abilityIcon;
    public Entity source;
    public float useDuration;
    public bool overrideOtherAbilities;
    public float procChance;

    public List<Entity> targets = new List<Entity>();

    [System.NonSerialized]
    protected List<SpecialAbility> sequencedAbilities = new List<SpecialAbility>();

    public SpecialAbility ParentAbility { get; set; }
    public float sequenceWindow = 0.5f;

    private Timer sequenceTimer;
    private int sequenceIndex = 0;

    public bool InUse { get; protected set; }
    public string animationTrigger;

    protected Timer useTimer;
    public int UseCount { get; protected set; }
    public SpecialAbilityRecovery Recovery { get { return recoveryMethod; } }

    protected SpecialAbilityRecovery recoveryMethod;

    public List<Effect> effects = new List<Effect>();

    private SpecialAbilityData abilitydata;
    public int SessionID { get; protected set; }

    protected int animationHash;

    public virtual void Initialize(Entity source, SpecialAbilityData abilitydata, List<SpecialAbilityData> sequencedAbilities = null) {
        this.source = source;
        this.abilitydata = abilitydata;
        SessionID = IDFactory.GenerateAbilityID();
        SetUpAbility();

        if (sequencedAbilities != null && sequencedAbilities.Count > 0) {
            for (int i = 0; i < sequencedAbilities.Count; i++) {
                SpecialAbility sAbility = new SpecialAbility();
                this.sequencedAbilities.Add(sAbility);
                sAbility.ParentAbility = this;
                sAbility.Initialize(source, sequencedAbilities[i]);
                //Debug.Log(sequencedAbilities[i].abilityName + " is a sequence of " + abilitydata.abilityName);
            } 

            sequenceTimer = new Timer(sequenceWindow, false, ResetSequenceIndex);
        }

        animationHash = Animator.StringToHash(animationTrigger);

        if (activationMethod == Constants.SpecialAbilityActivationMethod.Passive) {
            Activate();
        }


    }

    private void SetUpAbility() {
        abilityName = abilitydata.abilityName;
        abilityType = abilitydata.abilityType;
        activationMethod = abilitydata.activationMethod;
        procChance = abilitydata.procChance;
        animationTrigger = abilitydata.animationTrigger;
        effects = abilitydata.GetAllEffects();
        recoveryMethod = abilitydata.GetRecoveryMechanic();
        sequenceWindow = abilitydata.sequenceWindow;
        useDuration = abilitydata.useDuration;
        useTimer = new Timer(useDuration, true, PopAbilityUseTimer);
        overrideOtherAbilities = abilitydata.overrideOtherAbilities;
        abilityIcon = abilitydata.abilityIcon;
        abilityLimitations = abilitydata.abilityLimitations;

        if (recoveryMethod != null) {
            recoveryMethod.Initialize(this);
        }

        for (int i = 0; i < effects.Count; i++) {
            effects[i].Initialize(this);
        }

        RegisterListeners();

        SetupRiders();

        //Debug.Log(abilityName + " has been initialized");
    }



    public void RegisterListeners() {
        //switch (activationMethod) {
        //    case Constants.SpecialAbilityActivationMethod.DamageDealt:
        //        EventGrid.EventManager.RegisterListener(Constants.GameEvent.StatChanged, OnDamageDealt);
        //        break;

        //    case Constants.SpecialAbilityActivationMethod.DamageTaken:
        //        EventGrid.EventManager.RegisterListener(Constants.GameEvent.StatChanged, OnDamageTaken);
        //        break;

        //    case Constants.SpecialAbilityActivationMethod.EffectApplied:
        //        EventGrid.EventManager.RegisterListener(Constants.GameEvent.EffectApplied, OnEffectApplied);
        //        break;

        //    case Constants.SpecialAbilityActivationMethod.EntityKilled:
        //        EventGrid.EventManager.RegisterListener(Constants.GameEvent.EntityDied, OnEntityKilled);
        //        break;
        //}
    }



    #region EVENTS

    private bool ProcRoll() {
        float roll = Random.Range(0f, 1f);
        return roll <= procChance;
    }


    //private void OnDamageDealt(EventData data) {
    //    StatCollection.BaseStat.BaseStatType stat = (StatCollection.BaseStat.BaseStatType)data.GetInt("Stat");
    //    Entity target = data.GetMonoBehaviour("Target") as Entity;
    //    Entity cause = data.GetMonoBehaviour("Cause") as Entity;
    //    float value = data.GetFloat("Value");

    //    //Debug.Log(target.gameObject + " has been damaged by " + cause.gameObject);

    //    if (cause != source)
    //        return;

    //    if (stat != StatCollection.BaseStat.BaseStatType.Health)
    //        return;

    //    if (value > 0)
    //        return;


    //    //Debug.Log(abilityName + " is activating on damage dealt");
    //    AddTarget(target);


    //    Activate();
    //}

    //private void OnEntityKilled(EventData data) {
    //    Entity cause = data.GetMonoBehaviour("Cause") as Entity;
    //    Entity target = data.GetMonoBehaviour("Target") as Entity;

    //    if (cause != source)
    //        return;


    //    AddTarget(target);
    //    Activate();

    //}

    //private void OnDamageTaken(EventData data) {
    //    StatCollection.BaseStat.BaseStatType stat = (StatCollection.BaseStat.BaseStatType)data.GetInt("Stat");
    //    //Entity target = data.GetMonoBehaviour("Target") as Entity;
    //    Entity cause = data.GetMonoBehaviour("Cause") as Entity;
    //    Entity target = data.GetMonoBehaviour("Target") as Entity;
    //    float value = data.GetFloat("Value");

    //    if (cause == source)
    //        return;

    //    if (target != source)
    //        return;

    //    if (stat != StatCollection.BaseStat.BaseStatType.Health)
    //        return;

    //    if (value > 0)
    //        return;


    //    //Debug.Log("Activating an on-damage-taken ability");
    //    Activate();

    //}

    //private void OnEffectApplied(EventData data) {
    //    Entity cause = data.GetMonoBehaviour("Cause") as Entity;
    //    //Entity target = data.GetMonoBehaviour("Target") as Entity;

    //    if (cause != source)
    //        return;

    //    Activate();
    //}


    #endregion



    public void IncrementSequenceIndex() {
        sequenceIndex++;
        if (sequenceIndex >= sequencedAbilities.Count) {
            sequenceIndex = 0;
        }

        //Debug.Log(sequenceIndex + " is the sequence index");
    }

    private void ResetSequenceIndex() {
        //Debug.Log("resetting sequence index");
        sequenceIndex = 0;
    }

    public Effect GetEffectByName(string effectName) {
        if (string.IsNullOrEmpty(effectName)) {
            //Debug.Log("String was empty");
            return null;
        }

        for (int i = 0; i < effects.Count; i++) {
            //Debug.Log("Seeking " + effectName);

            if (effects[i].effectName == effectName) {
                return effects[i];
            }
        }

        return null;
    }


    public bool CheckLimitations() {
        switch (abilityLimitations) {
            case Constants.SpecialAbilityLimitations.None:
                return true;

            case Constants.SpecialAbilityLimitations.Grounded:
                return ((MovingEntity)source).Controller.collisionInfo.below == true;

            case Constants.SpecialAbilityLimitations.Arial:
                return ((MovingEntity)source).Controller.collisionInfo.below == false 
                    && ((MovingEntity)source).Controller.collisionInfo.left == false
                    && ((MovingEntity)source).Controller.collisionInfo.right == false;


            default:

                return true;
        }
    }
    public virtual bool Activate() {
        if (CheckLimitations() == false) {
            Debug.Log(abilityName + " could not activate due to its limitations");
            return false;
        }

        if (PlayAnimation() == false) {
            Debug.Log(abilityName + " could not activate because its animation was already playing");
            return false;
        }

        if (recoveryMethod != null && !recoveryMethod.Ready) {
             Debug.Log(abilityName + " is not ready");
            return false;
        }

        if (procChance < 1f && !ProcRoll())
            return false;

        //Debug.Log(abilityName + " has been activated");

        if (sequencedAbilities.Count < 1) {
            for (int i = 0; i < effects.Count; i++) {
                //targets.Clear();
                effects[i].Activate();
            }
            //Debug.Log(abilityName + " has no sequence and is activating");
            FinishActivation();
            return true;
        }

        targets.Clear();

        //Debug.Log(sequencedAbilities[sequenceIndex].abilityName + " is activating");

        if (sequencedAbilities[sequenceIndex].Activate()) {
            sequenceTimer.ResetTimer();
        }

        FinishActivation();

        
        return true;
    }

    public void AddTarget(Entity target) {

        targets.AddUnique(target);

        //if (!targets.Contains(target)) {
        //    targets.Add(target);
        //}

    }

    private void FinishActivation() {
        if (recoveryMethod != null) {
            recoveryMethod.Trigger();
        }

        if(useDuration > 0f)
            InUse = true;

        targets.Clear();

    }

    public virtual void ManagedUpdate() {

        if (recoveryMethod != null)
            recoveryMethod.ManagedUpdate();

        for (int i = 0; i < effects.Count; i++) {
            effects[i].ManagedUpdate();
        }

        UpdateInUseTimer();

        for (int i = 0; i < sequencedAbilities.Count; i++) {
            sequencedAbilities[i].ManagedUpdate();
        }

        if(sequenceTimer != null) {
            sequenceTimer.UpdateClock();
        }

    }

    public virtual void ResetUseCount() {
        UseCount = 0;
    }

    protected void UpdateInUseTimer() {
        if (InUse) {
            useTimer.UpdateClock();
            //Debug.Log("Updating " + abilityName + " use clock");
        }
    }

    protected bool PlayAnimation() {
        if (string.IsNullOrEmpty(animationTrigger) == false) {
            if (source.MyAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationTrigger) == true) {
                //Debug.Log("Already Playing Animation");
                return false;
            }
            else {
                source.MyAnimator.SetTrigger(animationHash);
                //Debug.Log("Setting " + animationTrigger + " trigger");
                return true;
            }
        }

        return true;
    }

    protected virtual void PopAbilityUseTimer() {
        InUse = false;

        //Debug.Log("Popping usetimer for " + abilityName + " Use status: " + InUse);

        if (ParentAbility != null) {
            ParentAbility.IncrementSequenceIndex();
        }

    }

    protected void SetupRiders() {
        for (int i = 0; i < effects.Count; i++) {
            effects[i].SetUpRiders();
        }
    }


}
