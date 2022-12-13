using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Color))]
public class TabController : MonoBehaviour, IPointerClickHandler
{
    // UI References
    public Color backgroud;
    public Color foreground;

    // Script Reference
    public TabGroupController tabGroup;

    // OnClick Event
    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }

    // Subscribe to TabGroup as soon as created
    void Start()
    {
        tabGroup.Subscribe(this);
    }
}
