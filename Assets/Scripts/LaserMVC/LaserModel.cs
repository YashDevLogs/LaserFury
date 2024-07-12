public class LaserModel
{
    public float LaserRange { get; private set; }
    public float RotationSpeed { get; private set; }
    public float RotationRange { get; private set; }

    public bool RotatingRight { get; set; } = true;
    public float CurrentRotationAngle { get; set; } = 0f;

    public bool isLaserActive = false;


    // these variables change for each wave so need to have a constructor to update the values according to the wave_SO. 
    public LaserModel(float laserRange, float rotationSpeed, float rotationRange)
    {
        LaserRange = laserRange;
        RotationSpeed = rotationSpeed;
        RotationRange = rotationRange;
    }
}
