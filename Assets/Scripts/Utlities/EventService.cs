using System;

public static class EventService
{
    public static event Action OnPlayerDeath;

    public static void PlayerDied()
    {
        OnPlayerDeath?.Invoke();
    }
}

