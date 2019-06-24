using UnityEngine;

public class Steam : MonoBehaviour
{
    private int firecount = 0;
    private int icecount = 0;

    public float addSize = 0.1f;

    public float grapTime = 0.3f;
    private float expiredTime;

    public bool isElectric = false;

    private Vector3 defaultScale;
    private void Awake()
    {
        defaultScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void Start()
    {
        expiredTime = Time.time + grapTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= expiredTime)
        {
            if (firecount > icecount)
            {
                firecount = icecount;
            }
            else
            {
                icecount = firecount;
            }
        }
        if (firecount > icecount)
        {
            // use icecount as scale
            transform.localScale = new Vector3(defaultScale.x + (icecount * addSize), defaultScale.y + (icecount * addSize), defaultScale.z);
        }
        else
        {
            // use firecount as scale
            transform.localScale = new Vector3(defaultScale.x + (firecount * addSize), defaultScale.y + (firecount * addSize), defaultScale.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<IceMissile>() != null)
        {
            expiredTime = Time.time + grapTime;
            icecount += 1;
        }
        else if (collision.gameObject.GetComponent<FlameProjectile>() != null)
        {
            expiredTime = Time.time + grapTime;
            firecount += 1;
        }

        if (collision.gameObject.name == "AttackCollider")
        {
            ElectricHand eh = collision.gameObject.GetComponent<ElectricHand>();
            if (eh.getElectricHand())
            {
                Debug.Log("Electric touch");
                isElectric = true;
                eh.setElectricHand(false);
            }
        }
    }

    public void Restart()
    {
        transform.localScale = defaultScale;
        expiredTime = Time.time + grapTime;
        isElectric = false;
    }
}
