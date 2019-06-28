
using UnityEngine;

public class FlameProjectile : RangeWeapon
{
    private Vector3 defaultScale;

    protected override void Awake()
    {
        base.Awake();
        defaultScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    void Start()
    {
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (isActiveAndEnabled)
        {
            transform.localScale += new Vector3(0.05f, 0.05f, 0);
        }
    }

    public override void Restart()
    {
        base.Restart();
        this.transform.localScale = defaultScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Steam>() != null)
        {
            // hit steam
            gameObject.SetActive(false);
        }
    }
}
