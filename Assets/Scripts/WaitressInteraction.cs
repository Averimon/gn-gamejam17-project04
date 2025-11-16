using UnityEngine;

public class WaitressInteraction : MonoBehaviour
{   
    public static WaitressInteraction Instance { get; private set; }

    [SerializeField] private Transform itemHoldOffset;

    private Animator animator;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        animator = GetComponent<Animator>();
    }

    public void PickUpItem(Consumable consumable)
    {
        animator.SetBool("isServing", true);
        Debug.Log($"Picked up consumable: {consumable}");
        WaitressMovement.Instance.itemInHand = consumable;

        consumable.transform.SetParent(WaitressMovement.Instance.transform);
        consumable.transform.localPosition = itemHoldOffset.localPosition;
        
        Barista.Instance.FreeReadyConsumableSpawnPoint();
    }

    public void PlaceItem(Table table)
    {
        animator.SetBool("isServing", false);
        Consumable item = WaitressMovement.Instance.itemInHand;
        if (item == null)
        {
            Debug.LogWarning("No item in hand to place.");
            return;
        }

        Debug.Log($"Placing item: {item} at position: {table.transform.position}");
        item.transform.SetParent(table.transform);
        item.transform.position = table.transform.position + (Vector3)table.itemPlaceOffset;

        WaitressMovement.Instance.itemInHand = null;
        table.itemPlacedOnTable.Invoke(item);
    }
}
