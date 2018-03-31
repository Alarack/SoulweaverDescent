using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour {



    public List<BasePanel> allPanels = new List<BasePanel>();


    private void Awake() {
        BasePanel[] panels = GetComponentsInChildren<BasePanel>();

        int count = panels.Length;

        for (int i = 0; i < count; i++) {
            panels[i].Initialize(this);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            TogglePanel(BasePanel.PanelType.Inventory);
        }

        if (Input.GetKeyDown(KeyCode.M)) {
            TogglePanel(BasePanel.PanelType.Map);
        }
    }

    public void OpenPanel(string panelName) {
        switch (panelName) {
            case "Inventory":
                GetPanelByType(BasePanel.PanelType.Inventory).Open();
                break;

            case "InGameMenu":
                GetPanelByType(BasePanel.PanelType.InGameMenu).Open();
                break;
        }
    }

    public void OpenPanel(BasePanel.PanelType panelType) {
        GetPanelByType(panelType).Open();
    }

    public void TogglePanel(BasePanel.PanelType panelType) {
        GetPanelByType(panelType).Toggle();
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

    public bool IsPanelOpen(BasePanel.PanelType panelType) {
        return GetPanelByType(panelType).IsOpen;
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
