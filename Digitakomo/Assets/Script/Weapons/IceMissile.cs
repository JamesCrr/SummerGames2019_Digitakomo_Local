using UnityEngine;

public class IceMissile : RangeWeapon
{
    public GameObject partical;

    public override void Restart()
    {
        base.Restart();
        SoundManager.instance.PlaySound("IceMissile");
    }

    public override void SetRotation(int rotation)
    {
        base.SetRotation(rotation);
        partical.transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
}
