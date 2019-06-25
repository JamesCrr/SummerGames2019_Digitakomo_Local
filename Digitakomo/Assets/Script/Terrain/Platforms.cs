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

    BoxCollider2D myCollider;
    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();

        float width = myCollider.size.x / 2;
        float height = myCollider.size.y / 2;

        // X Axis
        leftPoint.x = gameObject.transform.position.x - (width * transform.localScale.x) + 0.3f;
        rightPoint.x = gameObject.transform.position.x + (width * transform.localScale.x) - 0.3f;
        // Y Axis
        leftPoint.y = gameObject.transform.position.y + (height * transform.localScale.y) + 0.3f;
        rightPoint.y = gameObject.transform.position.y + (height * transform.localScale.y) + 0.3f;

        // set the collider's position
        leftCollider.gameObject.transform.position = leftPoint;
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


    // Returns which point on this Platform that is closest 
    // to the position passed in
    public Vector2 GetClosestPosition(Vector2 targetPos)
    {
        if ((targetPos - leftPoint).sqrMagnitude > (targetPos - rightPoint).sqrMagnitude)
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

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;

    //    Gizmos.DrawWireSphere(leftPoint, 0.3f);
    //    Gizmos.DrawWireSphere(rightPoint, 0.3f);
    //}
}
