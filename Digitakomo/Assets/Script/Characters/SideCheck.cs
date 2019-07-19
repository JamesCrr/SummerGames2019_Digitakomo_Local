using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideCheck : MonoBehaviour
{
    [SerializeField]
    LayerMask whatIsGround;
    // public Vector3 radius;
    public bool isHit;
    Character character;
    public Vector3 size;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponentInParent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        // isHit = Physics2D.OverlapCircle(transform.position, groundCheckRadius, whatIsGround);
        // Physics2D.OverlapArea(new Vector2(transform.position.x + ) )


        // Collider2D[] collides = Physics2D.OverlapBoxAll(transform.position, transform.localScale / 2, whatIsGround);



        isHit = Physics2D.OverlapArea(transform.position - size, transform.position + size, whatIsGround);
        Debug.DrawLine(transform.position - size, transform.position + size);
        // Debug.Log(Physics2D.OverlapArea(transform.position - size, transform.position + size, whatIsGround));
        // foreach (Collider2D col in collides)
        // {
        //     if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        //     {
        //         isHit = true;
        //     }
        // }

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
}
