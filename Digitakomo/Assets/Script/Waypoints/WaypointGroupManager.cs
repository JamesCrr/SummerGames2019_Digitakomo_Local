using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointGroupManager : MonoBehaviour
{
    public static WaypointGroupManager instance = null;

    // Create a struct used to return all the needed waypoint data
    public struct WaypointReturnData
    {
        public int nextWaypointIndex;
        public Vector3 nextPosition;

        public void Clear()
        {
            nextWaypointIndex = -1;
            nextPosition = Vector3.zero;
        }
    };
    // The actual Variable
    WaypointReturnData waypointDataHolder = new WaypointReturnData();
    // Stores all the waypoint groups
    [SerializeField]
    List<WaypointGroup> listOfGroups = new List<WaypointGroup>();


    private void Awake()
    {
        // Make sure object is singleton
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        // Attach Singleton
        instance = this;

        // Set the ID for the groups
        for(int i = 0; i < listOfGroups.Count; ++i)
        {
            listOfGroups[i].groupID = i;
            listOfGroups[i].gameObject.name = (i).ToString();
        }
    }


    // Returns a waypoint from a group
    public Transform GetWaypoint(int groupID, int waypointID)
    {
        // check valid
        if (!GetValidGroup(groupID) || !GetValidWaypoint(groupID, waypointID))
            return null;

        // returns the waypoint
        return listOfGroups[groupID].listOfPoints[waypointID];
    }
    // Returns the next waypoint that we need
    public WaypointReturnData GetNextWaypoint_Wrapped(int groupID, int currentWaypointID)
    {
        // check valid
        if (!GetValidGroup(groupID))
        {
            waypointDataHolder.Clear();
            return waypointDataHolder;
        }

        // Check if we need to wrap around or just add one
        if (currentWaypointID + 1 >= listOfGroups[groupID].listOfPoints.Count)
            waypointDataHolder.nextWaypointIndex = 0;
        else
            waypointDataHolder.nextWaypointIndex = currentWaypointID + 1;
        // Record down the new waypoint position
        waypointDataHolder.nextPosition = listOfGroups[groupID].listOfPoints[waypointDataHolder.nextWaypointIndex].position;


        return waypointDataHolder;
    }





    // Returns true if Group is valid
    // Returns false if Group is null or doesn't exist
    bool GetValidGroup(int groupID)
    {
        // Check if group valid
        if (groupID >= listOfGroups.Count || listOfGroups[groupID] == null)
        {
            Debug.LogError("Waypoint_GROUP Does not exist!");
            return false;
        }
        return true;
    }
    // Returns true if waypoint is valid
    // Returns false if waypoint is null or doesn't exist
    bool GetValidWaypoint(int groupID, int waypointID)
    {
        // Check if waypoint valid
        if (waypointID >= listOfGroups[groupID].listOfPoints.Count || listOfGroups[groupID].listOfPoints[waypointID] == null)
        {
            Debug.LogError("WAYPOINT Does not exist!");
            return false;
        }
        return true;
    }
}
