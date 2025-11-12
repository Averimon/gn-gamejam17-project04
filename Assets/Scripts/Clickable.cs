using UnityEngine;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Vector2 playerLocation;
    public void OnPointerClick (PointerEventData eventData)
    {
        PlayerMovement.Instance.OnClick(playerLocation);
    }
}
