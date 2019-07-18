using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    // The Colliders for jumping detection
    [SerializeField]
    Collider2D leftCollider = null;
    [SerializeField]
    Collider2D rightCollider = null;
    // To Store the platform ending points
    Vector2 leftPoint;
    Vector2 rightPoint;
    float width;
    float height;

    BoxCollider2D myCollider;
    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();

        width = myCollider.size.x / 2;
        height = myCollider.size.y / 2;

        // 0.8f, and 0.5f for how much to move 
        float xRatio = (0.3f * (transform.localScale.x / 1f));
        float yRatio = (1.75f * (transform.localScale.y / 1f));

        leftPoint.y = myCollider.bounds.max.y + yRatio;
        rightPoint.y = myCollider.bounds.max.y + yRatio;

        leftPoint.x = myCollider.bounds.min.x + xRatio;
        rightPoint.x = myCollider.bounds.max.x - xRatio;

        // set the collider's position
        if(leftCollider != null)
            leftCollider.gameObject.transform.position = leftPoint;
        if(rightCollider != null)
            rightCollider.gameObject.transform.position = rightPoint;
    }


    // Getters
    public Vector2 GetLeftPoint()
    {
        return leftPoint;
    }
    public Vector2 GetRightsPoint()
    {
        return rightPoint;
    }
    public GameObject GetLeftGO()
    {
        return leftCollider.gameObject;
    }
    public GameObject GetRightGO()
    {
        return rightCollider.gameObject;
    }
    public float GetPlatformSurface()
    {
        return myCollider.bounds.max.y;
    }


    // Returns which point on this Platform that is closest 
    // to the position passed in
    public Vector2 GetClosestPosition(Vector2 targetPos)
    {
        if (Mathf.Abs((targetPos - leftPoint).sqrMagnitude) > Mathf.Abs((targetPos - rightPoint).sqrMagnitude))
            return rightPoint;
        else
            return leftPoint;
    }
    // Returns which point on this Platform that is closest 
    // to the position passed in
    // WITH AN OFFSET
    public Vector2 GetClosestPosition_Offset(Vector2 targetPos)
    {
        Vector2 finalPos;
        if ((targetPos - leftPoint).sqrMagnitude > (targetPos - rightPoint).sqrMagnitude)
            finalPos = rightPoint + Vector2.right;
        else
            finalPos = leftPoint - Vector2.right;

        return finalPos;
    }
    // Returns which point on this Platform that is furtherest 
    // to the position passed in
    public Vector2 GetFurtherestPosition(Vector2 targetPos)
    {
        if ((targetPos - leftPoint).sqrMagnitude < (targetPos - rightPoint).sqrMagnitude)
            return rightPoint;
        else
            return leftPoint;
    }

}
