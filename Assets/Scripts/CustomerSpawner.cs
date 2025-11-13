using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private List<GameObject> customers;
    [SerializeField] private Vector2 spawnPosition = Vector2.zero;
    [SerializeField] private bool spawnEnabled = true;
    [SerializeField] private float spawnTime = 1f;
    [SerializeField] private float timePassed = 0f;
    
    // -------------------------------------------- Event Functions --------------------------------------------
    void Start()
    {
        if (customerPrefab == null)
        {
            Debug.LogError("CustomerPrefab is null, add to this script where it is used in the scene!");
            return;
        }
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
        customers.Add(customer);
    }
}
