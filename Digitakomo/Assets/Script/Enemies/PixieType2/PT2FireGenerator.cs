using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PT2FireGenerator : MonoBehaviour
{
    [SerializeField]    // The Fire Prefab to Create when we hit the ground
    GameObject firePrefab;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        ObjectPooler.Instance.FetchGO_Pos(firePrefab.name, transform.position);    
    }
}
