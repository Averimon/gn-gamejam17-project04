using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public static CustomerSpawner Instance { get; private set; }
    
    [Header("Prefabs")]
    [SerializeField] private List<GameObject> customerPrefabs = new List<GameObject>(); // finished customer variants
    [SerializeField] private List<GameObject> customers = new List<GameObject>();       // active customers in the scene
    
    [Header("Spawn Settings")]
    [SerializeField] private Vector3 spawnPosition = Vector3.zero;
    [SerializeField] private bool spawnEnabled = true;
    [SerializeField] private float spawnTime = 1f;
    [SerializeField] private float timePassed = 0f;
    
    // -------------------------------------------- Event Functions --------------------------------------------
    private void Awake()
    {
        // Singleton setup
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
    
    private void Start()
    {
        // Check if prefab list is assigned
        if (customerPrefabs == null || customerPrefabs.Count == 0)
        {
            Debug.LogError("CustomerSpawner: 'customerPrefabs' is empty. Please assign finished customer prefabs in the inspector.");
        }
    }

    private void Update()
    {
        if (!spawnEnabled) return;
        
        // Spawn timer
        if (timePassed > spawnTime)
        {
            SpawnCustomer();
            timePassed = 0f;
        }
        else
        {
            timePassed += Time.deltaTime;
        }
    }
    
    // -------------------------------------------- Public Functions --------------------------------------------
    public void ToggleSpawner()
    {
        // Enables or disables the spawner
        spawnEnabled = !spawnEnabled;
    }
    
    // -------------------------------------------- Helper Functions --------------------------------------------
    private void SpawnCustomer()
    {
        // Safety checks
        if (customerPrefabs == null || customerPrefabs.Count == 0)
        {
            Debug.LogWarning("CustomerSpawner: No prefabs assigned in 'customerPrefabs'.");
            return;
        }

        // Pick a random finished customer prefab
        GameObject prefabToSpawn = customerPrefabs[Random.Range(0, customerPrefabs.Count)];
        if (prefabToSpawn == null)
        {
            Debug.LogWarning("CustomerSpawner: One of the prefabs in 'customerPrefabs' is null.");
            return;
        }

        // Spawn the customer
        GameObject customer = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        
        // Add to list of active customers
        customers.Add(customer);
    }
}
