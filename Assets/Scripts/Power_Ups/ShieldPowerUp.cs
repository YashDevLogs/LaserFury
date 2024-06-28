using UnityEngine;

public class ShieldPowerUp : MonoBehaviour, IPowerUps
{
    public float duration = 10f;

    public void ApplyPowerUp(Player player)
    {
        player.HasShield = true;
        Debug.Log("Shield applied");
    }

    public void RemovePowerUp(Player player)
    {
        player.HasShield = false;
        Debug.Log("Shield removed");
    }

    public float PowerUpDuration()
    {
        return duration;
    }
}
