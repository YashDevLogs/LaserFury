using UnityEngine;

public class SlowDownPowerUp : MonoBehaviour, IPowerUps
{
    public float duration = 10f;
    public float slowDownFactor = 0.5f;

    public void ApplyPowerUp(Player player)
    {
        foreach (Laser laser in FindObjectsOfType<Laser>())
        {
            laser.RotationSpeed *= slowDownFactor;
        }
    }

    public float PowerUpDuration()
    {
        return duration;
    }

    public void RemovePowerUp(Player player)
    {
        foreach (Laser laser in FindObjectsOfType<Laser>())
        {
            laser.RotationSpeed /= slowDownFactor;
        }
    }
}
