using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Consumable : MonoBehaviour, IPointerClickHandler
{
    public int type;
    public ConsumableDifficulty difficulty;

    [SerializeField] private string consumableName;
    
    /*private Vector3 _maskStartPos;
    public Vector3 MaskStartPos => _maskStartPos;
    private Vector3 _maskEndPos;
    public Vector3 MaskEndPos => _maskEndPos;*/
    
    public bool isPreview = true;

    private bool isWaiting = false;

/*
    void Start()
    {
        var mask = GetComponentInChildren<SpriteMask>();
        _maskEndPos = mask.transform.localPosition;
        float maskScaleOffset = mask.transform.localScale.y;
        _maskStartPos = new Vector3(_maskEndPos.x, _maskEndPos.y - maskScaleOffset, _maskEndPos.z);
    }*/

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
