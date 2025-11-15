using UnityEngine;

public class WaitressInteraction : MonoBehaviour
{   
    public static WaitressInteraction Instance { get; private set; }

    [SerializeField] private Vector2 itemHoldOffset;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PickUpItem(Consumable consumable)
    {
        Debug.Log($"Picked up consumable: {consumable}");
        WaitressMovement.Instance.itemInHand = consumable;

        consumable.transform.SetParent(WaitressMovement.Instance.transform);
        consumable.transform.localPosition = (Vector3)itemHoldOffset;
        
        Barista.Instance.FreeReadyConsumableSpawnPoint();
    }

    public void PlaceItem(Table table)
    {
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
