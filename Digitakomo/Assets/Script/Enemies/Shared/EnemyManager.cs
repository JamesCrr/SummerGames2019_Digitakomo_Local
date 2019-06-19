using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance = null;

    // Class used to contain the spawning Template
    [System.Serializable]
    class SpawningTemplate
    {
        // The Enemy Prefab to spawn when timer reaches 0
        public GameObject enemyToSpawn = null;
        // How often to spawn the Enemy
        [Header("Spawn Intervals")]
        public float spawnInterval = 0.0f;
        float spawnTimer = 1.0f;
        [Header("Max Spawn Count")]
        public int maxSpawnCount = 1;
        int spawnCounter = 0;
        // Where to spawn the Enemy
        [Header("Spawn Zone")]
        public SpawnZone spawningZone;
        // Debug
        [Header("DEBUG")]
        public bool renderDebug = false;


        // Updates the Spawn Timer and when timer reaches 0
        // Returns true, if not always returns false
        public bool UpdateTimer()
        {
            // Have we spawned all enemies
            if (spawnCounter == maxSpawnCount)
                return false;
            // Count down Timer
            spawnTimer -= Time.deltaTime;
            if (spawnTimer > 0.0f)
                return false;

            // Increment Counter
            spawnCounter++;
            // Reset Timer
            spawnTimer = spawnInterval;
            return true;
        }
        // Returns a random position from the spawning Zone, if provided.
        // If not, returns Vector3.zero;
        public Vector3 GetRandomPositionFromZone()
        {
            // If not center point to referrnce from
            if (spawningZone.centerPoint == null)
                return Vector3.zero;
            // Get a random position
            float xPos = Random.Range(spawningZone.centerPoint.position.x - spawningZone.zoneDimensions.x, spawningZone.centerPoint.position.x + spawningZone.zoneDimensions.x);
            float yPos = Random.Range(spawningZone.centerPoint.position.y - spawningZone.zoneDimensions.y, spawningZone.centerPoint.position.y + spawningZone.zoneDimensions.y);
            // Return the new position
            return new Vector3(xPos, yPos, 1.0f);
        }
        // Used to check if we have spawned all the enemies from this template
        public bool SpawnedAllEnemies()
        {
            if (spawnCounter >= maxSpawnCount)
                return true;
            return false;
        }
        // Returns if we want to draw the Debug Zone
        public bool GetRenderDebug()
        {
            return renderDebug;
        }
    }

    // List used to contain all the spawning Template
    [SerializeField]
    List<SpawningTemplate> listOfSpawns = new List<SpawningTemplate>();
    
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // Update Timer for all the Spawning Templates
        for (int i = 0; i < listOfSpawns.Count; ++i)
        {
            // have we spawned all enemies?
            if(!listOfSpawns[i].SpawnedAllEnemies())
            {
                // is it time to spawn and send the RPC call?
                if(listOfSpawns[i].UpdateTimer())
                {
                    
                    // Create new enemy
                    //GameObject newEnemy = Instantiate(listOfSpawns[i].enemyToSpawn, listOfSpawns[i].GetRandomPositionFromZone(), Quaternion.identity);

                    // Get a random path from the zone that you just spawned
                    //newEnemy.gameObject.GetComponent<PixieType1Script>().SetWaypointGroup(listOfSpawns[i].spawningZone.GetRandomPath());
   
                }
            }
                

        }        
    }


}
