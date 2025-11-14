using UnityEngine;
using UnityEngine.EventSystems;

public class Consumable : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string consumableName;
    [SerializeField] private ConsumableDifficulty difficulty;
    public void OnPointerClick(PointerEventData eventData)
    {
        WaitressInteraction.Instance.PickUp(this);
    }
}
