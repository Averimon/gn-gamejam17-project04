using UnityEngine;
using UnityEngine.Events;

public class RestaurantManager : MonoBehaviour
{
    public static RestaurantManager Instance { get; private set; }

    public UnityEvent<bool> onRestaurantStateChanged; // true for open, false for closed

    [SerializeField] private bool isOpen = true;
    [SerializeField] private float openDuration = 60f; // seconds
    [SerializeField] private float closedDuration = 30f; // seconds
    private float timer = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (onRestaurantStateChanged == null)
                onRestaurantStateChanged = new UnityEvent<bool>();
        }
        else
        {
            Destroy(gameObject);
        }
        // Ensure UpgradeManager is initialized if not already
        if (UpgradeManager.Instance == null)
        {
            // Assuming UpgradeManager is in the scene or will be instantiated elsewhere
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        float duration = isOpen ? openDuration : closedDuration;

        if (timer >= duration)
        {
            isOpen = !isOpen;
            CustomerSpawner.Instance.ToggleSpawner();
            timer = 0f;
            onRestaurantStateChanged?.Invoke(isOpen);
            Debug.Log($"Restaurant is now {(isOpen ? "open" : "closed")}.");
        }
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}
