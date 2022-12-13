using UnityEngine;
using UnityEngine.EventSystems;

public class SearchButton : MonoBehaviour, IPointerClickHandler
{
    // Script Reference
    public ProgramManager programManager;

    // OnClick Event
    public void OnPointerClick(PointerEventData eventData)
    {
        programManager.PerformSearchAsync();
    }
}
