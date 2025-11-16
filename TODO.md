# TODO: Fix MissingReferenceException in Customer.cs

- [x] Edit Assets/Scripts/TableHandler.cs: Add null check in FreeTable() method after dequeuing a waiting customer to ensure it's not destroyed before calling AcquireTable. If null, enqueue the table back to _freeTables.
- [ ] Test the fix by running the game and verifying no more MissingReferenceException occurs when customers are assigned tables.
