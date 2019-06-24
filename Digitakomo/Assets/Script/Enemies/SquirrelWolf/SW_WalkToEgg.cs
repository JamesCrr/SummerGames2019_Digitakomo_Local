using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SW_WalkToEgg : BaseState
{
    Egg eggInstance = null;

    // Constructor
    public SW_WalkToEgg(GameObject go, StateMachine mySM) : base(go, mySM)
    {
        
    }


    public override void EnterState()
    {
        eggInstance = Egg.Instance;    
    }
    public override void UpdateState()
    {

    }
}
