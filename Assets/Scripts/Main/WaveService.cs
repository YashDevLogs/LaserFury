using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveService
{
    private WaveData_SO[] waves; // Scriptable objects having configuration of each wave.
    private int currentWaveIndex = 0;
    private LaserView laserPrefab;
    private LaserFactory laserFactory;

    private List<LaserController> activeLaserControllers = new List<LaserController>(); // List of all active laser for the current wave. 
    public List<LaserController> ActiveLaserControllers => activeLaserControllers; // need reference of this list in Game Manager.

    private float waveDuration = 10f;
    private float countdownTime = 0f;
    private float waveTimeRemaining = 0f;
    private bool isWaveActive = false;
    private bool isCountdownActive = false;
    private bool isWaveCompletePending = false;

    public WaveService(WaveData_SO[] waves, LaserView laserPrefab)
    {
        this.waves = waves;
        this.laserPrefab = laserPrefab;

        Initialize();
    }

    private void Initialize()
    {
        laserFactory = new LaserFactory(laserPrefab, 10); // creates a Objectpool of LaserPrefabs.
        StartWaveWithCountdown(3);
    }

    public void Update()
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
            ServiceLocator.Instance.GameManager.GameWon();
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

            LaserModel laserModel = new LaserModel(currentWave.laserRange, currentWave.rotationSpeed, currentWave.rotationRange); // Each wave has Different Rotation Speed and Range for Laser, Hence updating the values for each waves.
            LaserView laserView = laserObj.GetComponent<LaserView>();
            LaserController laserController = new LaserController(laserModel, laserView, laserObj.transform); 
            // Starts all laser prefabs
            laserController.StartLaser();
            // Adds active laser in the scene to the list of laser controllers.
            activeLaserControllers.Add(laserController);
        }

        waveTimeRemaining = waveDuration;
        isWaveActive = true;
    }

    public void StartWaveWithCountdown(int countdown)
    {
        ServiceLocator.Instance.UIManager.WaveText.text = "Wave " + (currentWaveIndex + 1);
        ServiceLocator.Instance.UIManager.WaveText.gameObject.SetActive(true);

        ServiceLocator.Instance.UIManager.CountdownText.gameObject.SetActive(true);
        countdownTime = countdown;
        isCountdownActive = true;
    }

    private void HandleCountdown()
    {
        if (countdownTime > 0)
        {
            countdownTime -= Time.deltaTime;
            ServiceLocator.Instance.UIManager.CountdownText.text = Mathf.CeilToInt(countdownTime).ToString();
        }
        else
        {
            ServiceLocator.Instance.UIManager.CountdownText.text = "Start!";
            isCountdownActive = false;
            ServiceLocator.Instance.UIManager.CountdownText.gameObject.SetActive(false);
            ServiceLocator.Instance.UIManager.WaveText.gameObject.SetActive(false);
            StartWave();
        }
    }

    private void HandleWaveDuration()
    {
        if (waveTimeRemaining > 0)
        {
            waveTimeRemaining -= Time.deltaTime;
            ServiceLocator.Instance.UIManager.WaveTimerText.text = "Time Remaining: " + Mathf.CeilToInt(waveTimeRemaining).ToString();
            ServiceLocator.Instance.UIManager.WaveTimerText.gameObject.SetActive(true);
        }
        else
        {
            isWaveActive = false;
            OnWaveCompleted();
        }
    }

    private void OnWaveCompleted()
    {
        ServiceLocator.Instance.UIManager.WaveText.text = "Wave " + (currentWaveIndex + 1) + " Completed!";
        ServiceLocator.Instance.UIManager.WaveText.gameObject.SetActive(true);

        foreach (LaserController laserController in activeLaserControllers)
        {
            laserController.StopLaser();
            GameObject.Destroy(laserController.LaserView.gameObject); // Destroy the LaserView game object
        }
        activeLaserControllers.Clear();

        currentWaveIndex++;
        ServiceLocator.Instance.UIManager.NextWaveText.text = "Next Wave: " + (currentWaveIndex + 1);
        ServiceLocator.Instance.UIManager.NextWaveText.gameObject.SetActive(true);

        isWaveCompletePending = true;
        CoroutineHelper.Instance.StartHelperCoroutine(WaitAndHandleWaveCompletion(2f));
    }

    private IEnumerator WaitAndHandleWaveCompletion(float delay)
    {
        yield return new WaitForSeconds(delay);
        HandleWaveCompletion();
    }

    private void HandleWaveCompletion()
    {
        ServiceLocator.Instance.UIManager.WaveText.gameObject.SetActive(false);
        ServiceLocator.Instance.UIManager.NextWaveText.gameObject.SetActive(false);

        if (currentWaveIndex < waves.Length)
        {
            StartWaveWithCountdown(3);
        }
        else
        {
            ServiceLocator.Instance.GameManager.GameWon();
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
