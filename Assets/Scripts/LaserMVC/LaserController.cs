using UnityEngine;

public class LaserController
{
    private LaserModel laserModel;
    private LaserView laserView;
    private Transform laserSpawn;

    public LaserView LaserView => laserView; // reference needed for these in Game manager and Wave manager

    public LaserController(LaserModel model, LaserView view, Transform spawnTransform)
    {
        laserModel = model;
        laserView = view;
        laserSpawn = spawnTransform;
    }

    public void UpdateLaser()
    {
        if (laserModel.isLaserActive)
        {
            ShootLaser();
        }
    }

    private void ShootLaser()
    {
        if (laserModel.RotatingRight)
        {
            float rotationStep = laserModel.RotationSpeed * Time.deltaTime;
            laserSpawn.Rotate(Vector3.up, rotationStep);
            laserModel.CurrentRotationAngle += rotationStep;

            if (laserModel.CurrentRotationAngle >= laserModel.RotationRange)
            {
                laserModel.RotatingRight = false;
            }
        }
        else
        {
            float rotationStep = laserModel.RotationSpeed * Time.deltaTime;
            laserSpawn.Rotate(Vector3.up, -rotationStep);
            laserModel.CurrentRotationAngle -= rotationStep;

            if (laserModel.CurrentRotationAngle <= -laserModel.RotationRange)
            {
                laserModel.RotatingRight = true;
            }
        }

        RaycastHit hit;
        if (Physics.Raycast(laserSpawn.position, laserSpawn.forward, out hit, laserModel.LaserRange))
        {
            laserView.SetLaserPositions(laserSpawn.position, hit.point);

            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null && !damageable.HasShield && !damageable.HasLaserProtectionGear)
            {
                damageable.TakeDamage();
                StopLaser();
            }
        }
        else
        {
            laserView.SetLaserPositions(laserSpawn.position, laserSpawn.position + laserSpawn.forward * laserModel.LaserRange);
        }
    }

    public void StartLaser()
    {
        laserModel.isLaserActive = true;
        laserView.EnableLaser(true);
    }

    public void StopLaser()
    {
        laserModel.isLaserActive = false;
        laserView.EnableLaser(false);
    }
}
