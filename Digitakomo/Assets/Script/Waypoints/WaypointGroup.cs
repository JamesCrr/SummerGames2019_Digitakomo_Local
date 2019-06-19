using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointGroup : MonoBehaviour
{
    [Header("Group Settings")]
    [SerializeField]
    bool groundedPath = false;
    // Used to contain the group of waypoints
    public List<Transform> listOfPoints = new List<Transform>();
    // Used to identify the groups
    [System.NonSerialized]
    public int groupID;
    // DEBUG
    [SerializeField]
    bool drawDebugLine = false;


    // Start is called before the first frame update
    void Start()
    {
        // If not added, add all children in
        if(listOfPoints.Count == 0)
        {
            foreach (Transform child in transform)
            {
                listOfPoints.Add(child);
            }
        }    
    }

    // Draw Debug Lines
    private void OnDrawGizmos()
    {
        if (!drawDebugLine)
            return;

        // set color
        Gizmos.color = Color.green;
        //// draw first line
        //if (listOfPoints.Count > 0)
        //    Gizmos.DrawLine(transform.position, listOfPoints[0].position);
        // draw connecting line
        for (int i = 0; i < listOfPoints.Count; ++i)
        {
            if (i + 1 >= listOfPoints.Count || listOfPoints[i] == null)
                break;

            Gizmos.DrawLine(listOfPoints[i].position, listOfPoints[i + 1].position);
        }
    }


    // Get Whether this path is grouded
    public bool GetPathGrounded()
    {
        return groundedPath;
    }
}
