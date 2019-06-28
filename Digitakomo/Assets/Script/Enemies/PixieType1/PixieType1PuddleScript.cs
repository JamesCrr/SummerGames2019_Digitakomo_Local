using UnityEngine;

public class PixieType1PuddleScript : MonoBehaviour
{
    [SerializeField]
    float MinDamage = 15f;
    float MaxDamage = 20f;

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
        // hit player
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<IDamagable>().TakeDamage(Random.Range(MinDamage, MaxDamage));
        }

        //gameObject.SetActive(false);
    }
}
