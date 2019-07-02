using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager
{
    // Who has all these status effects
    GameObject owner = null;
    // Store all the status effects
    Dictionary<string, BaseStatusEffect> activeStatus = new Dictionary<string, BaseStatusEffect>();
    // Store all the status effects to remove
    List<string> toBeRemoved = new List<string>();


    // Constructor
    public StatusEffectManager(GameObject ownerObj)
    {
        owner = ownerObj;
    }



    public void Update()
    {
        // Update each status
        foreach (KeyValuePair<string,BaseStatusEffect> item in activeStatus)
        {
            // When the status effects are done
            if(!item.Value.UpdateEffect())
            {
                // Call onLeaves
                item.Value.onLeave();
                // Remove from the Dictionary by adding to list of toBeRemoved
                toBeRemoved.Add(item.Key);
            }
        }
        // Remove status that are done from the dictionary
        foreach (string item in toBeRemoved)
        {
            activeStatus.Remove(item);
        }
        toBeRemoved.Clear();
    }

    // Function to add or reapply a Status Effect
    public bool AddEffect(string key, BaseStatusEffect newEffect, EnemyBaseClass newOwner)
    {
        // If status effect is already in, then reset and return
        if (activeStatus.ContainsKey(key))
        {
            activeStatus[key].onReApply();
            return false;
        }
        // Not in dictionary, so add
        activeStatus.Add(key, newEffect);
        // call onApply as we just added it
        newEffect.onApply(newOwner);
        return true;
    }
    // Function to check if we already have that effect
    public bool EffectAlreadyIn(string key)
    {
        return activeStatus.ContainsKey(key);
    }
}
