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
        if (Input.GetKeyDown(KeyCode.I)) {
            OpenPanel(BasePanel.PanelType.Inventory);
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
