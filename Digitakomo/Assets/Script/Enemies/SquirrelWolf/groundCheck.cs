using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheck : MonoBehaviour
{
    public SquirrelWolf attachedObj = null;
    [System.NonSerialized]
    public bool startCheck = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (startCheck == false)
            return;
        if (collision.gameObject.layer != LayerMask.GetMask("Ground"))
            return;

        attachedObj.isGrounded = true;
        startCheck = false;
    }
}
