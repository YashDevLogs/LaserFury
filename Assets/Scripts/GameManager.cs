using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public WaveData_SO[] waves;
    public Transform playerPrefab;
    public Transform playerSpawnPoint;
    public GameObject laserPrefab;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI countdownText;
    public GameObject gameOverScreen;
    public GameObject gameWonScreen;

    private Transform player;
    private int currentWaveIndex = 0;
    private int lasersCompleted = 0;
    private bool gameEnded = false;

    private void Start()
    {
        gameOverScreen.SetActive(false);
        gameWonScreen.SetActive(false);

        StartWave();
    }

    private void StartWave()
    {
        if (currentWaveIndex >= waves.Length)
        {
            // Player wins the game
            gameWonScreen.SetActive(true);
            return;
        }

        WaveData_SO currentWave = waves[currentWaveIndex];
        lasersCompleted = 0;

        for (int i = 0; i < currentWave.numberOfLasers; i++)
        {
            Vector3 spawnPosition = currentWave.spawnPositions[i];
            Quaternion initialRotation = Quaternion.Euler(currentWave.initialRotations[i]);
            GameObject laserObj = Instantiate(laserPrefab, spawnPosition, initialRotation);
            Laser laser = laserObj.GetComponent<Laser>();
            laser.OnLaserCompleted += OnLaserCompleted;
            laser.OnPlayerHit += GameOver; // Subscribe to the player hit event
            laser.InitializeLaser(currentWave.rotationSpeed, currentWave.rotationRange);
        }

        StartCoroutine(StartWaveWithCountdown(3));
    }

    private IEnumerator StartWaveWithCountdown(int countdown)
    {
        countdownText.gameObject.SetActive(true);
        for (int i = countdown; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        countdownText.text = "Start!";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);

        // Start lasers
        foreach (Laser laser in FindObjectsOfType<Laser>())
        {
            laser.StartLaser();
        }
    }

    private void OnLaserCompleted()
    {
        lasersCompleted++;
        if (lasersCompleted >= waves[currentWaveIndex].numberOfLasers)
        {
            StartCoroutine(HandleWaveCompletion());
        }
    }

    private IEnumerator HandleWaveCompletion()
    {
        waveText.text = "Wave " + (currentWaveIndex + 1) + " Completed!";
        waveText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        waveText.gameObject.SetActive(false);

        foreach (Laser laser in FindObjectsOfType<Laser>())
        {
            Destroy(laser.gameObject);
        }

        waveText.text = "Wave " + (currentWaveIndex + 2);
        waveText.gameObject.SetActive(true);

        // Wait for a few seconds
        yield return new WaitForSeconds(2f);

        // Hide wave completion UI
        waveText.gameObject.SetActive(false);

        // Move to the next wave
        currentWaveIndex++;
        StartWave();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !gameEnded)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        if (!gameEnded)
        {
            gameEnded = true;
            Debug.Log("Game Over!");
            gameOverScreen.SetActive(true);
            StopAllCoroutines();
            Time.timeScale = 0;
        }
    }

    public void RestartGame()
    {
        Debug.Log("Restart button clicked");
        gameEnded = false;
        currentWaveIndex = 0;
        gameOverScreen.SetActive(false);
        gameWonScreen.SetActive(false);

        Time.timeScale = 1;

        // Destroy all existing lasers before restarting
        foreach (Laser laser in FindObjectsOfType<Laser>())
        {
            Destroy(laser.gameObject);
        }

        // Destroy existing player instance
        if (player != null)
        {
            Destroy(player.gameObject);
        }

        // Respawn the player
        player = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);

        StartWave();
    }
}
