using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheck : MonoBehaviour
{
    public SquirrelWolf attachedObj = null;
    [System.NonSerialized]
    public bool startCheck = true;
    [System.NonSerialized]
    public Platforms platformStandingOn = null;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (startCheck == false)
            return;
        if (collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
            return;

        // attach platform if have
        if (collision.GetComponent<Platforms>())
            platformStandingOn = collision.GetComponent<Platforms>();
        // modify data
        attachedObj.SetGrounded();
        startCheck = false;

        // Call the wolf function reacting function
        attachedObj.HitGround();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (startCheck == false)
            return;
        if (collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
            return;

        // attach platform if have
        if (collision.GetComponent<Platforms>())
            platformStandingOn = collision.GetComponent<Platforms>();
        // modify data
        attachedObj.SetGrounded();
        startCheck = false;

        // Call the wolf function reacting function
        attachedObj.HitGround();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
            return;

        attachedObj.LeftGrounded();
        startCheck = true;
    }

}
