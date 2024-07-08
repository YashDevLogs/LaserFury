using Assets.Scripts.Utlities;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawnManager : GenericMonoSingleton<PowerUpSpawnManager>
{
    private float powerUpSpawnInterval = 10f;
    [SerializeField] private Transform[] powerUpSpawnPoints;
    private PowerUpFactory powerUpFactory;
    public List<GameObject> activePowerUps = new List<GameObject>();

    private float timer;

    private void Start()
    {
        powerUpFactory = new PowerUpFactory();
        timer = powerUpSpawnInterval;
    }

    private void Update()
    {
        if (!GameManager.Instance.GameEnded)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                SpawnRandomPowerUp();
                timer = powerUpSpawnInterval;
            }
        }
    }

    public void SpawnRandomPowerUp()
    {
        PowerupTypes[] powerUpTypes = (PowerupTypes[])System.Enum.GetValues(typeof(PowerupTypes));
        PowerupTypes randomPowerUp = powerUpTypes[Random.Range(0, powerUpTypes.Length)];
        Transform spawnPoint = powerUpSpawnPoints[Random.Range(0, powerUpSpawnPoints.Length)];

        GameObject powerUp = powerUpFactory.CreatePowerUp(randomPowerUp);
        if (powerUp != null)
        {
            powerUp.transform.position = spawnPoint.position;
            activePowerUps.Add(powerUp);
        }
    }
}
