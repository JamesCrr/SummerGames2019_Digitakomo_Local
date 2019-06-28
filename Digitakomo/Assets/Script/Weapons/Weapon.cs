using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Range(0, 50)]
    public float MinDamage;
    public float MaxDamage;
    public AttackType at = AttackType.UNKNOWN;

    // Condenses attack types into
    // FIRE, WATER or UNKNOWN for easier comparision
    public AttackType GetMainType()
    {
        switch (at)
        {
            // Unknown
            case AttackType.UNKNOWN:
                return AttackType.UNKNOWN;
            // Ice
            case AttackType.ICE:
                return AttackType.ICE;
            case AttackType.ICE_JUMP:
                return AttackType.ICE;
            // Fire
            case AttackType.FIRE:
                return AttackType.FIRE;
            case AttackType.FIRE_JUMP:
                return AttackType.FIRE;
        }
        return AttackType.UNKNOWN;
    }
}
