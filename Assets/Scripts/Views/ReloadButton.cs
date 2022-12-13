using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReloadButton : MonoBehaviour, IPointerClickHandler
{
    // Script References
    public ProgramManager programManager;

    // OnClick Event
    public void OnPointerClick(PointerEventData eventData)
    {
        programManager.LoadDataAsync();
    }
}
