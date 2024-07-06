using UnityEngine;
using System.Collections.Generic;

public class PowerUpFactory
{
    private Dictionary<PowerupTypes, GameObject> powerUpPrefabs;

    public PowerUpFactory()
    {
        powerUpPrefabs = new Dictionary<PowerupTypes, GameObject>
        {
            { PowerupTypes.Shield, Resources.Load<GameObject>("Prefabs/ShieldPowerUp") },
            { PowerupTypes.SlowDown, Resources.Load<GameObject>("Prefabs/SlowDownPowerUp") },
            { PowerupTypes.LaserProtectionGear, Resources.Load<GameObject>("Prefabs/LaserProtectionGearPowerUp") }
        };
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
