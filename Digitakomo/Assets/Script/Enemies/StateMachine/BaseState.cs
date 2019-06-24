using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    protected GameObject ownerObj = null;
    protected StateMachine ownerSM = null;

    // Constructor
    public BaseState(GameObject newGO, StateMachine newSM)
    {
        ownerObj = newGO;
        ownerSM = newSM;
    }

    // Main Loop to Update State Logic
    public abstract void UpdateState();

    // Optional Functions
    public virtual void EnterState() { }
    public virtual void LeaveState() { }
}
