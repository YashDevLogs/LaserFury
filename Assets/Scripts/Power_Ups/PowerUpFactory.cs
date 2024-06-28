using UnityEngine;

public class PowerUpFactory
{
    public static GameObject CreatePowerUp(string type)
    {
        GameObject powerUpPrefab = null;
        switch (type)
        {
            case "Shield":
                powerUpPrefab = Resources.Load<GameObject>("Prefabs/ShieldPowerUp");
                break;
            case "SlowDown":
                powerUpPrefab = Resources.Load<GameObject>("Prefabs/SlowDownPowerUp");
                break;
            case "LaserProtectionGear":
                powerUpPrefab = Resources.Load<GameObject>("Prefabs/LaserProtectionGearPowerUp");
                break;
        }
        return powerUpPrefab;
    }
}
 