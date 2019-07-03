using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AWGroundCheck : MonoBehaviour
{
    public AWScript attachedObj = null;
    [System.NonSerialized]
    public Platforms platformStandingOn = null;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
            return;

        // attach platform if have
        if (collision.GetComponent<Platforms>())
            platformStandingOn = collision.GetComponent<Platforms>();
        // We landed on the ground
        attachedObj.LandedGround();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
            return;
        // Left the ground
        attachedObj.LeftGround();
    }
}
