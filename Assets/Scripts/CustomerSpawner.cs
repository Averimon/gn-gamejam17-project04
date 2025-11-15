using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public static CustomerSpawner Instance { get; private set; }
    
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private List<GameObject> customers;
    [SerializeField] private List<Sprite> skins = new List<Sprite>();
    [SerializeField] private Vector3 spawnPosition = new Vector3(-3.8f, 2.2f, 0f);
    [SerializeField] private bool spawnEnabled = true;
    [SerializeField] private float spawnTime = 1f;
    [SerializeField] private float timePassed = 0f;
    
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
        if (customerPrefab == null)
        {
            Debug.LogError("CustomerPrefab is null, add to this script where it is used in the scene!");
            return;
        }
        if (skins.Count <= 0) Debug.LogError("Customer skins must have at least one skin");
    }

    void Update()
    {
        if (spawnEnabled && timePassed > spawnTime)
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
        spawnEnabled = !spawnEnabled;
    }
    
    // -------------------------------------------- Helper Functions --------------------------------------------
    private void SpawnCustomer()
    {
        GameObject customer = Instantiate(customerPrefab, spawnPosition, Quaternion.identity);
        customer.GetComponent<Customer>().SetTexture(skins[Random.Range(0, skins.Count)]);
        customers.Add(customer);
    }
}
