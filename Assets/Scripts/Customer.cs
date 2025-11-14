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

    private Table _table;
    private float _timeWaited = 0f;
    private bool _happy = false;
    private bool _served = false;

    // -------------------------------------------- Event Functions --------------------------------------------
    private void Start()
    {
        spawnPosition = transform.position;
        AcquireTable(TableHandler.Instance.GetFreeTable(this));
    }

    private void Update()
    {
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
        _happy = received.type == craving.type;

        ChangeState(CustomerStates.Eating);
        print("todo: Wait a few seconds?");

        TableHandler.Instance.FreeTable(_table);
        desiredPosition = spawnPosition;
        direction = (desiredPosition - transform.position).normalized;
        ChangeState(CustomerStates.Walking);
    }

    public void SetTexture(Sprite texture)
    {
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        if (rend != null)
        {
            rend.sprite = texture;
        }
    }

    public void AcquireTable(Table table)
    {
        _table = table;
        if (_table == null)
        {
            ChangeState(CustomerStates.Waiting);;
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
        print("todo: Add a timer to simulate ordering time");
        if (Vector3.Distance(WaitressMovement.Instance.transform.position, transform.position) < 0.1f &&
            WaitressMovement.Instance.agent.remainingDistance < 0.1f)
        {
            print("todo: verify if order that player brought is correct");
            Transform Order = transform.GetChild(0);
            Order.gameObject.SetActive(false);
            ReceiveItem(WaitressMovement.Instance.itemInHand);
        }
        _timeWaited = 0f;
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
        _timeWaited += Time.deltaTime;
        if (_timeWaited > 2f)
        {
            TableHandler.Instance.FreeTable(_table);
            
            desiredPosition = spawnPosition;
            direction = (desiredPosition - transform.position).normalized;
            ChangeState(CustomerStates.Walking);
        }
    }
    private void Move()
    {
        transform.position += direction * (speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, desiredPosition) < 0.1f) ChangeState(CustomerStates.Waiting);
        if (_served && Vector3.Distance(transform.position, spawnPosition) < 0.1f) ;
    }

    private void Wait()
    {
        _timeWaited += Time.deltaTime;
        if (_timeWaited > maxWaitTime)
        {
            TableHandler.Instance.FreeTable(_table);
            
            desiredPosition = spawnPosition;
            direction = (desiredPosition - transform.position).normalized;
            ChangeState(CustomerStates.Walking);
        }
    }

    private void ChangeState(CustomerStates newState)
    {
        states = newState;
        if (newState != CustomerStates.Waiting) _timeWaited = 0f;
    }
}
