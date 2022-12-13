using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExitButton : MonoBehaviour, IPointerClickHandler
{
    // OnClick Event
    public void OnPointerClick(PointerEventData eventData)
    {
        Application.Quit();
    }
}
