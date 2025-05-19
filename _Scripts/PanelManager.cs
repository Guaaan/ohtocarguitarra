using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [System.Serializable]
    public class Panel
    {
        public GameObject panelObject;
        public bool isOpen;
        public bool openOnEsc = false;
    }

    [SerializeField] private Panel[] panels;

    private void Start()
    {
        InitializePanels();
    }

    private void InitializePanels()
    {
        // Cerrar todos los paneles al inicio excepto los marcados como abiertos
        bool foundOpenPanel = false;

        foreach (Panel panel in panels)
        {
            if (!foundOpenPanel && panel.isOpen)
            {
                foundOpenPanel = true;
                SetPanelState(panel, true);
            }
            else
            {
                SetPanelState(panel, false);
            }
        }
    }

    public void OpenPanel(int panelIndex)
    {
        if (panelIndex < 0 || panelIndex >= panels.Length)
        {
            Debug.LogError("Invalid panel index!");
            return;
        }

        // Cerrar todos los paneles primero
        CloseAllPanels();

        // Abrir el panel seleccionado
        SetPanelState(panels[panelIndex], true);
    }

    public void TogglePanel(int panelIndex)
    {
        if (panelIndex < 0 || panelIndex >= panels.Length)
        {
            Debug.LogError("Invalid panel index!");
            return;
        }

        bool newState = !panels[panelIndex].isOpen;

        if (newState)
        {
            CloseAllPanels();
        }

        SetPanelState(panels[panelIndex], newState);
    }

    public void ClosePanel(int panelIndex)
    {
        
        SetPanelState(panels[panelIndex], false);
        
    }
    public void CloseAllPanels()
    {
        foreach (Panel panel in panels)
        {
            SetPanelState(panel, false);
        }
    }

    private void SetPanelState(Panel panel, bool state)
    {
        panel.isOpen = state;
        panel.panelObject.SetActive(state);
    }

    // Método para debuggear estados
    public void PrintPanelStates()
    {
        foreach (Panel panel in panels)
        {
            Debug.Log($"{panel.panelObject.name}: {(panel.isOpen ? "Open" : "Closed")}");
        }
    }
}