using UnityEngine;

public class Tuonela : BaseItem
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        IceCharacter ic = collision.gameObject.GetComponent<IceCharacter>();
        if (ic != null)
        {
            ic.AddEnergy(Energy);
            gameObject.SetActive(false);
        }
    }
}
