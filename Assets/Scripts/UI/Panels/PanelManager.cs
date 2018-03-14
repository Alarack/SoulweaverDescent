using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour {



    public List<BasePanel> allPanels = new List<BasePanel>();


    private void Start() {
        BasePanel[] panels = GetComponentsInChildren<BasePanel>();

        int count = panels.Length;

        for (int i = 0; i < count; i++) {
            panels[i].Initialize(this);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.D)) {
            GetPanelByType(BasePanel.PanelType.DeckView).Toggle();
        }
    }

    public void OpenPanel(string panelName) {
        switch (panelName) {
            case "DrawPanel":
                GetPanelByType(BasePanel.PanelType.DrawPanel).Open();
                break;

            case "OptionsPanel":
                GetPanelByType(BasePanel.PanelType.InGameMenu).Open();
                break;

            case "EnemyHand":
                GetPanelByType(BasePanel.PanelType.EnemyHand).Open();
                break;
        }
    }


    public BasePanel GetPanelByType(BasePanel.PanelType type) {
        int count = allPanels.Count;

        for (int i = 0; i < count; i++) {
            if (allPanels[i].panelType == type) {
                return allPanels[i];
            }

        }

        return null;
    }


    public bool IsPanelOpen() {
        bool result = false;

        int count = allPanels.Count;

        for (int i = 0; i < count; i++) {
            if (allPanels[i].IsOpen)
                return true;
        }
        return result;
    }

    public List<BasePanel> GetOpenPanels() {
        if (IsPanelOpen() == false)
            return null;

        List<BasePanel> results = new List<BasePanel>();

        int count = allPanels.Count;

        for (int i = 0; i < count; i++) {
            if (allPanels[i].IsOpen)
                results.Add(allPanels[i]);
        }

        return results;
    }

}
