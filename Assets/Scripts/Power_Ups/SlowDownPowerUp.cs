using UnityEngine;
using System.Collections;

public class SlowDownPowerUp : MonoBehaviour, IPowerUps
{
    private float duration = 5f; // Duration for the time slowdown
    private float slowDownFactor = 0.5f; // Factor by which time will be slowed down

    public void ApplyPowerUp(Player player)
    {
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public float PowerUpDuration()
    {
        return duration;
    }

    public void RemovePowerUp(Player player)
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }


}
