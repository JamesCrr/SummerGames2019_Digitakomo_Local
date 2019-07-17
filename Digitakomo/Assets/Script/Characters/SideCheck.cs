using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideCheck : MonoBehaviour
{
    [SerializeField]
    LayerMask whatIsGround = 0;     // What layer to detect as ground    public bool isBack = true;
    public Vector3 radius;
    public bool isHit;
    public Character character;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponentInParent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        // isHit = Physics2D.OverlapCircle(transform.position, groundCheckRadius, whatIsGround);
        Collider2D[] collides = Physics2D.OverlapBoxAll(transform.position, transform.localScale / 2, whatIsGround);
        isHit = false;
        foreach (Collider2D col in collides)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                isHit = true;
            }
        }

        // // if left
        if (character.transform.position.x > this.transform.position.x)
        {
            // hit left
            if (isHit)
            {
                character.moveLeftAble = false;
            }
            else
            {
                character.moveLeftAble = true;
            }
        }
        if (character.transform.position.x < this.transform.position.x)
        {
            // hit right
            if (isHit)
            {
                character.moveRightAble = false;
            }
            else
            {
                character.moveRightAble = true;
            }
        }
    }

    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
    //     //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
    //     Gizmos.DrawWireCube(transform.position, transform.localScale);
    // }
}
