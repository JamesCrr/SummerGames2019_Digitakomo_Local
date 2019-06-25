using UnityEngine;

public class GoldenFeather : BaseItem
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        FireCharacter fc = collision.gameObject.GetComponent<FireCharacter>();
        IceCharacter ic = collision.gameObject.GetComponent<IceCharacter>();
        if (fc != null)
        {
            fc.AddEnergy(Energy);
            gameObject.SetActive(false);
        }
        else if (ic != null)
        {
            Physics2D.IgnoreCollision(ic.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        }
    }
}
