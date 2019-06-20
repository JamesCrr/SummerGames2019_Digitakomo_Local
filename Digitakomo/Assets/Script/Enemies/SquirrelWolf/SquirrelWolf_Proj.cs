using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelWolf_Proj : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If we are colliding with enemy, don't anyting
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyProj")
            return;

        gameObject.SetActive(false);
    }
}
