using Assets.Scripts.Utlities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform playerPrefab;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Player player;
    public bool GameEnded = false;


/*    public GameManager(Transform playerPrefab, Transform playerSpawnPoint, Player player)    
    { 
        this.playerPrefab = playerPrefab;
        this.playerSpawnPoint = playerSpawnPoint;
        this.player = player;
    
    }*/

    private void Start()
    {
        EventService.OnPlayerDeath += GameOver;
    }

    public void OnDisable()
    {
        EventService.OnPlayerDeath -= GameOver;
    }

    public void GameWon()
    {
        if (!GameEnded)
        {
            GameEnded = true;
            Debug.Log("Game Won!");
            ServiceLocator.Instance.UIManager.gameWonScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void GameOver()
    {
        if (!GameEnded)
        {
            GameEnded = true;
            Debug.Log("Game Over!");
            ServiceLocator.Instance.UIManager.gameOverScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void RestartGame()
    {
        GameEnded = false;
        ServiceLocator.Instance.UIManager.gameOverScreen.SetActive(false);
        ServiceLocator.Instance.UIManager.gameWonScreen.SetActive(false);

        Time.timeScale = 1;

        // Clear existing lasers
        foreach (LaserView laserView in ServiceLocator.Instance.WaveManager.activeLaserViews)
        {
            laserView.StopLaser();
            GameObject.Destroy(laserView.gameObject);
        }
        ServiceLocator.Instance.WaveManager.activeLaserViews.Clear();

        // Destroy existing player and respawn
        GameObject.Destroy(player.gameObject);
        player = GameObject.Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation).GetComponent<Player>();
        EventService.OnPlayerDeath += GameOver;

        // Reset and start new wave
        ServiceLocator.Instance.WaveManager.ResetWaveManager();
        ServiceLocator.Instance.WaveManager.StartWaveWithCountdown(3);
        ServiceLocator.Instance.PowerUpSpawnManager.SpawnRandomPowerUp();
    }

}
