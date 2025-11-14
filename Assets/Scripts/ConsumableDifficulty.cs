using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableDifficulty", menuName = "Scriptable Objects/ConsumableDifficulty")]
public class ConsumableDifficulty : ScriptableObject
{
    [SerializeField] private string difficultyName;
    [SerializeField] private float preperationTime = 1.0f;
}
