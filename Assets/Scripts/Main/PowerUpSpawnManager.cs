using Assets.Scripts.Utlities;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawnManager : MonoBehaviour
{
    private float powerUpSpawnInterval = 10f;
    [SerializeField] private Transform[] powerUpSpawnPoints;
    [SerializeField] private PowerUpDatabase powerUpDatabase;
    private PowerUpFactory powerUpFactory;

    private float timer;
/*
    public PowerUpSpawnManager(Transform[] powerUpSpawnPoints, PowerUpDatabase powerUpDatabase )
    {
        this.powerUpDatabase = powerUpDatabase;
        this.powerUpSpawnPoints = powerUpSpawnPoints;
    }*/

    private void Start()
    {
        powerUpFactory = new PowerUpFactory(powerUpDatabase);
        timer = powerUpSpawnInterval;
    }

    public void Update()
    {
        if (!ServiceLocator.Instance.GameManager.GameEnded)  
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
        }
    }
}
