using UnityEngine;

public class LaserController
{
    private LaserModel laserModel;
    private LaserView laserView;
    private Transform laserSpawn;

    private bool rotatingRight = true;
    private float currentRotationAngle = 0f;

    public float RotationSpeed { get; private set; }

    public LaserController(LaserModel model, LaserView view, Transform spawnTransform)
    {
        laserModel = model;
        laserView = view;
        laserSpawn = spawnTransform;
        RotationSpeed = laserModel.RotationSpeed;
    }

    public void UpdateLaser()
    {
        if (laserView.isLaserActive)
        {
            ShootLaser();
        }
    }

    private void ShootLaser()
    {
        if (rotatingRight)
        {
            float rotationStep = laserModel.RotationSpeed * Time.deltaTime;
            laserSpawn.Rotate(Vector3.up, rotationStep);
            currentRotationAngle += rotationStep;

            if (currentRotationAngle >= laserModel.RotationRange)
            {
                rotatingRight = false;
            }
        }
        else
        {
            float rotationStep = laserModel.RotationSpeed * Time.deltaTime;
            laserSpawn.Rotate(Vector3.up, -rotationStep);
            currentRotationAngle -= rotationStep;

            if (currentRotationAngle <= -laserModel.RotationRange)
            {
                rotatingRight = true;
            }
        }

        // Check for collisions with the laser beam
        RaycastHit hit;
        if (Physics.Raycast(laserSpawn.position, laserSpawn.forward, out hit, laserModel.LaserRange))
        {
            laserView.SetLaserPositions(laserSpawn.position, hit.point);

            // Check if the collision is with an object tagged as "Player"
            if (hit.collider.CompareTag("Player"))
            {
                Player player = hit.collider.GetComponent<Player>();

                // Ensure the player is not protected by shields or gear
                if (player != null && !player.HasShield && !player.HasLaserProtectionGear)
                {
                    player.Die();
                    laserView.StopLaser();
                }
            }
        }
        else
        {
            // If no collision, extend the laser beam to its maximum range
            laserView.SetLaserPositions(laserSpawn.position, laserSpawn.position + laserSpawn.forward * laserModel.LaserRange);
        }
    }

    private Vector3 CalculateLaserEndPoint()
    {
        Vector3 rayOrigin = laserSpawn.position;
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, laserSpawn.transform.forward, out hit, laserModel.LaserRange))
        {
            return hit.point;
        }
        else
        {
            return rayOrigin + (laserSpawn.transform.forward * laserModel.LaserRange);
        }
    }
}
