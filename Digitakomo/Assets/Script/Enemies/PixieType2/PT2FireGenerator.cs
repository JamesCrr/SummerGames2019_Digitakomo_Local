using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PT2FireGenerator : MonoBehaviour
{
    [SerializeField]    // The Fire Prefab to Create when we hit the ground
    GameObject firePrefab = null;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only spawn on platforms or ground
        if (collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
            return;

        Vector2 newPos = transform.position;
        GameObject newObj = ObjectPooler.Instance.FetchGO(firePrefab.name);
        Platforms platformComponent = collision.GetComponent<Platforms>();
        SoundManager.instance.PlaySound("FirePillar");

        // Platform or ground?
        if (platformComponent != null)
            newPos.y = platformComponent.GetPlatformSurface();

        // apply the new pos
        newObj.transform.position = newPos;

        // parent the fire under the platform
        // just in case the platform is moving
        newObj.transform.parent = collision.gameObject.transform;
    }
}
