using UnityEngine;
using System.Collections.Generic;


public class LaserFactory
{
    private LaserView laserPrefab;
    private Queue<LaserView> laserPool;

    public LaserFactory(LaserView laserPrefab, int initialSize)
    {
        this.laserPrefab = laserPrefab;
        laserPool = new Queue<LaserView>();

        for (int i = 0; i < initialSize; i++)
        {
            LaserView laser = GameObject.Instantiate(laserPrefab);
            laser.SetActive(false);
            laserPool.Enqueue(laser);
        }
    }

    public LaserView GetLaser()
    {
        if (laserPool.Count > 0)
        {
            LaserView laser = laserPool.Dequeue();
            laser.SetActive(true);
            return laser;
        }
        else
        {
            LaserView laser = GameObject.Instantiate(laserPrefab);
            return laser;
        }
    }

    public void ReturnLaser(LaserView laser)
    {
        laser.SetActive(false);
        laserPool.Enqueue(laser);
    }
}
