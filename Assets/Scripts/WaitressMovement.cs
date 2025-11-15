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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            agent.updateRotation = false;
            agent.updateUpAxis = false;

            if (destinationReached == null)
                destinationReached = new UnityEvent();
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
}
