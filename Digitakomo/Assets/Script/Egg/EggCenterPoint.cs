using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggCenterPoint : MonoBehaviour
{
    [SerializeField]
    bool renderDebug = false;
    [SerializeField]    // The radius of the circle that fits in the map
    float circleRadius = 10.0f;


    // Getters
    public float GetMaxSizeRadius()
    {
        return circleRadius;
    }
    public Vector2 GetPosition()
    {
        return transform.position;
    }


    // Debugging
    private void OnDrawGizmos()
    {
        if (!renderDebug)
            return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, circleRadius);
    }
}
