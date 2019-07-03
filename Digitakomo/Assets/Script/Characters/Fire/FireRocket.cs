using UnityEngine;

public class FireRocket : Weapon
{
    private Collider2D rocketCollider;
    // Start is called before the first frame update
    void Start()
    {
        rocketCollider = gameObject.GetComponent<Collider2D>();
    }

    public void SetEnabled(bool isEnabled)
    {
        rocketCollider.enabled = isEnabled;
    }
}
