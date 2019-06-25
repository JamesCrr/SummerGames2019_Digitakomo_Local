using UnityEngine;

public class GoldenFeather : BaseItem
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        FireCharacter fc = collision.gameObject.GetComponent<FireCharacter>();
        if (fc != null)
        {
            fc.AddEnergy(Energy);
            gameObject.SetActive(false);
        }
    }
}
