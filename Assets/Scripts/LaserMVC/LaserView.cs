using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserView : MonoBehaviour
{
    private LineRenderer laserLine;
    public bool isLaserActive = false;

    private void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
        laserLine.enabled = false;
    }

    public void SetLaserPositions(Vector3 startPosition, Vector3 endPosition)
    {
        laserLine.SetPosition(0, startPosition);
        laserLine.SetPosition(1, endPosition);
    }

    public void EnableLaser(bool enable)
    {
        laserLine.enabled = enable;
    }

    public void StartLaser()
    {
        isLaserActive = true;
        EnableLaser(true);
    }

    public void StopLaser()
    {
        isLaserActive = false;
        EnableLaser(false);
    }

    internal void SetActive(bool v)
    {
        gameObject.SetActive(v);
    }
}