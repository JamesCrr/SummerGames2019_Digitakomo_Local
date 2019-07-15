using UnityEngine;

public class IceMissile : RangeWeapon
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<FlameProjectile>() != null)
        {
            // TODO // if player far enough

            // Create Steam
            GameObject go = ObjectPooler.Instance.FetchGO("Steam");
            go.transform.position = transform.position;
            Steam steam = go.GetComponent<Steam>();
            steam.Restart();
        }

        if (collision.gameObject.GetComponent<Steam>() != null)
        {
            // hit steam
            gameObject.SetActive(false);
        }
    }

    public override void Restart()
    {
        base.Restart();
        SoundManager.instance.PlaySound("IceMissile");
    }
}
