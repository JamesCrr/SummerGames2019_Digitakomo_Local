﻿using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    public float Energy = 50f;

    public float ExpiredTime = 15f;
    public float WarnningTime = 5f;
    private float createdTime;

    // Start is called before the first frame update
    public virtual void Start()
    {
        Restart();
    }

    protected virtual void FixedUpdate()
    {
        if (Time.time > createdTime + ExpiredTime)
        {
            gameObject.SetActive(false);
        }
    }

    public virtual void Restart()
    {
        createdTime = Time.time;
    }
}