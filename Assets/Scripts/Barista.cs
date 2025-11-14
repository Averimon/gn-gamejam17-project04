using UnityEngine;
using UnityEngine.EventSystems;

public class Barista : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Canvas orderCanvas;
    public void OnPointerClick (PointerEventData eventData)
    {
        ToggleOrderCanvas();
    }

    private void ToggleOrderCanvas()
    {
        orderCanvas.enabled = !orderCanvas.enabled;
    }
}
