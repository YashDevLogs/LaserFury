using UnityEngine;
using System.Collections.Generic;

public class PowerUpFactory
{
    private Dictionary<string, GameObject> powerUpPrefabs;

    public PowerUpFactory()
    {
        powerUpPrefabs = new Dictionary<string, GameObject>
        {
            { "Shield", Resources.Load<GameObject>("Prefabs/ShieldPowerUp") },
            { "SlowDown", Resources.Load<GameObject>("Prefabs/SlowDownPowerUp") },
            { "LaserProtectionGear", Resources.Load<GameObject>("Prefabs/LaserProtectionGearPowerUp") }
        };
    }

    public GameObject CreatePowerUp(string type)
    {
        if (powerUpPrefabs.ContainsKey(type))
        {
            return GameObject.Instantiate(powerUpPrefabs[type]);
        }
        return null;
    }
}
