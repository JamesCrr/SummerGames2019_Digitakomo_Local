﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseClass : MonoBehaviour
{
    // Enum for direction
    public enum DIRECTION
    {
        D_LEFT,
        D_RIGHT,
    }
    protected DIRECTION facingDirection = DIRECTION.D_RIGHT;

    // Stats
    [Header("Stats")]
    [SerializeField]
    protected int hp = 1;
    [SerializeField]
    protected int damage = 1;
    [SerializeField]
    protected float moveSpeed = 1.0f;

    // Spawn Zone
    protected SpawnZone spawningZone = null;

    // Unity Stuff
    protected Rigidbody2D myRb2D = null;
    protected Vector2 targetPosition = Vector2.zero;
    protected Vector2 direction = Vector2.zero;


    // Called after being Fetched
    public abstract void ResetEnemy(SpawnZone newSpawnZone, Vector3 newPos);

    // Called to move 
    protected virtual void Move()
    {
        direction = targetPosition - myRb2D.position;
        // Flip the enemy?
        FlipEnemy();

        // Move
        myRb2D.MovePosition(myRb2D.position + (direction.normalized * moveSpeed * Time.deltaTime));
    }
    // Called to Flip Sprite
    protected virtual void FlipEnemy()
    {
        // Do we need to switch direction?
        if ((facingDirection == DIRECTION.D_LEFT && direction.x > -0.1f) ||
            (facingDirection == DIRECTION.D_RIGHT && direction.x < 0.0f))
        {
            switch (facingDirection)
            {
                case DIRECTION.D_LEFT:
                    facingDirection = DIRECTION.D_RIGHT;
                    break;
                case DIRECTION.D_RIGHT:
                    facingDirection = DIRECTION.D_LEFT;
                    break;
            }
            // Reverse the Object
            transform.rotation = Quaternion.Inverse(transform.rotation);
        }
    }
}
