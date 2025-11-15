using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    // Upgrade levels
    [SerializeField] private int waitressSpeedLevel = 1;
    [SerializeField] private int customerPatienceLevel = 1;
    [SerializeField] private int spawnRateLevel = 1;

    // Multipliers (increase per level)
    [SerializeField] private float waitressSpeedMultiplierPerLevel = 0.1f; // e.g., +10% per level
    [SerializeField] private float customerPatienceMultiplierPerLevel = 0.2f; // e.g., +20% per level
    [SerializeField] private float spawnRateMultiplierPerLevel = 0.15f; // e.g., +15% per level (faster spawning)

    // Costs (increase per level)
    [SerializeField] private int baseWaitressSpeedCost = 50;
    [SerializeField] private int baseCustomerPatienceCost = 40;
    [SerializeField] private int baseSpawnRateCost = 60;

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

    // Get current multipliers
    public float GetWaitressSpeedMultiplier()
    {
        return 1f + (waitressSpeedLevel - 1) * waitressSpeedMultiplierPerLevel;
    }

    public float GetCustomerPatienceMultiplier()
    {
        return 1f + (customerPatienceLevel - 1) * customerPatienceMultiplierPerLevel;
    }

    public float GetSpawnRateMultiplier()
    {
        return 1f + (spawnRateLevel - 1) * spawnRateMultiplierPerLevel;
    }

    // Upgrade methods (assume some currency system exists, e.g., money)
    public bool UpgradeWaitressSpeed()
    {
        int cost = baseWaitressSpeedCost * waitressSpeedLevel;
        // TODO: Check if player has enough money, deduct if yes
        // For now, just upgrade
        waitressSpeedLevel++;
        ApplyUpgrades();
        Debug.Log($"Upgraded waitress speed to level {waitressSpeedLevel}");
        return true;
    }

    public bool UpgradeCustomerPatience()
    {
        int cost = baseCustomerPatienceCost * customerPatienceLevel;
        // TODO: Check if player has enough money, deduct if yes
        customerPatienceLevel++;
        ApplyUpgrades();
        Debug.Log($"Upgraded customer patience to level {customerPatienceLevel}");
        return true;
    }

    public bool UpgradeSpawnRate()
    {
        int cost = baseSpawnRateCost * spawnRateLevel;
        // TODO: Check if player has enough money, deduct if yes
        spawnRateLevel++;
        ApplyUpgrades();
        Debug.Log($"Upgraded spawn rate to level {spawnRateLevel}");
        return true;
    }

    // Apply upgrades to relevant components
    private void ApplyUpgrades()
    {
        if (WaitressMovement.Instance != null)
        {
            WaitressMovement.Instance.ApplySpeedMultiplier();
        }
        if (CustomerSpawner.Instance != null)
        {
            CustomerSpawner.Instance.ApplySpawnRateMultiplier();
        }
        // Customer patience is applied per customer instance
    }

    // Get costs for UI or checks
    public int GetWaitressSpeedCost()
    {
        return baseWaitressSpeedCost * waitressSpeedLevel;
    }

    public int GetCustomerPatienceCost()
    {
        return baseCustomerPatienceCost * customerPatienceLevel;
    }

    public int GetSpawnRateCost()
    {
        return baseSpawnRateCost * spawnRateLevel;
    }
}
