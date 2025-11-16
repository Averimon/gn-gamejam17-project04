using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    [SerializeField] private float baseMaxWaitTime = 1f;
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private Vector3 direction;
    [SerializeField] private List<Consumable> menu = new List<Consumable>();

    [Header("Debug")]
    [SerializeField] private CustomerStates states = CustomerStates.Waiting;
    [SerializeField] private Consumable craving;
    [SerializeField] private Vector3 desiredPosition;

    [SerializeField] private UnityEngine.AI.NavMeshAgent agent;

    private GameObject _order;
    private TableUnit _table;
    private float _timeWaited = 0f;
    private bool _happy = false;
    private bool _served = false;

    private SpriteRenderer _mainSpriteRenderer;
    private SpriteRenderer _highlightSpriteRenderer;
    private Sprite _lastSprite;
    private Animator _animator;

    // -------------------------------------------- Event Functions --------------------------------------------

    void Awake()
    {
        _mainSpriteRenderer = GetComponent<SpriteRenderer>();
        _order = transform.GetChild(0).gameObject;
        _animator = GetComponent<Animator>();

        Transform colorSpriteChild = transform.Find("ColorSprite");
        if (colorSpriteChild != null)
        {
            _highlightSpriteRenderer = colorSpriteChild.GetComponent<SpriteRenderer>();
        }

        if (_mainSpriteRenderer != null && _highlightSpriteRenderer != null)
        {
            _lastSprite = _mainSpriteRenderer.sprite;
            _highlightSpriteRenderer.sprite = _lastSprite;
        }
    }

    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.height = 1.0f;
        agent.baseOffset = 0.3f;
        if (menu.Count == 0)
        {
            Debug.LogError("Please add at least one Consumable to the customers menu!");
            gameObject.SetActive(false); // Disable customer if no menu
            return;
        }
        craving = menu[UnityEngine.Random.Range(0, menu.Count)];
        spawnPosition = transform.position;
        AcquireTable(TableHandler.Instance.GetFreeTable(this));
        agent.SetDestination(desiredPosition);
        ChangeState(CustomerStates.Walking);
        ApplyPatienceMultiplier(); // Apply initial multiplier
    }

    private void Update()
    {
        if(this.gameObject == null) return;
        _timeWaited += Time.deltaTime;


        if (_mainSpriteRenderer != null && _highlightSpriteRenderer != null)
        {
            if (_mainSpriteRenderer.sprite != _lastSprite)
            {
                _lastSprite = _mainSpriteRenderer.sprite;
                _highlightSpriteRenderer.sprite = _lastSprite;
            }
        }

        if (agent.remainingDistance <= 0.1f && !agent.isStopped)
        {
            agent.isStopped = true;
            if (spawnPosition != desiredPosition && _table != null)
            {
                ChangeState(CustomerStates.Ordered);
            }else
            {
                // Reached spawn position, destroy customer
                Destroy(gameObject);
            }
        }

        switch (states)
        {
            case CustomerStates.Waiting:
                Wait();
                break;
            case CustomerStates.Walking:
                _animator.SetBool("isSitting", false);
                _animator.SetBool("isWalking", true);
                Move();
                break;
            /*REFACTOR: case CustomerStates.Ordering:
                OrderingItem();
                break;*/
            case CustomerStates.Ordered:
                _animator.SetBool("isWalking", false);
                _animator.SetBool("isSitting", true);
                OrderItem();
                break;
            case CustomerStates.Eating:
                _animator.SetBool("isHappy", true);
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
        // Eating state will handle the wait and then transition to Walking
    }

    public void SetTexture(Sprite texture)
    {
        if (_mainSpriteRenderer != null)
        {
            _mainSpriteRenderer.sprite = texture;
        }

        _mainSpriteRenderer.color = Color.grey;
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
        StartCoroutine(LosingPatience(craving));
        _order.SetActive(true);
        StartCoroutine(OrderingTimer());
    }

    private IEnumerator OrderingTimer()
    {
        yield return new WaitForSeconds(1f); // Wait 1 second for ordering

        if (Vector3.Distance(WaitressMovement.Instance.transform.position, _table.gameObject.transform.position) <= 1f &&
            WaitressMovement.Instance.agent.remainingDistance < 0.1f &&
            WaitressMovement.Instance.itemInHand != null)
        {
            ReceiveItem(WaitressMovement.Instance.itemInHand);
            _order.gameObject.SetActive(false);
        }
        _timeWaited = 0f;
    }

    // TODO: THIS LINE SHOULD BE PLACED SOMEWHERE HERE ??
    // ChangeState(CustomerStates.Waiting);

    private void OrderingItem()
    {
        print("todo: wait a few seconds, the player need to click on it add a random gen to get a random order");

        if (Vector3.Distance(WaitressMovement.Instance.transform.position, _table.gameObject.transform.position) <= 1f &&
            WaitressMovement.Instance.agent.remainingDistance < 0.1f)
        {
            ChangeState(CustomerStates.Ordered);
            _order.gameObject.SetActive(true);
            
        }
    }

    private void Eating()
    {
        if (_timeWaited > 2f)
        {
            if (_happy) _mainSpriteRenderer.color = Color.white;
            TableHandler.Instance.FreeTable(_table);
            _table = null;

            desiredPosition = spawnPosition;
            agent.isStopped = false;
            agent.SetDestination(spawnPosition);
            ChangeState(CustomerStates.Walking);
        }
    }

    private void Move()
    {
        _order.SetActive(false);

        if (Vector3.Distance(transform.position, desiredPosition) > 0.1f) return;
        if (_table is not null) ChangeState(CustomerStates.Ordered);
        else ChangeState(CustomerStates.Waiting);
    }

    private void Wait()
    {
        _timeWaited += Time.deltaTime;
        float effectiveMaxWaitTime = baseMaxWaitTime * GetPatienceMultiplier();
        if (_timeWaited > effectiveMaxWaitTime)
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

    private void ApplyPatienceMultiplier()
    {
        // Patience is applied in Wait() method
    }

    private float GetPatienceMultiplier()
    {
        if (UpgradeManager.Instance != null)
        {
            return UpgradeManager.Instance.GetCustomerPatienceMultiplier();
        }
        return 1f;
    }

    private void ChangeDestination(Vector3 newDestination)
    {
        desiredPosition = newDestination;
        direction = (desiredPosition - transform.position).normalized;
        _mainSpriteRenderer.flipX = direction.x < 0;
    }

    private IEnumerator LosingPatience(Consumable consumable)
    {
        consumable.transform.localPosition = Vector3.zero;

        var mask = consumable.transform.Find("Mask").GetComponent<SpriteMask>();
        
        Vector3 endPos = mask.transform.localPosition;
        float maskScaleOffset = mask.transform.localScale.y;
        Vector3 startPos = new Vector3(endPos.x, endPos.y - maskScaleOffset, endPos.z);

        float elapsedTime = 0f;
        mask.transform.localPosition = startPos;

        while (elapsedTime < baseMaxWaitTime)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / baseMaxWaitTime);
            mask.transform.localPosition = Vector3.Lerp(startPos, endPos, t);

            yield return null;
        }

        mask.transform.localPosition = endPos;
    }
}
