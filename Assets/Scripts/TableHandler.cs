using System.Collections.Generic;
using UnityEngine;

public class TableHandler : MonoBehaviour
{
    public static TableHandler Instance { get; private set; }

    [SerializeField] private GameObject environment;
    [SerializeField] private TableUnit[] tables;
    [SerializeField] private Vector3 waitingPosition;
    
    private Queue<TableUnit> _freeTables = new Queue<TableUnit>();
    private Queue<Customer> _waitingCustomers = new Queue<Customer>();
    
    // -------------------------------------------- Event Functions --------------------------------------------
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
    }
    
    void Start()
    {
        if (environment == null) Debug.LogWarning("Please add an environment (with has Tables as children)!");

        tables = environment.GetComponentsInChildren<TableUnit>();
        foreach (TableUnit table in tables) _freeTables.Enqueue(table);
    }

    
    // -------------------------------------------- Public Functions --------------------------------------------
    public TableUnit GetFreeTable(Customer customer)
    {
        if (_freeTables.Count > 0) return _freeTables.Dequeue();
   
        print("one more customer waiting on a table");
        _waitingCustomers.Enqueue(customer);
        return null;
    }

    public void FreeTable(TableUnit table)
    {
        if (table is null) return;
        table.FreeTableUnit();

        print("Table freed!");
        if (_waitingCustomers.Count > 0)
        {
            Customer customer = _waitingCustomers.Dequeue();
            if (customer != null)
            {
                customer.AcquireTable(table);
            }
            else
            {
                _freeTables.Enqueue(table);
            }
        }
        else
        {
            _freeTables.Enqueue(table);
        }
    }
}
