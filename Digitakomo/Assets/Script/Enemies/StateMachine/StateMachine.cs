using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    BaseState currentState = null;

    Dictionary<string, BaseState> stateDictionary = new Dictionary<string, BaseState>();



    public bool RegisterNewState(BaseState newState, string keyOfState, bool setAsStart = false)
    {
        // Key already Exists?
        if (stateDictionary.ContainsKey(keyOfState))
            return false;
        // Add into dictionary
        stateDictionary.Add(keyOfState, newState);
        // Set as current State
        if (setAsStart)
            SetNewState(keyOfState);

        return true;
    }

    public bool SetNewState(string keyOfState)
    {
        // State Exists?
        if (!stateDictionary.ContainsKey(keyOfState))
            return false;

        // Call Leave State
        if (currentState != null)
            currentState.LeaveState();
        // Set to new State and call Enter State
        currentState = stateDictionary[keyOfState];
        currentState.EnterState();
        
        return true;
    }

    public void UpdateCurrentState()
    {
        if (currentState == null)
            return;

        currentState.UpdateState();
    }
}
