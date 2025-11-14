using UnityEngine;

public class WaitressInteraction : MonoBehaviour
{   
    public static WaitressInteraction Instance { get; private set; }

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

    public void PickUp(Consumable consumable)
    {
        Debug.Log($"Picked up consumable: {consumable}");
        WaitressMovement.Instance.itemInHand = consumable;
    }
}
