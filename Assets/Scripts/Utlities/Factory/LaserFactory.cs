using UnityEngine;
using System.Collections.Generic;

public class LaserFactory
{
    private GameObject laserPrefab;
    private Queue<GameObject> laserPool;

    public LaserFactory(GameObject laserPrefab, int initialSize)
    {
        this.laserPrefab = laserPrefab;
        laserPool = new Queue<GameObject>();

        for (int i = 0; i < initialSize; i++)
        {
            GameObject laser = GameObject.Instantiate(laserPrefab);
            laser.SetActive(false);
            laserPool.Enqueue(laser);
        }
    }

    public GameObject GetLaser()
    {
        if (laserPool.Count > 0)
        {
            GameObject laser = laserPool.Dequeue();
            laser.SetActive(true);
            return laser;
        }
        else
        {
            GameObject laser = GameObject.Instantiate(laserPrefab);
            return laser;
        }
    }

    public void ReturnLaser(GameObject laser)
    {
        laser.SetActive(false);
        laserPool.Enqueue(laser);
    }
}
