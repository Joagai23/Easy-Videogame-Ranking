using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ParameterModel : MonoBehaviour, IPointerClickHandler
{
    // Public class properties
    public string Url = string.Empty;
    public string Name = string.Empty;
    public string Type = string.Empty;

    // UI References
    public Text imageText;

    // Script References
    public InterfaceManager interfaceManager;

    // OnClick Event
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        interfaceManager.AddQueryProperty(this);
    }

    // Assign class properties
    public void SetData(string url, string name, string type)
    {
        Url = url;
        Name = name;
        Type = type;

        SetImageText();
    }

    // Update UI
    public void SetImageText()
    {
        imageText = gameObject.GetComponentInChildren<Text>();
        imageText.text = Name;
    }
}
