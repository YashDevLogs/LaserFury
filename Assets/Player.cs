using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool HasShield { get; set; }
    public bool HasLaserProtectionGear { get; set; }


    public delegate void PlayerDeathHandler();
    public event PlayerDeathHandler OnPlayerDeath;

    public void Die()
    {
        OnPlayerDeath?.Invoke();
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
                StartCoroutine(PowerUpCycle(powerUp));
            }
            else
            {
                Debug.LogWarning("No IPowerUps component found on: " + other.gameObject.name);
            }
        }
    }

    private IEnumerator PowerUpCycle(IPowerUps powerUp)
    {
        powerUp.ApplyPowerUp(this);
        yield return new WaitForSeconds(powerUp.PowerUpDuration());
        powerUp.RemovePowerUp(this);
    }
}
