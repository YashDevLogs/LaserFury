using Assets.Scripts.Utlities;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : GenericMonoSingleton<WaveManager>
{
    [SerializeField] private WaveData_SO[] waves;
    public int currentWaveIndex = 0;
    [SerializeField] private LaserView laserPrefab;
    private LaserFactory laserFactory;

    public List<LaserView> activeLaserViews = new List<LaserView>();
    private List<LaserController> activeLaserControllers = new List<LaserController>();

    public float waveDuration = 10f;
    private float countdownTime = 0f;
    private float waveTimeRemaining = 0f;
    private bool isWaveActive = false;
    private bool isCountdownActive = false;
    private bool isWaveCompletePending = false;

    private void Start()
    {
        laserFactory = new LaserFactory(laserPrefab, 10);
        StartWaveWithCountdown(3);
    }

    private void Update()
    {
        foreach (var laserController in activeLaserControllers)
        {
            laserController.UpdateLaser();
        }

        if (isCountdownActive)
        {
            HandleCountdown();
        }
        else if (isWaveActive)
        {
            HandleWaveDuration();
        }
        else if (isWaveCompletePending)
        {
            HandleWaveCompletion();
        }
    }

    private void StartWave()
    {
        if (currentWaveIndex >= waves.Length)
        {
            GameManager.Instance.GameWon();
            return;
        }

        WaveData_SO currentWave = waves[currentWaveIndex];

        for (int i = 0; i < currentWave.numberOfLasers; i++)
        {
            Vector3 spawnPosition = currentWave.spawnPositions[i];
            Quaternion initialRotation = Quaternion.Euler(currentWave.initialRotations[i]);
            LaserView laserObj = laserFactory.GetLaser();
            laserObj.transform.position = spawnPosition;
            laserObj.transform.rotation = initialRotation;

            LaserModel laserModel = new LaserModel(currentWave.laserRange, currentWave.rotationSpeed, currentWave.rotationRange);
            LaserView laserView = laserObj.GetComponent<LaserView>();
            LaserController laserController = new LaserController(laserModel, laserView, laserObj.transform);
            laserView.StartLaser();

            activeLaserViews.Add(laserView);
            activeLaserControllers.Add(laserController);
        }

        waveTimeRemaining = waveDuration;
        isWaveActive = true;
    }

    public void StartWaveWithCountdown(int countdown)
    {
        UIManager.Instance.waveText.text = "Wave " + (currentWaveIndex + 1);
        UIManager.Instance.waveText.gameObject.SetActive(true);

        UIManager.Instance.countdownText.gameObject.SetActive(true);
        countdownTime = countdown;
        isCountdownActive = true;
    }

    private void HandleCountdown()
    {
        if (countdownTime > 0)
        {
            countdownTime -= Time.deltaTime;
            UIManager.Instance.countdownText.text = Mathf.CeilToInt(countdownTime).ToString();
        }
        else
        {
            UIManager.Instance.countdownText.text = "Start!";
            isCountdownActive = false;
            UIManager.Instance.countdownText.gameObject.SetActive(false);
            UIManager.Instance.waveText.gameObject.SetActive(false);
            StartWave();
        }
    }

    private void HandleWaveDuration()
    {
        if (waveTimeRemaining > 0)
        {
            waveTimeRemaining -= Time.deltaTime;
            UIManager.Instance.waveTimerText.text = "Time Remaining: " + Mathf.CeilToInt(waveTimeRemaining).ToString();
            UIManager.Instance.waveTimerText.gameObject.SetActive(true);
        }
        else
        {
            isWaveActive = false;
            OnWaveCompleted();
        }
    }

    private void OnWaveCompleted()
    {
        UIManager.Instance.waveText.text = "Wave " + (currentWaveIndex + 1) + " Completed!";
        UIManager.Instance.waveText.gameObject.SetActive(true);

        foreach (LaserView laserView in activeLaserViews)
        {
            laserView.StopLaser();
            Destroy(laserView.gameObject);
        }
        activeLaserViews.Clear();
        activeLaserControllers.Clear();

        currentWaveIndex++;
        UIManager.Instance.nextWaveText.text = "Next Wave: " + (currentWaveIndex + 1);
        UIManager.Instance.nextWaveText.gameObject.SetActive(true);

        isWaveCompletePending = true;
        Invoke(nameof(HandleWaveCompletion), 2f);  // Schedule HandleWaveCompletion to run after 2 seconds
    }

    private void HandleWaveCompletion()
    {
        UIManager.Instance.waveText.gameObject.SetActive(false);
        UIManager.Instance.nextWaveText.gameObject.SetActive(false);

        if (currentWaveIndex < waves.Length)
        {
            StartWaveWithCountdown(3);
        }
        else
        {
            GameManager.Instance.GameWon();
        }

        isWaveCompletePending = false;
    }

    public void ResetWaveManager()
    {
        currentWaveIndex = 0;
        isWaveActive = false;
        isCountdownActive = false;
        isWaveCompletePending = false;
        waveDuration = 10f;
    }
}
