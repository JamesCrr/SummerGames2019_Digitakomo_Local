using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    #region Enemies To Spawn
    // Class used to contain the spawning Template
    [System.Serializable]
    class SpawningTemplate
    {
        public bool SpawnOnStart = true;
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
        [System.NonSerialized]
        public SpawnZone spawningZone = null;


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

        public void UpdateTimerOnStart()
        {
            if (!SpawnOnStart)
            {
                spawnTimer = spawnInterval;
            }
        }
        // Returns a random position from the spawning Zone, if provided.
        // If not, returns Vector3.zero;
        public Vector3 GetRandomPositionFromZone()
        {
            // If not center point to referrnce from, then just spawn from me
            if (spawningZone.centerPoint == null)
                return spawningZone.transform.position;
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
    }
    [Header("Enemies To spawn")]
    [SerializeField]
    // The Enemies to spawn
    List<SpawningTemplate> listOfEnemies = new List<SpawningTemplate>();
    #endregion  

    // Zone Related Data
    [Header("Zone Width and Height")]
    public Transform centerPoint = null;
    public Vector2 zoneDimensions = Vector2.zero;
    public bool renderDebugBox = false;
    // What paths can we take after spawning from this zone
    [Header("Waypoint Groups to use")]
    public List<WaypointGroup> listOfAvaliablePaths = new List<WaypointGroup>();


    private void Start()
    {
        // If no center point has been referrence yet, use this as referrence
        if (centerPoint == null)
            centerPoint = this.transform;

        // Attach this's transform to all the templates
        foreach (SpawningTemplate item in listOfEnemies)
        {
            item.UpdateTimerOnStart();
            item.spawningZone = this;
        }
    }


    private void Update()
    {
        // Update the timers for the enemy spawning template
        foreach (SpawningTemplate spawn in listOfEnemies)
        {
            // have we spawned all enemies?
            if (!spawn.SpawnedAllEnemies())
            {
                // is it time to spawn
                if (spawn.UpdateTimer())
                {
                    // Fetch enemy
                    GameObject newEnemy = ObjectPooler.Instance.FetchGO(spawn.enemyToSpawn.name);
                    // Attach SpawnZone and Reset the Enemy before anything
                    //newEnemy.GetComponent<EnemyBaseClass>().ResetEnemy(spawn.spawningZone, spawn.GetRandomPositionFromZone());

                    EnemyBaseClass baseClass = newEnemy.GetComponent<EnemyBaseClass>();
                    if (baseClass == null)
                        baseClass = newEnemy.GetComponentInChildren<EnemyBaseClass>();

                    baseClass.ResetEnemy(spawn.spawningZone, spawn.GetRandomPositionFromZone());
                }
            }
        }


    }



    // Returns you a WaypointGroup ID from this zone
    public int GetRandomPath()
    {
        int rand = Random.Range(0, listOfAvaliablePaths.Count);

        return listOfAvaliablePaths[rand].groupID;
    }


    // Debug
    private void OnDrawGizmos()
    {
        if (!renderDebugBox)
            return;
        if (centerPoint == null)
            centerPoint = this.transform;

        // Change the color
        Gizmos.color = new Color(1, 0, 0, 0.5f);

        Gizmos.DrawWireCube(centerPoint.position, zoneDimensions * 2.0f);
    }
}
