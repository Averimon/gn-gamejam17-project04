using UnityEngine;

public class Customer : MonoBehaviour
{
    private enum CustomerState
    {
        Waiting,
        Walking,
        Satisfied,
    }
    
    [SerializeField] private float speed = 5f;
    [SerializeField] private CustomerState state = CustomerState.Waiting;
    [SerializeField] private Vector3 desiredPosition;
    [SerializeField] private Vector3 direction;
    
    [SerializeField] private Item craving;
    
    // -------------------------------------------- Event Functions --------------------------------------------
    private void Start()
    {
        // for testing:
        desiredPosition.x = Random.Range(-9f, 9f);
        desiredPosition.y = Random.Range(-5f, 5f);
        state = CustomerState.Walking;
        // -----------
    }

    private void Update()
    {
        switch (state)
        {
            case CustomerState.Waiting:
                print("todo");
                break;
            case CustomerState.Walking:
                Move();
                break;
        }
    }
    
    // -------------------------------------------- Public Functions --------------------------------------------
    public void StartMoving()
    {
        direction = (transform.position - desiredPosition).normalized;
        state = CustomerState.Walking;
    }

    public void ReciveItem(Item recived)
    {
        print("todo");
        if (recived.type == craving.type)
        {
            state = CustomerState.Satisfied;
        }
    }

    // -------------------------------------------- Helper Functions --------------------------------------------
    private void Move()
    {
        direction = (desiredPosition - transform.position).normalized;   
        transform.position += direction * (speed * Time.deltaTime);
        
        if (Vector3.Distance(transform.position, desiredPosition) < 0.1f)
        {
            state = CustomerState.Waiting;
        }
    }
}
