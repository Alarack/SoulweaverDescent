using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour {

    public static StatusManager statusManager;

    public List<StatusEntry> statusEntries = new List<StatusEntry>();

    private void Awake() {
        if (statusManager == null)
            statusManager = this;
        else
            Destroy(this);
    }

    public void Initialize() {

    }

    private void Update() {
        for (int i = 0; i < statusEntries.Count; i++) {
            statusEntries[i].ManagedUpdate();
        }
    }

    public static void AddStatus(Entity target, Status status, EffectStatus sourceEffect, SpecialAbility sourceAbility) {
        int count = statusManager.statusEntries.Count;
        StatusEntry targetEntry = null;

        for (int i = 0; i < count; i++) {
            if (statusManager.statusEntries[i].target == target) {
                targetEntry = statusManager.statusEntries[i];
                break;
            }
        }

        if (targetEntry != null) {
            targetEntry.AddStatus(status, sourceEffect, sourceAbility);
            return;
        }


        StatusEntry newStatus = new StatusEntry(target, new StatusContainer(status));
        statusManager.statusEntries.Add(newStatus);
    }

    public static void RemoveStatus(Entity target, Status targetStatus) {
        int count = statusManager.statusEntries.Count;
        StatusEntry targetEntry = null;

        for (int i = 0; i < count; i++) {
            if (statusManager.statusEntries[i].target == target) {
                targetEntry = statusManager.statusEntries[i];
                //statusManager.statusEntries.Remove(statusManager.statusEntries[i]);
                break;
            }
        }

        if (targetEntry != null) {
            targetEntry.RemoveStatus(targetStatus);
            if (targetEntry.GetStatusCount() < 1) {
                statusManager.statusEntries.Remove(targetEntry);
            }
        }
    }

    public static bool IsTargetAlreadyAffected(Entity target, Status status, SpecialAbility parentAbility) {
        int count = statusManager.statusEntries.Count;
        StatusEntry targetEntry = null;

        for(int i = 0; i < count; i++) {
            if (statusManager.statusEntries[i].target == target) {
                targetEntry = statusManager.statusEntries[i];
                //statusManager.statusEntries.Remove(statusManager.statusEntries[i]);
                break;
            }
        }

        if (targetEntry != null) {
            return targetEntry.IsTargetAlreadyAffected(target, status, parentAbility);
        }

        return false;

    }


    [System.Serializable]
    public class StatusEntry {
        public Entity target;
        private StatusContainer statusContainer;

        public StatusEntry(Entity target, StatusContainer statusContainer) {
            this.target = target;
            this.statusContainer = statusContainer;
        }

        public void ManagedUpdate() {
            statusContainer.ManagedUpdate();
        }

        public int GetStatusCount() {
            return statusContainer.activeStatusList.Count;
        }

        public bool IsTargetAlreadyAffected(Entity target, Status status, SpecialAbility sourceAbility) {
            if (this.target != target)
                return false;

            List<Status> existingStatus = statusContainer.GetStatusListByType(status.statusType);
            int count = existingStatus.Count;

            if (count < 1)
                return false;

            for (int i = 0; i < count; i++) {
                if (existingStatus[i].IsFromSameSource(sourceAbility)) {
                    return true;
                }
            }

            return false;
        }


        public void AddStatus(Status status, EffectStatus sourceEffect, SpecialAbility sourceAbility) {
            List<Status> existingStatus = statusContainer.GetStatusListByType(status.statusType);

            int count = existingStatus.Count;

            if (existingStatus.Count > 0) {
                for (int i = 0; i < count; i++) {
                    if (existingStatus[i].IsFromSameSource(sourceAbility)) {
                        switch (sourceEffect.stackMethod) {
                            case Constants.StatusStackingMethod.None:
                                existingStatus[i].RefreshDuration();
                                return;

                            case Constants.StatusStackingMethod.LimitedStacks:
                                if (existingStatus[i].StackCount < existingStatus[i].maxStack) {
                                    existingStatus[i].Stack();
                                }
                                else {
                                    existingStatus[i].RefreshDuration();
                                }
                                return;


                            case Constants.StatusStackingMethod.StacksWithOtherAbilities:
                                existingStatus[i].RefreshDuration();
                                return;
                        }
                    }
                }
            }

            statusContainer.AddStatus(status);
        }

        public void RemoveStatus(Status status) {
            statusContainer.RemoveStatus(status);
        }

    }


    [System.Serializable]
    public class StatusContainer {
        public List<Status> activeStatusList = new List<Status>();

        public StatusContainer(Status initialStatus) {
            AddStatus(initialStatus);
        }

        public void AddStatus(Status status) {
            activeStatusList.Add(status);
        }

        public void RemoveStatus(Status status) {
            if (activeStatusList.Contains(status)) {
                activeStatusList.Remove(status);
            }
        }

        public void ManagedUpdate() {
            for (int i = 0; i < activeStatusList.Count; i++) {
                activeStatusList[i].ManagedUpdate();
            }
        }

        public List<Status> GetStatusListByType(Constants.StatusEffectType type) {
            List<Status> results = new List<Status>();

            int count = activeStatusList.Count;

            for (int i = 0; i < count; i++) {
                if (activeStatusList[i].statusType == type) {
                    results.Add(activeStatusList[i]);
                }
            }

            return results;
        }


    }

}
