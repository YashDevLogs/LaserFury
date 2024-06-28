using UnityEngine;

public class LaserProtectionGearPowerUp : MonoBehaviour, IPowerUps
{
    public float duration = 10f;

    public void ApplyPowerUp(Player player)
    {
        player.HasLaserProtectionGear = true;
        Debug.Log("Laser protection gear applied");
    }

    public void RemovePowerUp(Player player)
    {
        player.HasLaserProtectionGear = false;
        Debug.Log("Laser protection gear removed");
    }

    public float PowerUpDuration()
    {
        return duration;
    }
}
