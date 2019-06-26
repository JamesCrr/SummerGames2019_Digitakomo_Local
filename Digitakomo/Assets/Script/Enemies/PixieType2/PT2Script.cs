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
    float radiusModifyRate = 0.05f;
    [SerializeField]    // The Minimum radius before we change to darting around the egg
    float minRadius = 5.0f;
    [Header("Direction")]
    [SerializeField]
    DIRECTION rotatingDir = DIRECTION.D_LEFT;

    // The current Angle of rotation
    public float currentRadAngle = 1.5708f;


    // Awake
    void Awake()
    {
        myRb2D = GetComponent<Rigidbody2D>();
        // Move to top of Circle first
        moveTargetPos.x = centerPoint.x + Mathf.Cos(currentRadAngle) * circleRadius;
        moveTargetPos.y = centerPoint.y + Mathf.Sin(currentRadAngle) * circleRadius;
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
        moveTargetPos.x = centerPoint.x + Mathf.Cos(currentRadAngle) * circleRadius;
        moveTargetPos.y = centerPoint.y + Mathf.Sin(currentRadAngle) * circleRadius;
        // Move the Angle
        ModifyAngle();
    }
    // Reduces or Increases the Angle depending on what 
    // Direction we are moving
    void ModifyAngle()
    {
        switch (rotatingDir)
        {
            case DIRECTION.D_LEFT:
                currentRadAngle += radiusModifyRate;        
                break;
            case DIRECTION.D_RIGHT:
                currentRadAngle -= radiusModifyRate;
                break;
        }

        currentRadAngle = Wrap(currentRadAngle, 0.0f, 6.28319f);
    }




    #region Overriden
    public override void ResetEnemy(SpawnZone newSpawnZone, Vector3 newPos)
    {
        // Get the data from the center point
        centerPoint = Egg.Instance.GetPosition();
        circleRadius = Egg.Instance.GetStartingRadius();
        currentRadAngle = 1.5708f;
        // Move to top of Circle again
        moveTargetPos.x = centerPoint.x + Mathf.Cos(currentRadAngle) * circleRadius;
        moveTargetPos.y = centerPoint.y + Mathf.Sin(currentRadAngle) * circleRadius;

        // Set the new Position
        transform.position = newPos;
        myRb2D.position = newPos;

    }
    #endregion
    // Own Wrapping Function
    float Wrap(float value, float min, float max)
    {
        if (value > max)
            value = min;
        if (value < min)
            value = max;
        return value;
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(centerPoint, circleRadius);

        moveTargetPos.x = centerPoint.x + Mathf.Cos(currentRadAngle) * circleRadius;
        moveTargetPos.y = centerPoint.y + Mathf.Sin(currentRadAngle) * circleRadius;
        Gizmos.DrawLine(centerPoint, moveTargetPos);
    }
}
