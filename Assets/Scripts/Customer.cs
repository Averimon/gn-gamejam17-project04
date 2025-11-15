using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    private enum CustomerStates
    {
        Waiting,
        Walking,
        Ordered,
        Eating,
    }
    
    [Header("Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxWaitTime = 1f;
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private List<Consumable> menu = new List<Consumable>();
    
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
        if (menu.Count == 0)
        {
            Debug.LogError("Please add at least one Consumable to the customers menu!");
            return;
        }
        craving = menu[UnityEngine.Random.Range(0, menu.Count)];
        craving.gameObject.SetActive(true);
        
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
        _order.SetActive(false);
        _happy = received.type == craving.type;

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
        
        ChangeDestination(_table.GetSeat(this));
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
        StartCoroutine(LosingPatience(craving));
        ChangeState(CustomerStates.Waiting);
    }

    private void Eating()
    {
        if (_timeWaited > 2f)
        {
            if (_happy) _spriteRenderer.color = Color.white;
            TableHandler.Instance.FreeTable(_table);
            _table = null;
            
            ChangeDestination(spawnPosition);
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
            _served = true;
            ChangeDestination(spawnPosition);
            ChangeState(CustomerStates.Walking);
        }
    }

    private void ChangeState(CustomerStates newState)
    {
        states = newState;
        if (newState != CustomerStates.Waiting) _timeWaited = 0f;
    }

    private void ChangeDestination(Vector3 newDestination)
    {
        desiredPosition = newDestination;
        direction = (desiredPosition - transform.position).normalized;
        _spriteRenderer.flipX = direction.x < 0;
    }
    
    private IEnumerator LosingPatience(Consumable consumable)
    {
        consumable.transform.localPosition = Vector3.zero;

        var mask = consumable.transform.Find("Mask").GetComponent<SpriteMask>();

        float elapsedTime = 0f;
        mask.transform.localPosition = consumable.maskStartPos;

        while (elapsedTime < maxWaitTime)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / maxWaitTime);
            mask.transform.localPosition = Vector3.Lerp(consumable.maskStartPos, consumable.maskEndPos, t);

            yield return null;
        }

        mask.transform.localPosition = consumable.maskEndPos;
    }
}
