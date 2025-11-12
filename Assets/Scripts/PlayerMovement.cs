using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

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

    public void OnClick(Vector2 locationToMove)
    {
        Debug.Log("PlayerMovement received a click event.");
        // Player smoothly walks to location
        // Implement movement logic here
        
    }
}
