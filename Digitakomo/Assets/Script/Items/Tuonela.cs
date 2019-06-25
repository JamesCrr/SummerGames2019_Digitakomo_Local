using UnityEngine;

public class Tuonela : BaseItem
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IceCharacter ic = collision.gameObject.GetComponent<IceCharacter>();
        FireCharacter fc = collision.gameObject.GetComponent<FireCharacter>();
        if (ic != null)
        {
            ic.AddEnergy(Energy);
            gameObject.SetActive(false);
        }
        else if (fc != null)
        {
            Physics2D.IgnoreCollision(fc.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        }
    }
}
