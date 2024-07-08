using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject Shield;
    public bool HasShield { get; set; }
    public bool HasLaserProtectionGear { get; set; }

    private bool isPowerUpActive = false;
    private IPowerUps activePowerUp;
    private float powerUpStartTime;
    private float powerUpDuration;

    public void Die()
    {
        EventService.PlayerDied();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered with: " + other.gameObject.name);
        if (other.gameObject.CompareTag("PowerUp"))
        {
            IPowerUps powerUp = other.gameObject.GetComponent<IPowerUps>();
            if (powerUp != null)
            {
                Destroy(other.gameObject);
                ApplyPowerUp(powerUp);
            }
            else
            {
                Debug.LogWarning("No IPowerUps component found on: " + other.gameObject.name);
            }
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
}
