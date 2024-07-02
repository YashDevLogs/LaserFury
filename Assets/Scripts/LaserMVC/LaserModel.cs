public class LaserModel
{
    public float LaserRange { get; private set; }
    public float RotationSpeed { get; private set; }
    public float RotationRange { get; private set; }

    public LaserModel(float laserRange, float rotationSpeed, float rotationRange)
    {
        LaserRange = laserRange;
        RotationSpeed = rotationSpeed;
        RotationRange = rotationRange;
    }
}