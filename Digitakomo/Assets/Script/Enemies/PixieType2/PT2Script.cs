using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PT2Script : EnemyBaseClass
{

    [Header("Pixie Type 2 Class")]
    [SerializeField]    // The Center point to rotate around 
    Vector2 centerPoint = Vector2.zero;
    [SerializeField]    // The Radius of the Circle to start with
    float circleRadius = 10.0f;
    [SerializeField]    // How fast does the radius decrease every frame
    float radiusReduceRate = 1.0f;
    [SerializeField]    // The Minimum radius before we change to darting around the egg
    float minRadius = 5.0f;

    // The current Angle of rotation
    float currentAngle = 90.0f;


    // Awake
    void Awake()
    {
        myRb2D = GetComponent<Rigidbody2D>();
    }



    // Update is called once per frame
    void Update()
    {
        Move();

        if (ReachedTarget())
            CalNextCirPos();
    }



    // Calculates the next position for the rotating movement
    // Using radius, Sine and Cos
    // Sets data to moveTargetPos
    void CalNextCirPos()
    {
        moveTargetPos.x = centerPoint.x + Mathf.Cos(currentAngle) * circleRadius;
        moveTargetPos.y = centerPoint.y + Mathf.Sin(currentAngle) * circleRadius;
    }




    #region Overriden
    public override void ResetEnemy(SpawnZone newSpawnZone, Vector3 newPos)
    {
        // Get the data from the center point
        centerPoint = Egg.Instance.GetPosition();
        circleRadius = Egg.Instance.GetStartingRadius();
        currentAngle = 90.0f;
        // Recalculate the starting circular Position
        CalNextCirPos();

        // Set the new Position
        transform.position = newPos;
        myRb2D.position = newPos;

    }
    #endregion


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(centerPoint, circleRadius);
    }
}
