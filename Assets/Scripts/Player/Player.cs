using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public GameObject Shield; // A transparent sphere-like GameObject (with no script) which enables when the shield is active.

    private bool isPowerUpActive = false;
    private IPowerUps activePowerUp;
    private float powerUpStartTime;
    private float powerUpDuration;

    public bool HasShield { get; set; }
    public bool HasLaserProtectionGear { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has an IPowerUps component
        IPowerUps powerUp = other.GetComponent<IPowerUps>();
        if (powerUp != null)
        {
            // Apply & Destroy power-up
            Destroy(other.gameObject);
            ApplyPowerUp(powerUp);
        }
        else
        {
            Debug.LogWarning("No IPowerUps component found on: " + other.gameObject.name);
        }
    }

    private void Update()
    {
        // Check if a power-up is active and should be expired
        if (isPowerUpActive && Time.time >= powerUpStartTime + powerUpDuration)
        {
            RemovePowerUp(activePowerUp);
        }
    }

    private void ApplyPowerUp(IPowerUps powerUp)
    {
        if (!isPowerUpActive)
        {
            isPowerUpActive = true;
            activePowerUp = powerUp;
            powerUpStartTime = Time.time;
            powerUpDuration = powerUp.PowerUpDuration();
            powerUp.ApplyPowerUp(this);
        }
    }

    private void RemovePowerUp(IPowerUps powerUp)
    {
        isPowerUpActive = false;
        activePowerUp = null;
        powerUp.RemovePowerUp(this);
    }

    public void TakeDamage()
    {
        EventService.PlayerDied();
    }

}
