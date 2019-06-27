﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PT2Script : EnemyBaseClass
{
    // To randomise the data values
    [System.Serializable]
    public class RandomRangeValue
    {
        public float current = 0.0f;
        public float min = 0.0f;
        public float max = 0.0f;
    }
    // The States of this enemy
    enum STATES
    {
        S_NORMAL,
        S_CLOSE
    }
    STATES currentState = STATES.S_NORMAL;
    [Header("Pixie Type 2 Class")]
    [SerializeField]    // The Center point to rotate around 
    Vector2 centerPoint = Vector2.zero;
    [SerializeField]    // The Radius of the Circle to start with
    float circleRadius = 10.0f;

    [SerializeField]    // How often to decrease the radius
    RandomRangeValue radiusDecreaseTime_Range = new RandomRangeValue();
    float radiusDecreaseTimer = 0.0f;
    [SerializeField]    // How much to decrease the radius
    float radiusDecreaseRate = 1.0f;
    [SerializeField]    // The Minimum radius before we change to darting around the egg
    RandomRangeValue minRadius_Range = new RandomRangeValue();

    [Header("Darting")]
    [SerializeField]    // How often to dart around the egg
    float dartingTime = 1.0f;
    float dartingTimer = 0.0f;
    float dartSpeed = 0.0f;

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
        // Reset Timers
        dartingTimer = dartingTime;
        radiusDecreaseTimer = radiusDecreaseTime_Range.current;
    }



    // Update is called once per frame
    void Update()
    {
        UpdateStates();
    }
    // Update our State Machine
    void UpdateStates()
    {
        switch (currentState)
        {
            case STATES.S_NORMAL:
                {
                    // Move as usual
                    Move();

                    // Check if we need to change state
                    if (circleRadius < minRadius_Range.current)
                    {
                        // Only change state if we reached final destination
                        if (!ReachedTarget(0.1f))
                            return;

                        currentState = STATES.S_CLOSE;
                    }
                    else
                    {
                        // Check if we need to get new target
                        if (ReachedTarget())
                        {
                            // Count down the timer
                            radiusDecreaseTimer -= Time.deltaTime;
                            if (radiusDecreaseTimer < 0.0f)
                            {
                                circleRadius -= radiusDecreaseRate;
                                // Reset Timers
                                radiusDecreaseTimer = radiusDecreaseTime_Range.current;
                            }
                            // Get the next target
                            CalNextCirPos();
                        }
                    }
                }
                break;
            case STATES.S_CLOSE:
                {
                    // If haven't reached position, move
                    if (!ReachedTarget(0.5f))
                    {
                        DartToTarget();
                        return;
                    }
                        
                    // Decrement Timer
                    dartingTimer -= Time.deltaTime;
                    if (dartingTimer < 0.0f)
                    {
                        // Find new target
                        SetNewDartTarget();
                        // Reset Timer
                        dartingTimer = dartingTime;
                    }
                }
                break;
        }
    }



    // Calculates the next position for the rotating movement
    // Using radius, Sine and Cos
    // Sets data to moveTargetPos
    void CalNextCirPos()
    {
        // Calculate next target position
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
                currentRadAngle += 0.1f;        
                break;
            case DIRECTION.D_RIGHT:
                currentRadAngle -= 0.1f;
                break;
        }
        // Wrap angle
        currentRadAngle = Wrap(currentRadAngle, 0.0f, 6.28319f);
    }


    // Set a new darting target
    void SetNewDartTarget()
    {
        // Change the angle
        currentRadAngle += 3.1415f;//Random.Range(2.1f, 4.2f);
        // Calculate next target position
        moveTargetPos.x = centerPoint.x + Mathf.Cos(currentRadAngle) * circleRadius;
        moveTargetPos.y = centerPoint.y + Mathf.Sin(currentRadAngle) * circleRadius;
        // set the new speed
        dartSpeed = (moveTargetPos - myRb2D.position).sqrMagnitude * 0.45f;
    }
    // Move towards the dart target
    void DartToTarget()
    {
        myRb2D.MovePosition(myRb2D.position + (moveTargetPos - myRb2D.position).normalized * dartSpeed * Time.deltaTime);
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
        // Reset Timers
        radiusDecreaseTimer = radiusDecreaseTime_Range.current;

        // Set the new Position
        transform.position = newPos;
        myRb2D.position = newPos;

        // Hmm maybe can randomise the radius modify rates
        RandomiseData();        
    }
    #endregion
    // randomise data values 
    void RandomiseData()
    {
        // Randomise Direction
        int max = (int)DIRECTION.D_RIGHT;
        max = Random.Range(0, max+1);
        rotatingDir = (DIRECTION)max;

        // Radius Decreasing
        radiusDecreaseTime_Range.current = Random.Range(radiusDecreaseTime_Range.min, radiusDecreaseTime_Range.max);

        // Minimum Radius before darting
        minRadius_Range.current = Random.Range(minRadius_Range.min, minRadius_Range.max);
    }
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