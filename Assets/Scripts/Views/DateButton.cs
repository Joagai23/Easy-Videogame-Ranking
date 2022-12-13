using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DateButton : MonoBehaviour, IPointerClickHandler
{
    // UI References
    public Text minText;
    public Text maxText;

    // Script References
    public InterfaceManager interfaceManager;

    // OnClick Event
    public void OnPointerClick(PointerEventData eventData)
    {
        ProcessData(minText, true);
        ProcessData(maxText, false);
    }

    // Add Query Object depending on the type
    private void ProcessData(Text text, bool isMin)
    {
        int number;
        try
        {
            number = int.Parse(text.text);
        }
        catch (Exception) 
        {
            number = 0;
        }

        if(number != 0)
        {
            if(isMin)
            {
                interfaceManager.AddQueryProperty(new ParameterModel() { Name = "> " + number, Url = number.ToString(), Type = "DateMin" });
            }
            else
            {
                interfaceManager.AddQueryProperty(new ParameterModel() { Name = "< " + number, Url = number.ToString(), Type = "DateMax" });
            }
        }
    }
}
