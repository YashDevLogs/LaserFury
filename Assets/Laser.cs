using System.Collections;
using UnityEngine;
using System;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    public Transform LaserSpawn;
    public float LaserRange = 50f;
    public float RotationSpeed = 100f;
    public float RotationRange = 45f;

    private LineRenderer laserLine;
    private Quaternion initialRotation;
    private bool rotatingRight = true;
    private bool rotatingLeft = false;
    private bool returningToInitial = false;
    private float currentRotationAngle = 0f;

    public event Action OnLaserCompleted;
    public event Action OnPlayerHit;

    private Coroutine laserCoroutine;

    private void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
        laserLine.enabled = false;
        initialRotation = transform.rotation;
    }

    public void InitializeLaser(float rotationSpeed, float rotationRange)
    {
        RotationSpeed = rotationSpeed;
        RotationRange = rotationRange;
    }

    public void StartLaser()
    {
        laserCoroutine = StartCoroutine(ShootLaser());
    }

    public void StopLaser()
    {
        if (laserCoroutine != null)
        {
            StopCoroutine(laserCoroutine);
            laserLine.enabled = false;
        }
    }

    private IEnumerator ShootLaser()
    {
        laserLine.enabled = true;

        while (true)
        {
            if (rotatingRight)
            {
                float rotationStep = RotationSpeed * Time.deltaTime;
                transform.Rotate(Vector3.up, rotationStep);
                currentRotationAngle += rotationStep;

                if (currentRotationAngle >= RotationRange)
                {
                    rotatingRight = false;
                    rotatingLeft = true;
                }
            }
            else if (rotatingLeft)
            {
                float rotationStep = RotationSpeed * Time.deltaTime;
                transform.Rotate(Vector3.up, -rotationStep);
                currentRotationAngle -= rotationStep;

                if (currentRotationAngle <= -RotationRange)
                {
                    rotatingLeft = false;
                    returningToInitial = true;
                }
            }
            else if (returningToInitial)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, initialRotation, RotationSpeed * Time.deltaTime);
                currentRotationAngle = Quaternion.Angle(transform.rotation, initialRotation);

                if (currentRotationAngle <= 0.1f)
                {
                    transform.rotation = initialRotation;
                    laserLine.enabled = false;
                    OnLaserCompleted?.Invoke();
                    yield break;
                }
            }

            laserLine.SetPosition(0, LaserSpawn.position);
            Vector3 rayOrigin = LaserSpawn.position;
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, LaserSpawn.transform.forward, out hit, LaserRange))
            {
                laserLine.SetPosition(1, hit.point);

                if (hit.collider.CompareTag("Player"))
                {
                    Destroy(hit.collider.gameObject);
                    OnPlayerHit?.Invoke();
                    yield break;
                }
            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + (LaserSpawn.transform.forward * LaserRange));
            }

            yield return null;
        }
    }
}
