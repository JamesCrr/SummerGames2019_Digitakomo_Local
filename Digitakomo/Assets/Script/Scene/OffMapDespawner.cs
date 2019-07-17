using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class used to despawn any objects that fly
// to far from the map
public class OffMapDespawner : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.transform.parent != null)
            return;

        DespawnObject(collision.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        DespawnObject(collision.gameObject);
    }


    // Despawning Logic
    void DespawnObject(GameObject collidedObj)
    {
        if (collidedObj.GetComponent<EnemyBaseClass>() != null)
        {
            collidedObj.GetComponent<EnemyBaseClass>().ModifyHealth(-collidedObj.gameObject.GetComponent<EnemyBaseClass>().GetCurrentHP());
        }
        else if (collidedObj.GetComponent<Character>() != null)
        {
            collidedObj.GetComponent<Character>().TakeDamage(collidedObj.GetComponent<Character>().GetCurrentHP());
            collidedObj.SetActive(false);
        }
        else
        {
            collidedObj.SetActive(false);
        }
    }
}
