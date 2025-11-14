using UnityEngine;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour, IPointerClickHandler
{
    private enum PlayerDestination
    {
        Top,
        Bottom,
        Left,
        Right
    }
    
    [SerializeField] private PlayerDestination playerDestination;
    public void OnPointerClick (PointerEventData eventData)
    {
        Vector3 targetPosition = transform.position;

        Debug.Log($"Clickable object at {targetPosition} was clicked. Moving player {playerDestination}.");

        switch (playerDestination)
        {
            case PlayerDestination.Top:
                targetPosition += new Vector3(0, 1.5f, 0);
                break;
            case PlayerDestination.Bottom:
                targetPosition += new Vector3(0, -1.5f, 0);
                break;
            case PlayerDestination.Left:
                targetPosition += new Vector3(-1.5f, 0, 0);
                break;
            case PlayerDestination.Right:
                targetPosition += new Vector3(1.5f, 0, 0);
                break;
        }

        WaitressMovement.Instance.target = targetPosition;
    }
}