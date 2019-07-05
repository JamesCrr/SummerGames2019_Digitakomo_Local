
using UnityEngine;

public class FlameProjectile : RangeWeapon
{
    private Vector3 defaultScale;
    public float ExpandSize = 0.05f;

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
            transform.localScale += new Vector3(ExpandSize, ExpandSize, 0);
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
