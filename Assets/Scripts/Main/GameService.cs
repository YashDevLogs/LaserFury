using UnityEngine;

public class GameService
{
    private Transform playerPrefab;
    private Transform playerSpawnPoint;
    private Player player;
    public bool GameEnded = false;

    public GameService(Transform playerPrefab, Transform playerSpawnPoint)
    {
        this.playerPrefab = playerPrefab;
        this.playerSpawnPoint = playerSpawnPoint;
        EventService.OnPlayerDeath += GameOver;
    }

    ~GameService()
    {
        EventService.OnPlayerDeath -= GameOver;
    }

    public void GameWon()
    {
        if (!GameEnded)
        {
            GameEnded = true;
            Debug.Log("Game Won!");
            ServiceLocator.Instance.UIManager.GameWonScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void GameOver()
    {
        if (!GameEnded)
        {
            GameEnded = true;
            Debug.Log("Game Over!");
            ServiceLocator.Instance.UIManager.GameOverScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }

    // Spawns a new player Game Object after restarting the game as previous player gets destroyed.
    private void SpawnPlayer()
    {
        player = GameObject.Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation).GetComponent<Player>();
        EventService.OnPlayerDeath += GameOver;
    }

    public void RestartGame()
    {
        GameEnded = false;
        ServiceLocator.Instance.UIManager.GameOverScreen.SetActive(false);
        ServiceLocator.Instance.UIManager.GameWonScreen.SetActive(false);

        Time.timeScale = 1;

        // Clear existing lasers
        foreach (LaserController laserController in ServiceLocator.Instance.WaveManager.ActiveLaserControllers)
        {
            laserController.StopLaser();
            GameObject.Destroy(laserController.LaserView.gameObject);
        }
        ServiceLocator.Instance.WaveManager.ActiveLaserControllers.Clear();

        // Destroy existing player and respawn
         SpawnPlayer();
         GameObject.Destroy(player.gameObject);
        

        // Reset and start new wave
        ServiceLocator.Instance.WaveManager.ResetWaveManager();
        ServiceLocator.Instance.WaveManager.StartWaveWithCountdown(3);
        ServiceLocator.Instance.PowerUpSpawnManager.SpawnRandomPowerUp();
    }

   
}
