using System.Collections.Generic;
using UnityEngine;

public class TableUnit : MonoBehaviour
{
    public Consumable itemOnTable;
    [SerializeField] private GameObject chair1;
    [SerializeField] private GameObject chair2;

    private List<Customer> _customers = new List<Customer>();

    void Awake()
    {
        Table table = GetComponentInChildren<Table>();
        if (table != null)
        {
            table.itemPlacedOnTable.AddListener(HandleItemPlaced);
        }
    }

    void Start()
    {
        if (chair1 == null)
        {
            Debug.LogError("please add a GameObject as seat1!");
            return;
        }
        if (chair2 == null)
        {
            Debug.LogError("please add a GameObject as seat2!");
            return;
        }
    }
    
    private void HandleItemPlaced(Consumable item)
    {
        itemOnTable = item;
        foreach (Customer customer in _customers)
        {
            customer.ReceiveItem(itemOnTable);
        }
    }

    public Vector3 GetSeat(Customer customer)
    {
        _customers.Add(customer);
        
        if (Random.value >= 0.5) return chair1.transform.position;
        else return chair2.transform.position;
    }

    public void FreeTableUnit()
    {
        _customers.Clear();
        if (itemOnTable != null)
        {
            Destroy(itemOnTable.gameObject);
            itemOnTable = null;
        }
    }

    public void GetTableOrders()
    {
        foreach (Customer customer in _customers)
        {
            customer.GiveOrder();
        }
    }
}
