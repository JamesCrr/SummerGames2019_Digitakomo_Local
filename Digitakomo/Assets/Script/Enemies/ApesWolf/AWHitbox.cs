using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AWHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Collide with player?
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;

        Debug.LogWarning("HIT PLAYER");
    }

}
