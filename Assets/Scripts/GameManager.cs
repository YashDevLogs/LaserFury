using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public WaveData_SO[] waves;
    public Transform playerPrefab;
    public Transform playerSpawnPoint;
    public GameObject laserPrefab;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI nextWaveText;
    public TextMeshProUGUI countdownText;
    public GameObject gameOverScreen;
    public GameObject gameWonScreen;

    [SerializeField] private Player player;
    private int currentWaveIndex = 0;
    private bool gameEnded = false;
    private float waveDuration = 10f;  // Each wave lasts for 10 seconds

    public Transform[] powerUpSpawnPoints;
    public float powerUpSpawnInterval = 10f;

  

    private void Start()
    {
        gameOverScreen.SetActive(false);
        gameWonScreen.SetActive(false);

        waveText.text = "Wave 1";
        waveText.gameObject.SetActive(true);
        StartCoroutine(StartWaveWithCountdown(3));
        StartCoroutine(SpawnPowerUps());

        player.OnPlayerDeath += GameOver;
    }

    private void OnDisable()
    {
        player.OnPlayerDeath -= GameOver;
    }

    private IEnumerator SpawnPowerUps()
    {
        while (!gameEnded)
        {
            yield return new WaitForSeconds(powerUpSpawnInterval);

            string[] powerUpTypes = { "Shield", "SlowDown", "LaserProtectionGear" };
            string randomPowerUp = powerUpTypes[Random.Range(0, powerUpTypes.Length)];
            Transform spawnPoint = powerUpSpawnPoints[Random.Range(0, powerUpSpawnPoints.Length)];

            Instantiate(PowerUpFactory.CreatePowerUp(randomPowerUp), spawnPoint.position, Quaternion.identity);
        }
    }

    private void StartWave()
    {
        if (currentWaveIndex >= waves.Length)
        {
            gameWonScreen.SetActive(true);
            return;
        }

        WaveData_SO currentWave = waves[currentWaveIndex];

        for (int i = 0; i < currentWave.numberOfLasers; i++)
        {
            Vector3 spawnPosition = currentWave.spawnPositions[i];
            Quaternion initialRotation = Quaternion.Euler(currentWave.initialRotations[i]);
            GameObject laserObj = Instantiate(laserPrefab, spawnPosition, initialRotation);
            Laser laser = laserObj.GetComponent<Laser>();
            laser.InitializeLaser(currentWave.rotationSpeed, currentWave.rotationRange);
            laser.StartLaser();
        }

        StartCoroutine(WaveTimer());
    }

    private IEnumerator StartWaveWithCountdown(int countdown)
    {
        waveText.text = "Wave " + (currentWaveIndex + 1);
        waveText.gameObject.SetActive(true);

        countdownText.gameObject.SetActive(true);
        for (int i = countdown; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        countdownText.text = "Start!";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);
        waveText.gameObject.SetActive(false);

        StartWave();
    }

    private IEnumerator WaveTimer()
    {
        yield return new WaitForSeconds(waveDuration);
        OnWaveCompleted();
    }

    private void OnWaveCompleted()
    {
        waveText.text = "Wave " + (currentWaveIndex + 1) + " Completed!";
        waveText.gameObject.SetActive(true);

        foreach (Laser laser in FindObjectsOfType<Laser>())
        {
            laser.StopLaser();
            Destroy(laser.gameObject);
        }

        currentWaveIndex++;
        nextWaveText.text = "Next Wave: " + (currentWaveIndex + 1);
        nextWaveText.gameObject.SetActive(true);

        StartCoroutine(HandleWaveCompletion());
    }

    private IEnumerator HandleWaveCompletion()
    {
        yield return new WaitForSeconds(2f);

        waveText.gameObject.SetActive(false);
        nextWaveText.gameObject.SetActive(false);

        if (currentWaveIndex < waves.Length)
        {
            StartCoroutine(StartWaveWithCountdown(3));
        }
        else
        {
            gameWonScreen.SetActive(true);
        }
    }

    public void GameOver()
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

        Destroy(player.gameObject);

        // Respawn the player
        player = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation).GetComponent<Player>();
        player.OnPlayerDeath += GameOver;

        StartCoroutine(StartWaveWithCountdown(3));
        StartCoroutine(SpawnPowerUps());
    }
}
