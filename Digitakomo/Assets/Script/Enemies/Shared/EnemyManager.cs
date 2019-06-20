using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance = null;

    
    // List used to contain all the spawning Zones
    [SerializeField]
    List<SpawnZone> listOfSpawnZones = new List<SpawnZone>();
    
    // Awake
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

}
