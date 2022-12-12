using UnityEngine;
using UnityEngine.EventSystems;

public class BackButton : MonoBehaviour, IPointerClickHandler
{
    // Script References
    public InterfaceManager interfaceManager;

    // OnClick Event
    public void OnPointerClick(PointerEventData eventData)
    {
        interfaceManager.BackToQuery();
    }
}
