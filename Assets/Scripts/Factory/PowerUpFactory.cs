using UnityEngine;
using System.Collections.Generic;

public class PowerUpFactory
{
    private Dictionary<PowerupTypes, GameObject> powerUpPrefabs = new Dictionary<PowerupTypes, GameObject>();

    public PowerUpFactory(PowerUpDatabase powerUpDatabase)
    {
        foreach (var powerUpData in powerUpDatabase.powerUps)
        {
            if (!powerUpPrefabs.ContainsKey(powerUpData.powerUpType))
            {
                powerUpPrefabs.Add(powerUpData.powerUpType, powerUpData.powerUpPrefab);
            }
        }
    }

    public GameObject CreatePowerUp(PowerupTypes type)
    {
        if (powerUpPrefabs.ContainsKey(type))
        {
            return GameObject.Instantiate(powerUpPrefabs[type]);
        }
        return null;
    }
}
