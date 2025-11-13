using System.Collections.Generic;
using UnityEngine;

public class TableHandler : MonoBehaviour
{
    public static TableHandler Instance { get; private set; }
    
    [SerializeField] private List<Table> tables = new List<Table>();
    [SerializeField] private Queue<Table> _freeTables = new Queue<Table>();
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
        if (tables.Count == 0)
        {
            Debug.LogError("There are no tables! Add them to the TableHandler!");
            return;
        }

        if (waitingPosition == Vector3.zero)
        {
            Debug.LogWarning("Waiting position is set to zero!");
        }
        
        foreach (Table table in tables) _freeTables.Enqueue(table);
    }

    
    // -------------------------------------------- Public Functions --------------------------------------------
    public Table GetFreeTable()
    {
        return _freeTables.Count > 0 ? _freeTables.Dequeue() : null;
    }

    public void AddTable(Table table)
    {
        _freeTables.Enqueue(table);
    }
}
