using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Range(0, 100)]
    public int MinDamage;
    public int MaxDamage;
    public AttackType at = AttackType.UNKNOWN;

    public virtual int GetActualDamage()
    {
        return Random.Range(MinDamage, MaxDamage);
    }

    // Condenses attack types into
    // FIRE, WATER or UNKNOWN for easier comparision
    public AttackType GetMainType()
    {
        switch (at)
        {
            // Unknown
            case AttackType.UNKNOWN:
                return AttackType.UNKNOWN;
            // Normal
            case AttackType.Normal:
                return AttackType.Normal;
            // Electric
            case AttackType.Electric:
                return AttackType.Electric;
            // Ice
            case AttackType.ICE:
            case AttackType.ICE_JUMP:
                return AttackType.ICE;
            // Fire
            case AttackType.FIRE:
            case AttackType.FIRE_JUMP:
                return AttackType.FIRE;
            case AttackType.STEAM:
                return AttackType.STEAM;
        }
        return AttackType.UNKNOWN;
    }
}
