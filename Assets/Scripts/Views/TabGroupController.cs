using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroupController : MonoBehaviour
{
    // Gameobject References
    public GameObject choosingAreaColor;
    public GameObject propertyInstance;

    // Script References
    public List<TabController> tabs;
    public TabController currentTab;
    public InterfaceManager interfaceManager;

    // Add new Tab to list
    public void Subscribe(TabController tab)
    {
        if(tabs == null)
        {
            tabs = new List<TabController>();
        }

        tabs.Add(tab);
    }

    // OnClick Event
    public void OnTabSelected(TabController tab)
    {
        currentTab = tab;
        choosingAreaColor.GetComponent<Image>().color = new Color(currentTab.backgroud.r, currentTab.backgroud.g, currentTab.backgroud.b, 1.0f);
        propertyInstance.GetComponentInChildren<Image>().color = new Color(currentTab.foreground.r, currentTab.foreground.g, currentTab.foreground.b, 1.0f);

        // Hide all other tabs
        interfaceManager.HideContent();

        // Unhide current tab
        interfaceManager.UnhideContent(tab.gameObject.name);
    }
}
