using Assets.Scripts.Utlities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : GenericMonoSingleton<ServiceLocator>  
{
    public GameManager GameManager;
    public UIManager UIManager;
    public WaveManager WaveManager;
    public PowerUpSpawnManager PowerUpSpawnManager;


/*    [Header("GameManager References")]
    [SerializeField] private Transform playerPrefab;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Player player;

    [Header("PowerUpSpawnManager References")]
    [SerializeField] private Transform[] powerUpSpawnPoints;
    [SerializeField] private PowerUpDatabase powerUpDatabase;*/

/*    private void Awake()
    {
*//*        GameManager = new GameManager(playerPrefab, playerSpawnPoint, player);*/
/*        PowerUpSpawnManager = new PowerUpSpawnManager(powerUpSpawnPoints, powerUpDatabase);*//*
    }*/


    private void Update()
    {
     /*   PowerUpSpawnManager.Update();*/
    }

/*    private void OnDisable()
    {
        GameManager.OnDisable();
    }*/

}
