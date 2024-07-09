using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpDatabase", menuName = "ScriptableObjects/PowerUpDatabase", order = 2)]
public class PowerUpDatabase : ScriptableObject
{
    public List<PowerUpData_SO> powerUps;
}
