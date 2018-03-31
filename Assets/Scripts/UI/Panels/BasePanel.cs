using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BasePanel : MonoBehaviour {

    public enum PanelType {
        InGameMenu = 0,
        Inventory = 1,
        Map = 2,
    }

    [Header("Panel Type")]
    public PanelType panelType;

    [Header("Tweens")]
    public DOTweenAnimation enteranceTween;


    public bool IsOpen { get; private set; }

    protected PanelManager panelManager;


    public virtual void Initialize(PanelManager panelManager) {
        this.panelManager = panelManager;

        panelManager.allPanels.Add(this);
    }

    public void Toggle() {
        if (IsOpen)
            Close();
        else
            Open();
    }

    public virtual void Open() {

        enteranceTween.DOPlayForward();
        IsOpen = true;
    }

    public virtual void Close() {
        enteranceTween.DOPlayBackwards();
        IsOpen = false;

    }
}
