using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public WaveData_SO[] waves;
    public Transform playerPrefab;
    public Transform playerSpawnPoint;
    public GameObject laserPrefab;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI nextWaveText;
    public TextMeshProUGUI WaveTimerText;
    public TextMeshProUGUI countdownText;
    public GameObject gameOverScreen;
    public GameObject gameWonScreen;

    [SerializeField] private Player player;
    private int currentWaveIndex = 0;
    private bool gameEnded = false;
    private float waveDuration = 10f;

    public Transform[] powerUpSpawnPoints;
    public float powerUpSpawnInterval = 10f;

    private LaserFactory laserFactory;
    private PowerUpFactory powerUpFactory;

    private List<LaserController> activeLaserControllers = new List<LaserController>();
    private List<GameObject> activePowerUps = new List<GameObject>();

    private void Start()
    {
        laserFactory = new LaserFactory(laserPrefab, 10);
        powerUpFactory = new PowerUpFactory();

        gameOverScreen.SetActive(false);
        gameWonScreen.SetActive(false);

        waveText.text = "Wave 1";
        waveText.gameObject.SetActive(true);
        StartCoroutine(StartWaveWithCountdown(3));
        StartCoroutine(SpawnPowerUps());

        EventService.OnPlayerDeath += GameOver;
    }

    private void OnDisable()
    {
        EventService.OnPlayerDeath -= GameOver;
    }

    private IEnumerator SpawnPowerUps()
    {
        while (!gameEnded)
        {
            yield return new WaitForSeconds(powerUpSpawnInterval);

            string[] powerUpTypes = { "Shield", "SlowDown", "LaserProtectionGear" };
            string randomPowerUp = powerUpTypes[Random.Range(0, powerUpTypes.Length)];
            Transform spawnPoint = powerUpSpawnPoints[Random.Range(0, powerUpSpawnPoints.Length)];

            GameObject powerUp = powerUpFactory.CreatePowerUp(randomPowerUp);
            if (powerUp != null)
            {
                powerUp.transform.position = spawnPoint.position;
                activePowerUps.Add(powerUp);
            }
        }
    }

    private void StartWave()
    {
        if (currentWaveIndex >= waves.Length)
        {
            GameWon();
            return;
        }

        WaveData_SO currentWave = waves[currentWaveIndex];

        for (int i = 0; i < currentWave.numberOfLasers; i++)
        {
            Vector3 spawnPosition = currentWave.spawnPositions[i];
            Quaternion initialRotation = Quaternion.Euler(currentWave.initialRotations[i]);
            GameObject laserObj = laserFactory.GetLaser();
            laserObj.transform.position = spawnPosition;
            laserObj.transform.rotation = initialRotation;

            LaserModel laserModel = new LaserModel(currentWave.laserRange, currentWave.rotationSpeed, currentWave.rotationRange);
            LaserView laserView = laserObj.GetComponent<LaserView>();
            LaserController laserController = laserObj.AddComponent<LaserController>();
            laserController.InitializeLaser(laserModel, laserView, laserObj.transform);
            laserController.StartLaser();

            activeLaserControllers.Add(laserController);
        }

        StartCoroutine(WaveTimer());
        StartCoroutine(CountdownWaveDuration());
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

    private IEnumerator CountdownWaveDuration()
    {
        float timer = waveDuration;
        while (timer >= 0f)
        {
            WaveTimerText.gameObject.SetActive(true);
            int secondsRemaining = Mathf.CeilToInt(timer);
            WaveTimerText.text = "Time Remaining: " + secondsRemaining.ToString();
            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }
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

        foreach (LaserController laserController in activeLaserControllers)
        {
            laserController.StopLaser();
            Destroy(laserController.gameObject);
        }
        activeLaserControllers.Clear();

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
            GameWon();
        }
    }

    private void GameWon()
    {
        if (!gameEnded)
        {
            gameEnded = true;
            Debug.Log("Game Won!");
            gameWonScreen.SetActive(true);
            Time.timeScale = 0;
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

        foreach (LaserController laserController in activeLaserControllers)
        {
            laserController.StopLaser();
            Destroy(laserController.gameObject);
        }
        activeLaserControllers.Clear();

        foreach (GameObject powerUp in activePowerUps)
        {
            Destroy(powerUp);
        }
        activePowerUps.Clear();

        Destroy(player.gameObject);

        player = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation).GetComponent<Player>();
        EventService.OnPlayerDeath += GameOver;

        StartCoroutine(StartWaveWithCountdown(3));
        StartCoroutine(SpawnPowerUps());
    }
}
