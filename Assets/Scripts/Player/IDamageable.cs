public interface IDamageable
{
    void TakeDamage();  
    bool HasShield { get; }  
    bool HasLaserProtectionGear { get; }  
}
