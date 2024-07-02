using UnityEngine;

public class ShieldPowerUp : MonoBehaviour, IPowerUps
{

    public float duration = 10f;

    public void ApplyPowerUp(Player player)
    {
        player.HasShield = true;
        player.Shield.SetActive(true);
    }

    public void RemovePowerUp(Player player)
    {
        player.HasShield = false;
        player.Shield.SetActive(false);
    }

    public float PowerUpDuration()
    {
        return duration;
    }
}
