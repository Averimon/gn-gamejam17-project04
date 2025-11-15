using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableDifficulty", menuName = "Scriptable Objects/ConsumableDifficulty")]
public class ConsumableDifficulty : ScriptableObject
{
    public float preparationTime = 1.0f;
    [SerializeField] private string difficultyName;
}
