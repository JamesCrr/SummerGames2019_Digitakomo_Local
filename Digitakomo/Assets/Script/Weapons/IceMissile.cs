using UnityEngine;

public class IceMissile : Weapon
{
    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<FlameProjectile>() != null)
        {
            // TODO // if player far enough

            // Create Steam
            GameObject go = ObjectPooler.Instance.FetchGO("Steam");
            go.transform.position = transform.position;

        }

        if (collision.gameObject.GetComponent<Steam>() != null)
        {
            // hit steam
            gameObject.SetActive(false);
        }
    }
}
