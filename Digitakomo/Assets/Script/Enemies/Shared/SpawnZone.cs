using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    [Header("Zone Width and Height")]
    public Transform centerPoint = null;
    public Vector2 zoneDimensions = Vector2.zero;
    public bool renderDebugBox = false;
    // What paths can we take after spawning from this zone
    [Header("Waypoint Groups to use")]
    public List<WaypointGroup> listOfAvaliablePaths = new List<WaypointGroup>();

    
    // Returns you a WaypointGroup ID from this zone
    public int GetRandomPath()
    {
        int rand = Random.Range(0, listOfAvaliablePaths.Count);

        return listOfAvaliablePaths[rand].groupID;
    }


    // Debug
    private void OnDrawGizmos()
    {
        if (!renderDebugBox)
            return;
        if (centerPoint == null)
            centerPoint = this.transform;

        // Change the color
        Gizmos.color = new Color(1, 0, 0, 0.5f);

        Gizmos.DrawWireCube(centerPoint.position, zoneDimensions * 2.0f);
    }
}
