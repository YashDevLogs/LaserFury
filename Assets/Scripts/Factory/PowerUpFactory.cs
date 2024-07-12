using UnityEngine;
using System.Collections.Generic;

public class PowerUpFactory
{
    private Dictionary<PowerupTypes, GameObject> powerUpPrefabs = new Dictionary<PowerupTypes, GameObject>(); // Using GameObject as Type as it makes methods like "Instantiate" and "transform" accessible for powerup Spawning.

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
            // needs to be a game object so i can instantiate it in the game scene using "Instantiate".
            return GameObject.Instantiate(powerUpPrefabs[type]);
        }
        return null;
    }
}
