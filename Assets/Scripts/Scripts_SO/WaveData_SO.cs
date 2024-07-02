using UnityEngine;


[CreateAssetMenu(fileName = "NewWave", menuName = "ScriptableObjects/WaveData", order = 1)]
public class WaveData_SO : ScriptableObject
{
    public int numberOfLasers;
    public float rotationSpeed;
    public float rotationRange;
    public Vector3[] spawnPositions;
    public Vector3[] initialRotations;
    public float laserRange = 50f; 
}

