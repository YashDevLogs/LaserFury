using Assets.Scripts.Utlities;
using UnityEngine;
using UnityEngine.UI;

public class ServiceLocator : GenericMonoSingleton<ServiceLocator>  
{
    [SerializeField] private UIManager uIManager;
    private GameService gameManager;
    private WaveService waveManager;
    private PowerUpSpawnManager powerUpSpawnManager;
    public UIManager UIManager => uIManager;
    public GameService GameManager => gameManager;
    public PowerUpSpawnManager PowerUpSpawnManager => powerUpSpawnManager;
    public WaveService WaveManager => waveManager;

    [Header("GameManager References")]
    [SerializeField] private Transform playerPrefab;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Player player;
    [SerializeField] private Button RestartBtn;

    [Header("PowerUpSpawnManager References")]
    [SerializeField] private Transform[] powerUpSpawnPoints;
    [SerializeField] private PowerUpDatabase powerUpDatabase;


    [Header("WaveManager References")]
    [SerializeField] private WaveData_SO[] waves;
    [SerializeField] private LaserView laserPrefab;

    private void InjectDependencies()
    {
        gameManager = new GameService(playerPrefab, playerSpawnPoint);
        powerUpSpawnManager = new PowerUpSpawnManager(powerUpSpawnPoints, powerUpDatabase);
        waveManager = new WaveService(waves, laserPrefab);
    }

    private void Start()
    {
        InjectDependencies();
    }

    private void Update()
    {
        PowerUpSpawnManager.Update();
        WaveManager.Update();
    }

    public void ResetGameManager()
    {
        gameManager.RestartGame();
    }
}
