using System;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    private enum CustomerStates
    {
        Waiting,
        Walking,
        Ordering,
        Ordered,
        Eating,
    }
    
    [Header("Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxWaitTime = 1f;
    [SerializeField] private Vector3 spawnPosition;
    
    [Header("Debug")]
    [SerializeField] private CustomerStates states = CustomerStates.Waiting;
    [SerializeField] private Consumable craving;
    [SerializeField] private Vector3 desiredPosition;
    [SerializeField] private Vector3 direction;

    private SpriteRenderer _spriteRenderer;
    private GameObject _order;
    private TableUnit _table;
    private float _timeWaited = 0f;
    private bool _happy = false;
    private bool _served = false;

    // -------------------------------------------- Event Functions --------------------------------------------
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _order = transform.GetChild(0).gameObject;
    }
    
    private void Start()
    {
        spawnPosition = transform.position;
        AcquireTable(TableHandler.Instance.GetFreeTable(this));
    }

    private void Update()
    {
        _timeWaited += Time.deltaTime;
        
        switch (states)
        {
            case CustomerStates.Waiting:
                Wait();
                break;
            case CustomerStates.Walking:
                Move();
                break;
            case CustomerStates.Ordering:
                OrderingItem();
                break;
            case CustomerStates.Ordered:
                OrderItem();
                break;
            case CustomerStates.Eating:
                Eating();
                break;
        }
    }

    // -------------------------------------------- Public Functions --------------------------------------------
    public void ReceiveItem(Consumable received)
    {
        _served = true;
        // TODO: randomize craving type
        _happy = received.type == 0;

        ChangeState(CustomerStates.Eating);
    }

    public void SetTexture(Sprite texture)
    {
        if (_spriteRenderer != null)
        {
            _spriteRenderer.sprite = texture;
        }
        
        _spriteRenderer.color = Color.gray7;
    }

    public void AcquireTable(TableUnit table)
    {
        _table = table;
        if (_table == null)
        {
            ChangeState(CustomerStates.Waiting);
            return;
        }
        
        desiredPosition = _table.GetSeat(this);
        direction = (desiredPosition - transform.position).normalized;
        ChangeState(CustomerStates.Walking);
    }

    public void GiveOrder()
    {
        ChangeState(CustomerStates.Ordered);
    }

    // -------------------------------------------- Helper Functions --------------------------------------------
    private void OrderItem()
    {
        _order.SetActive(true);
        ChangeState(CustomerStates.Waiting);
        
        /*
        if (Vector3.Distance(WaitressMovement.Instance.transform.position, transform.position) < 0.1f &&
            WaitressMovement.Instance.agent.remainingDistance < 0.1f)
        {
            Transform Order = transform.GetChild(0);
            Order.gameObject.SetActive(false);
            ReceiveItem(WaitressMovement.Instance.itemInHand);
        }
        _timeWaited = 0f;
        */
    }

    private void OrderingItem()
    {
        print("todo: wait a few seconds, the player need to click on it add a random gen to get a random order");
        
        if (Vector3.Distance(WaitressMovement.Instance.transform.position, transform.position) < 0.1f &&
            WaitressMovement.Instance.agent.remainingDistance < 0.1f)
        {
            ChangeState(CustomerStates.Ordered);
            Transform Order = transform.GetChild(0);
            Order.gameObject.SetActive(true);
        }
    }

    private void Eating()
    {
        if (_timeWaited > 2f)
        {
            if (_happy) _spriteRenderer.color = Color.white;
            TableHandler.Instance.FreeTable(_table);
            _table = null;
            
            desiredPosition = spawnPosition;
            direction = (desiredPosition - transform.position).normalized;
            ChangeState(CustomerStates.Walking);
        }
    }
    
    private void Move()
    {
        _order.SetActive(false);
        transform.position += direction * (speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, desiredPosition) > 0.1f) return;
        if (_served) Destroy(gameObject);
        if (_table is not null) ChangeState(CustomerStates.Ordered);
        else ChangeState(CustomerStates.Waiting);
    }

    private void Wait()
    {
        if (_timeWaited > maxWaitTime)
        {
            TableHandler.Instance.FreeTable(_table);
            _table = null;
            
            desiredPosition = spawnPosition;
            direction = (desiredPosition - transform.position).normalized;
            _served = true;
            ChangeState(CustomerStates.Walking);
        }
    }

    private void ChangeState(CustomerStates newState)
    {
        states = newState;
        if (newState != CustomerStates.Waiting) _timeWaited = 0f;
    }
}
