using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class WaitressMovement : MonoBehaviour
{
    public static WaitressMovement Instance { get; private set; }

    public UnityEvent destinationReached;
    public Consumable itemInHand;
    public NavMeshAgent agent;

    private Animator animator;
    private bool hasDestination = false;
    [SerializeField] private float arrivalThreshold = 0.05f;
    [SerializeField] private float baseSpeed = 3.5f; // Default NavMeshAgent speed

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            agent.updateRotation = false;
            agent.updateUpAxis = false;
            agent.height = 1.0f;
            agent.baseOffset = 0.3f;
            agent.speed = baseSpeed; // Set initial speed

            if (destinationReached == null)
                destinationReached = new UnityEvent();

            ApplySpeedMultiplier(); // Apply initial multiplier
        }
        else
        {
            Destroy(gameObject);
        }

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!hasDestination) return;
        if (agent.pathPending) return;

        if (agent.remainingDistance <= arrivalThreshold)
        {
            hasDestination = false;
            agent.isStopped = true;
            destinationReached?.Invoke();
            animator.SetBool("isWalking", false);
        }
    }

    public void MoveTo(Vector3 locationToMove)
    {
        agent.isStopped = false;
        agent.SetDestination(locationToMove);
        hasDestination = true;
        animator.SetBool("isWalking", true);

        Vector3 direction = locationToMove - transform.position;

        if (direction.x > 0f)
            transform.localScale = new Vector3(1, 1, 1);   // looking right
        else if (direction.x < 0f)
            transform.localScale = new Vector3(-1, 1, 1);  // looking left
    }

    public void ApplySpeedMultiplier()
    {
        if (UpgradeManager.Instance != null)
        {
            agent.speed = baseSpeed * UpgradeManager.Instance.GetWaitressSpeedMultiplier();
        }
    }
}
