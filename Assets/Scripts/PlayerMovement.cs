using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    private bool isMoving = false;

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
        if (!isMoving)
        {
            Debug.Log("PlayerMovement received a click event.");
            StartCoroutine(MovePlayer(locationToMove));
        } else
        {
            Debug.Log("Player is already moving. Click ignored.");
        }
    }

    private IEnumerator MovePlayer(Vector2 targetPosition)
    {
        isMoving = true;
        float duration = 1.0f;
        float elapsed = 0.0f;
        Vector2 startingPosition = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector2.Lerp(startingPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
    }
}
