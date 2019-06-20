using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseClass : MonoBehaviour
{
    // Stats
    [Header("Stats")]
    [SerializeField]
    protected int hp = 1;
    [SerializeField]
    protected int damage = 1;

    // Spawn Zone
    protected SpawnZone spawningZone;

    // Unity Stuff
    protected Rigidbody2D myRb2D = null;
    protected Vector2 targetPosition = Vector2.zero;
    protected Vector2 direction = Vector2.zero;

    // Called after being Fetched
    public abstract void ResetEnemy(SpawnZone newSpawnZone, Vector3 newPos);

    // Called to Flip Sprite
    public virtual void FlipEnemy()
    {

    }
}
