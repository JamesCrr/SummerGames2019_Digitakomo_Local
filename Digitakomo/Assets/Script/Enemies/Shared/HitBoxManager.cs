using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxManager : MonoBehaviour
{
    [SerializeField]
    List<Collider2D> listOfColliders = new List<Collider2D>();
    List<int> activeColliders = new List<int>();



    private void Awake()
    {
        // Disable all the hitBoxes first
        foreach (Collider2D item in listOfColliders)
        {
            item.enabled = false;
        }
    }

    // Enable a new Collider and disables th currently active ones
    public void EnableNewCollider(int colliderID)
    {
        if (!CheckValid(colliderID))
            return;
        DisableActiveColliders();
        // Enable
        listOfColliders[colliderID].enabled = true;
        activeColliders.Add(colliderID);
    }

    // Disable all active colliders
    public void DisableActiveColliders()
    {
        foreach (int item in activeColliders)
        {
            listOfColliders[item].enabled = false; 
        }
        activeColliders.Clear();
    }
    // Check if the ID passed in, exists
    bool CheckValid(int ID)
    {
        if (ID < 0 || ID >= listOfColliders.Count)
            return false;
        return true;
    }
}
