using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpData", menuName = "ScriptableObjects/PowerUpData", order = 1)]
public class PowerUpData_SO : ScriptableObject
{
    public PowerupTypes powerUpType;
    public GameObject powerUpPrefab;
}

