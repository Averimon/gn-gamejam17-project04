using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Consumable : MonoBehaviour, IPointerClickHandler
{
    public int type;
    public ConsumableDifficulty difficulty;

    [SerializeField] private string consumableName;
    
    public Vector3 maskStartPos;
    public Vector3 maskEndPos;
    public bool isPreview = true;

    private bool isWaiting = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isPreview)
        {
            Barista.Instance.ItemOrdered(this);
            return;
        }

        if (isWaiting) return;
        isWaiting = true;

        WaitressMovement.Instance.MoveTo(transform.position);
        StartCoroutine(WaitForPlayerMove());
    }

    private IEnumerator WaitForPlayerMove()
    {
        bool reached = false;
        UnityEngine.Events.UnityAction action = () => { reached = true; };

        WaitressMovement.Instance.destinationReached.AddListener(action);

        yield return new WaitUntil(() => reached);

        WaitressMovement.Instance.destinationReached.RemoveListener(action);

        if (this != null && gameObject.activeInHierarchy)
        {
            WaitressInteraction.Instance.PickUpItem(this);
        }

        isWaiting = false;
    }
}
