using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    public Transform LaserSpawn;
    public float LaserRange = 50f;
    public float RotationSpeed = 100f;
    public float RotationRange = 45f; // Half of the full range for right and left rotation

    private LineRenderer laserLine;
    private Quaternion initialRotation;
    private bool rotatingRight = true;
    private bool rotatingLeft = false;
    private bool returningToInitial = false;
    private float currentRotationAngle = 0f;

    private void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
        laserLine.enabled = false;
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(ShootLaser());
        }
    }

    private IEnumerator ShootLaser()
    {
        laserLine.enabled = true;

        while (true)
        {
            // Rotate the laser GameObject right
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
            // Rotate the laser GameObject left
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
            // Return to initial position
            else if (returningToInitial)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, initialRotation, RotationSpeed * Time.deltaTime);
                currentRotationAngle = Quaternion.Angle(transform.rotation, initialRotation);

                if (currentRotationAngle <= 0.1f)
                {
                    transform.rotation = initialRotation;
                    laserLine.enabled = false;
                    yield break; // Stop the coroutine
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
                    Destroy(hit.collider.gameObject); // Destroy the player
                    // GameManager.instance.GameOver(); // Handle game over
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
