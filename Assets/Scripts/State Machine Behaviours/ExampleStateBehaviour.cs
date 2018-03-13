using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleStateBehaviour : StateMachineBehaviour {

    public enum AnimatorStateType {
        Attack,
        Movement,
    }

    public AnimatorStateType stateType;
    public string stateName;
    [Range(0f, 1f)]
    public float hitBoxDeliveryTime;

    private bool hitBoxDelivered;

    private Entity owner;

    //private List<SpecialAbility> assosiatedAbilities = new List<SpecialAbility>();

    public SpecialAbility CurrentAbility { get; set; }

    public void Initialize(Entity owner) {
        this.owner = owner;
    }

    //public void RegisterAbility(SpecialAbility ability) {
    //    assosiatedAbilities.Add(ability);
    //}

    //public int GetAbilityID(SpecialAbility ability) {
    //    int count = assosiatedAbilities.Count;

    //    for(int i =0; i < count; i++) {
    //        if(assosiatedAbilities[i] == ability) {
    //            return assosiatedAbilities[i].SessionID;
    //        }
    //    }

    //    return -1;
    //}


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //Debug.Log("Entering " + stateName);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        if (stateType != AnimatorStateType.Attack)
            return;


        if(Mathf.Repeat(stateInfo.normalizedTime, 1f) > hitBoxDeliveryTime && hitBoxDelivered == false) {
            SendHitBoxEvent();
            hitBoxDelivered = true;
        }

        //Debug.Log(stateName + Mathf.Repeat( stateInfo.normalizedTime, 1f) + " is the time");
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateExit(animator, stateInfo, layerIndex);

        hitBoxDelivered = false;
        CurrentAbility = null;
        //Debug.Log("Exiting " + stateName);
    }



    private void SendHitBoxEvent() {

        EventData data = new EventData();
        data.AddString("AttackName", stateName);
        //data.AddMonoBehaviour("Entity", owner);
        data.AddInt("ID", owner.SessionID);

        if(CurrentAbility != null) {
            data.AddInt("AbilityID", CurrentAbility.SessionID);
        }

        //Debug.Log(owner.entityName + " " + owner.SessionID + " is sending an animation event");

        EventGrid.EventManager.SendEvent(Constants.GameEvent.AnimationEvent, data);
    }

}
