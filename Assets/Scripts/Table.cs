using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Table : MonoBehaviour, IPointerClickHandler
{
    private enum WaitressDestination
    {
        Top,
        Bottom,
        Left,
        Right
    }

    public Vector2 itemPlaceOffset;
    [SerializeField] private WaitressDestination playerDestination;

    public UnityEvent<Consumable> itemPlacedOnTable;
    
    public void OnPointerClick (PointerEventData eventData)
    {
        Vector3 targetPosition = transform.position;

        Debug.Log($"Clickable object at {targetPosition} was clicked. Moving player {playerDestination}.");

        switch (playerDestination)
        {
            case WaitressDestination.Top:
                targetPosition += new Vector3(0, 1f, 0);
                break;
            case WaitressDestination.Bottom:
                targetPosition += new Vector3(0, -1f, 0);
                break;
            case WaitressDestination.Left:
                targetPosition += new Vector3(-1f, 0, 0);
                break;
            case WaitressDestination.Right:
                targetPosition += new Vector3(1f, 0, 0);
                break;
        }

        WaitressMovement.Instance.MoveTo(targetPosition);

        UnityEngine.Events.UnityAction onArrived = null;

        onArrived = () =>
        {
            WaitressMovement.Instance.destinationReached.RemoveListener(onArrived);
            WaitressInteraction.Instance.PlaceItem(this);
        };

        WaitressMovement.Instance.destinationReached.AddListener(onArrived);
    }
}