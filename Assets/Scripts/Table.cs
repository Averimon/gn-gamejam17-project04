using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] private GameObject seat1;
    [SerializeField] private GameObject seat2;
    
    void Start()
    {
        if (seat1 == null)
        {
            Debug.LogError("please add a GameObject as seat1!");
            return;
        }
        if (seat2 == null)
        {
            Debug.LogError("please add a GameObject as seat2!");
            return;
        }
    }

    public Vector3 GetSeat()
    {
        if (Random.value > 0.5) return seat1.transform.position;
        else return seat2.transform.position;
    }
}
