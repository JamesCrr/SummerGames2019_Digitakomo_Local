using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour
{
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
        leftPoint.x = gameObject.transform.position.x - (width * transform.localScale.x) + 0.1f;
        rightPoint.x = gameObject.transform.position.x + (width * transform.localScale.x) - 0.1f;
        // Y Axis
        leftPoint.y = gameObject.transform.position.y + (height * transform.localScale.y) + 0.1f;
        rightPoint.y = gameObject.transform.position.y + (height * transform.localScale.y) + 0.1f;
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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(leftPoint, 0.3f);
        Gizmos.DrawWireSphere(rightPoint, 0.3f);
    }
}
