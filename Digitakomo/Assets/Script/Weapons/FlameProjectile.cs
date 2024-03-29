﻿
using UnityEngine;

public class FlameProjectile : RangeWeapon
{
    private Vector3 defaultScale;
    public float ExpandSize = 0.001f;

    protected override void Awake()
    {
        base.Awake();
        defaultScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        // Destroy(gameObject.GetComponent<Rigidbody2D>());
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
        // this.transform.position = gameObject.GetComponentInParent<Transform>().position;
    }

    protected override void disableProjectile()
    {
        base.disableProjectile();
    }

    public override void Restart()
    {
        base.Restart();
        this.transform.localScale = defaultScale;
    }
}
