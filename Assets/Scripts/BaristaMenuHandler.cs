using System.Collections.Generic;
using UnityEngine;

public class BaristaMenuHandler : MonoBehaviour
{
    [Header("Consumable Prefabs")]
    [SerializeField] private List<GameObject> availableConsumables;

    [Header("Slots (UI elements)")]
    [SerializeField] private List<Transform> consumableSlots;

    private int currentStartIndex = 0;

    public List<GameObject> GetAvailableConsumables()
    {
        return availableConsumables;
    }

    private void Start()
    {
        RefreshMenu();
    }

    /// <summary>
    /// Instantiates consumable prefabs as children of the slot objects.
    /// </summary>
    private void RefreshMenu()
    {
        if (availableConsumables == null || consumableSlots == null)
            return;

        int consumableCount = availableConsumables.Count;
        int slotCount       = consumableSlots.Count;

        for (int slotIndex = 0; slotIndex < slotCount; slotIndex++)
        {
            Transform slot = consumableSlots[slotIndex];
            if (slot == null) continue;

            // Remove old children from this slot
            for (int i = slot.childCount - 1; i >= 0; i--)
            {
                Destroy(slot.GetChild(i).gameObject);
            }

            if (consumableCount == 0)
            {
                slot.gameObject.SetActive(false);
                continue;
            }

            int consumableIndex = (currentStartIndex + slotIndex) % consumableCount;
            GameObject prefab = availableConsumables[consumableIndex];

            if (prefab != null)
            {
                slot.gameObject.SetActive(true);

                // Instantiate prefab as child of the slot
                GameObject instance = Instantiate(prefab, slot);
                instance.transform.localPosition = new Vector3(0f, -0.15f, 0f);
                instance.transform.localScale    = new Vector3(0.2f, 0.2f, 1f);
                instance.transform.localRotation = Quaternion.identity;
            }
            else
            {
                slot.gameObject.SetActive(false);
            }
        }
    }

    public void OnClickRight()
    {
        Debug.Log("Clicked Right");
        if (availableConsumables == null || availableConsumables.Count == 0)
            return;

        currentStartIndex = (currentStartIndex + 1) % availableConsumables.Count;
        RefreshMenu();
    }

    public void OnClickLeft()
    {
        Debug.Log("Clicked Left");
        if (availableConsumables == null || availableConsumables.Count == 0)
            return;

        currentStartIndex--;
        if (currentStartIndex < 0)
            currentStartIndex += availableConsumables.Count;

        RefreshMenu();
    }
}
