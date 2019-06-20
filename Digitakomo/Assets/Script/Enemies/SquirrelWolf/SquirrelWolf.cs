using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelWolf : EnemyBaseClass
{
    
    [Header("SquirrelWolf Class")]
    [SerializeField]
    // How far to detect for player
    float playerDetectionRange = 0.0f;


    // Unity Stuff
    // Get referrence to the players
    GameObject player1Object = null;
    GameObject player2Object = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void ResetEnemy(SpawnZone newSpawnZone, Vector3 newPos)
    {
        
    }
}
