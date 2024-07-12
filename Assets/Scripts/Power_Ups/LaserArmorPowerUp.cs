using UnityEngine;

public class LaserProtectionGearPowerUp : MonoBehaviour, IPowerUps
{
    private float duration = 30f;
    private Color[] originalColors;
    private Renderer[] renderers;

    public void ApplyPowerUp(Player player)
    {
        player.HasLaserProtectionGear = true;
        Debug.Log("Laser protection gear applied");

        // Find all Renderer components in the player and its children
        renderers = player.GetComponentsInChildren<Renderer>();

        // Store original colors and change to red, excluding the shield
        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].gameObject.CompareTag("Shield"))
                continue;

            originalColors[i] = renderers[i].material.color;
            renderers[i].material.color = Color.red; // Change color to red
        }
    }

    public void RemovePowerUp(Player player)
    {
        player.HasLaserProtectionGear = false;
        Debug.Log("Laser protection gear removed");

        // Revert colors to original, excluding the shield
        if (renderers != null)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = originalColors[i];
            }
        }
    }

    public float PowerUpDuration()
    {
        return duration;
    }
}
