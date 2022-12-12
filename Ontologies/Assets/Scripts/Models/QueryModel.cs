using UnityEngine.EventSystems;

public class QueryModel : ParameterModel
{
    // OnClick Event
    public override void OnPointerClick(PointerEventData eventData)
    {
        Destroy(this.gameObject);
    }
}
