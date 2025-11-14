using System.Collections.Generic;
using UnityEngine;

public class TableHandler : MonoBehaviour
{
    public static TableHandler Instance { get; private set; }

    [SerializeField] private GameObject environment;
    [SerializeField] private Table[] tables;
    [SerializeField] private Queue<Table> _freeTables = new Queue<Table>();
    [SerializeField] private Queue<Customer> _waitingCustomers = new Queue<Customer>();
    [SerializeField] private Vector3 waitingPosition;
    
    // -------------------------------------------- Event Functions --------------------------------------------
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
    
    void Start()
    {
        if (environment == null)
        {
            Debug.LogWarning("Please add an environment (with has Tables as children)!");
        }

        tables = environment.GetComponentsInChildren<Table>();
        foreach (Table table in tables) _freeTables.Enqueue(table);
    }

    
    // -------------------------------------------- Public Functions --------------------------------------------
    public Table GetFreeTable(Customer customer)
    {
        if (_freeTables.Count > 0) return _freeTables.Dequeue();
   
        print("one more customer waiting on a table");
        _waitingCustomers.Enqueue(customer);
        return null;
    }

    public void FreeTable(Table table)
    {
        print("Table freed!");
        if (_waitingCustomers.Count > 0)
        {
            Customer customer = _waitingCustomers.Dequeue();
            customer.AcquireTable(table);
        }
        else
        {
            _freeTables.Enqueue(table);
        }
    }
}
