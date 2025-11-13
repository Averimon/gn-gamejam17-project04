using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] private GameObject chair1;
    [SerializeField] private GameObject chair2;
    
    void Start()
    {
        if (chair1 == null)
        {
            Debug.LogError("please add a GameObject as seat1!");
            return;
        }
        if (chair2 == null)
        {
            Debug.LogError("please add a GameObject as seat2!");
            return;
        }
    }

    public Vector3 GetSeat()
    {
        if (Random.value > 0.5) return chair1.transform.position;
        else return chair2.transform.position;
    }
}
