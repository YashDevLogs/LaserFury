using Assets.Scripts.Utlities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : GenericMonoSingleton<GameManager>
{
    [SerializeField] private Transform playerPrefab;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Player player;
    public bool GameEnded = false;


    private void Start()
    {
        EventService.OnPlayerDeath += GameOver;
    }

 
    private void OnDisable()
    {
        EventService.OnPlayerDeath -= GameOver;
    }

    public void GameWon()
    {
        if (!GameEnded)
        {
            GameEnded = true;
            Debug.Log("Game Won!");
            UIManager.Instance.gameWonScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void GameOver()
    {
        if (!GameEnded)
        {
            GameEnded = true;
            Debug.Log("Game Over!");
            UIManager.Instance.gameOverScreen.SetActive(true);
            StopAllCoroutines();
            Time.timeScale = 0;
        }
    }

    public void RestartGame()
    {
        GameEnded = false;
        UIManager.Instance.gameOverScreen.SetActive(false);
        UIManager.Instance.gameWonScreen.SetActive(false);

        Time.timeScale = 1;

        foreach (LaserController laserController in WaveManager.Instance.activeLaserControllers)
        {
            laserController.StopLaser();
            Destroy(laserController.gameObject);
        }
        WaveManager.Instance.activeLaserControllers.Clear();

        foreach (GameObject powerUp in PowerUpSpawnManager.Instance.activePowerUps)
        {
            Destroy(powerUp);
        }
        PowerUpSpawnManager.Instance.activePowerUps.Clear();

        Destroy(player.gameObject);

        player = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation).GetComponent<Player>();
        EventService.OnPlayerDeath += GameOver;

        WaveManager.Instance.ResetWaveManager();
        StartCoroutine(WaveManager.Instance.StartWaveWithCountdown(3));
        StartCoroutine(PowerUpSpawnManager.Instance.SpawnPowerUps());
    }
}
