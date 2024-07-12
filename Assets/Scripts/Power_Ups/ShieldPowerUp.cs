using UnityEngine;

public class ShieldPowerUp : MonoBehaviour, IPowerUps
{

    private float duration = 10f;

    public void ApplyPowerUp(Player player)
    {
        player.HasShield = true;
        player.Shield.SetActive(true); // sets the Sphere Game Object active in the Scene
    }

    public void RemovePowerUp(Player player)
    {
        player.HasShield = false;
        player.Shield.SetActive(false); //sets the Sphere Game Object inactive in the Scene
    }

    public float PowerUpDuration()
    {
        return duration;
    }
}
