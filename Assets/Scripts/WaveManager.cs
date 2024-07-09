using Assets.Scripts.Utlities;
using System.Collections;
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

    private Coroutine waveTimerCoroutine;
    private Coroutine waveDurationCoroutine;

    private void Start()
    {
        laserFactory = new LaserFactory(laserPrefab, 10);
        StartCoroutine(StartWaveWithCountdown(3));
    }

    private void Update()
    {
        foreach (var laserController in activeLaserControllers)
        {
            laserController.UpdateLaser();
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

        waveTimerCoroutine = StartCoroutine(WaveTimer());
        waveDurationCoroutine = StartCoroutine(CountdownWaveDuration());
    }

    public IEnumerator StartWaveWithCountdown(int countdown)
    {
        UIManager.Instance.waveText.text = "Wave " + (currentWaveIndex + 1);
        UIManager.Instance.waveText.gameObject.SetActive(true);

        UIManager.Instance.countdownText.gameObject.SetActive(true);
        for (int i = countdown; i > 0; i--)
        {
            UIManager.Instance.countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        UIManager.Instance.countdownText.text = "Start!";
        yield return new WaitForSeconds(1f);
        UIManager.Instance.countdownText.gameObject.SetActive(false);
        UIManager.Instance.waveText.gameObject.SetActive(false);

        StartWave();
    }

    private IEnumerator CountdownWaveDuration()
    {
        float timer = waveDuration;
        while (timer >= 0f)
        {
            UIManager.Instance.waveTimerText.gameObject.SetActive(true);
            int secondsRemaining = Mathf.CeilToInt(timer);
            UIManager.Instance.waveTimerText.text = "Time Remaining: " + secondsRemaining.ToString();
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

        StartCoroutine(HandleWaveCompletion());
    }

    private IEnumerator HandleWaveCompletion()
    {
        yield return new WaitForSeconds(2f);

        UIManager.Instance.waveText.gameObject.SetActive(false);
        UIManager.Instance.nextWaveText.gameObject.SetActive(false);

        if (currentWaveIndex < waves.Length)
        {
            StartCoroutine(StartWaveWithCountdown(3));
        }
        else
        {
            GameManager.Instance.GameWon();
        }
    }

    public void ResetWaveManager()
    {
        currentWaveIndex = 0;
        if (waveTimerCoroutine != null)
        {
            StopCoroutine(waveTimerCoroutine);
        }
        if (waveDurationCoroutine != null)
        {
            StopCoroutine(waveDurationCoroutine);
        }
        waveDuration = 10f;
    }
}
