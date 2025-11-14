using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "Scriptable Objects/Consumable")]
public class Consumable : ScriptableObject
{
    [SerializeField] private string consumableName;
    [SerializeField] private Sprite icon;
    [SerializeField] private ConsumableDifficulty difficulty;
}
