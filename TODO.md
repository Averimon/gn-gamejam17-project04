# TODO: Add Open/Closed Cycle

- [x] Create RestaurantManager.cs script to manage open/closed state with timer-based cycle (open for 60s, closed for 30s).
- [x] Modify CustomerSpawner.cs to check restaurant state before spawning customers.
- [x] Test the game to ensure the cycle works and spawning stops when closed.

# TODO: Add Upgrades

- [x] Create UpgradeManager.cs script as a singleton to manage upgrade levels, costs, and multipliers for waitress speed, customer patience, and spawn rate.
- [x] Edit WaitressMovement.cs to add speedMultiplier and apply it to agent.speed.
- [x] Edit Customer.cs to add patienceMultiplier and multiply maxWaitTime by it.
- [x] Edit CustomerSpawner.cs to add spawnRateMultiplier and divide spawnTime by it (for faster spawning).
- [x] Edit RestaurantManager.cs to integrate UpgradeManager instance (if needed for initialization).
- [ ] Test the upgrades in-game to ensure multipliers are applied correctly.
